package cert

import (
	"crypto"
	"crypto/ecdsa"
	"crypto/elliptic"
	"crypto/rand"
	"crypto/rsa"
	"crypto/x509"
	"encoding/pem"
	"fmt"
	"io/ioutil"
	"os"

	"github.com/go-acme/lego/v3/certcrypto"
	"github.com/go-acme/lego/v3/certificate"
	"github.com/go-acme/lego/v3/challenge/http01"
	"github.com/go-acme/lego/v3/challenge/tlsalpn01"
	"github.com/go-acme/lego/v3/lego"
	"github.com/go-acme/lego/v3/registration"
	"github.com/p4gefau1t/trojan-go/common"
)

var caDir string = "https://acme-v02.api.letsencrypt.org/directory"
var tlsPort string = "443"
var httpPort string = "80"

type User struct {
	Email        string
	Registration *registration.Resource
	Key          crypto.PrivateKey
}

func (u *User) GetEmail() string {
	return u.Email
}

func (u User) GetRegistration() *registration.Resource {
	return u.Registration
}

func (u *User) GetPrivateKey() crypto.PrivateKey {
	return u.Key
}

func createAndSaveUserKey() (*ecdsa.PrivateKey, error) {
	_, err := os.Stat("user.key")
	if os.IsExist(err) {
		return nil, common.NewError("user.key exists, cannot create new user")
	}
	userKey, err := ecdsa.GenerateKey(elliptic.P256(), rand.Reader)
	common.Must(err)
	userKeyFile, err := os.Create("user.key")
	if err != nil {
		return nil, common.NewError("failed to create user key file").Base(err)
	}
	defer userKeyFile.Close()

	x509Encoded, _ := x509.MarshalECPrivateKey(userKey)
	pemEncoded := pem.EncodeToMemory(&pem.Block{Type: "PRIVATE KEY", Bytes: x509Encoded})
	userKeyFile.Write(pemEncoded)
	return userKey, nil
}

func loadUserKey() (*ecdsa.PrivateKey, error) {
	pemEncoded, err := ioutil.ReadFile("user.key")
	if err != nil {
		return nil, common.NewError("failed to load user's key").Base(err)
	}
	block, _ := pem.Decode([]byte(pemEncoded))
	if block == nil {
		return nil, common.NewError("failed to parse user's key").Base(err)
	}
	x509Encoded := block.Bytes
	return x509.ParseECPrivateKey(x509Encoded)
}

func saveServerKeyAndCert(cert *certificate.Resource) error {
	ioutil.WriteFile("server.key", cert.PrivateKey, os.ModePerm)
	ioutil.WriteFile("server.crt", cert.Certificate, os.ModePerm)
	return nil
}

func loadServerKey() (*rsa.PrivateKey, error) {
	keyBytes, err := ioutil.ReadFile("server.key")
	if err != nil {
		return nil, err
	}
	block, _ := pem.Decode(keyBytes)
	serverKey, err := x509.ParsePKCS1PrivateKey(block.Bytes)
	if err != nil {
		return nil, err
	}
	return serverKey, nil
}

func obtainCertificate(domain, email string, userKey *ecdsa.PrivateKey, serverKey crypto.PrivateKey) (*certificate.Resource, error) {
	// Create a user. New accounts need an email and private key to start.
	user := User{
		Email: email,
		Key:   userKey,
	}

	config := lego.NewConfig(&user)

	// This CA URL is configured for a local dev instance of Boulder running in Docker in a VM.
	config.CADirURL = caDir
	config.Certificate.KeyType = certcrypto.RSA2048

	// A client facilitates communication with the CA server.
	client, err := lego.NewClient(config)
	if err != nil {
		return nil, err
	}

	// We specify an http port of 5002 and an tls port of 5001 on all interfaces
	// because we aren't running as root and can't bind a listener to port 80 and 443
	// (used later when we attempt to pass challenges). Keep in mind that you still
	// need to proxy challenge traffic to port 5002 and 5001.
	err = client.Challenge.SetHTTP01Provider(http01.NewProviderServer("", httpPort))
	if err != nil {
		return nil, err
	}
	err = client.Challenge.SetTLSALPN01Provider(tlsalpn01.NewProviderServer("", tlsPort))
	if err != nil {
		return nil, err
	}

	reg, err := client.Registration.Register(registration.RegisterOptions{TermsOfServiceAgreed: true})
	if err != nil {
		return nil, err
	}
	user.Registration = reg

	request := certificate.ObtainRequest{
		Domains:    []string{domain},
		Bundle:     false,
		PrivateKey: serverKey,
	}
	certificates, err := client.Certificate.Obtain(request)
	if err != nil {
		return nil, err
	}

	// Each certificate comes back with the cert bytes, the bytes of the client's
	// private key, and a certificate URL. SAVE THESE TO DISK.
	fmt.Println("certificates obtained for:", certificates.Domain)

	return certificates, nil
}

func isFilesExist(nameList []string) bool {
	fileInfo, err := ioutil.ReadDir("./")
	common.Must(err)
	for _, v := range fileInfo {
		name := v.Name()
		for _, u := range nameList {
			if name == u {
				return true
			}
		}
	}
	return false
}

func RequestCert(domain, email string) error {
	if isFilesExist([]string{"server.key", "server.crt"}) {
		return common.NewError("cert files(server.key, server.crt) already exist")
	}
	userKey, err := loadUserKey()
	if err != nil {
		fmt.Println("failed to load user key, trying to create one..")
		userKey, err = createAndSaveUserKey()
		if err != nil {
			return err
		}
	} else {
		fmt.Println("found user.key, using exist user key")
	}
	cert, err := obtainCertificate(domain, email, userKey, nil)
	if err != nil {
		return err
	}
	if err := saveServerKeyAndCert(cert); err != nil {
		return common.NewError("failed to save cert").Base(err)
	}
	return nil
}

func RenewCert(domain, email string) error {
	serverKey, err := loadServerKey()
	if err != nil {
		return err
	}
	userKey, err := loadUserKey()
	if err != nil {
		return err
	}
	cert, err := obtainCertificate(domain, email, userKey, serverKey)
	if err != nil {
		return err
	}
	if err := saveServerKeyAndCert(cert); err != nil {
		return common.NewError("failed to save cert").Base(err)
	}
	return nil
}
