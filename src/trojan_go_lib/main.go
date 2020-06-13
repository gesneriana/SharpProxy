package main

/*
#include <stdio.h>
#include <stdlib.h>
*/
import "C"
import (
	"encoding/base64"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"os"
	"runtime"
	"sync/atomic"
	"unsafe"

	"github.com/p4gefau1t/trojan-go/cert"
	"github.com/p4gefau1t/trojan-go/common"
	"github.com/p4gefau1t/trojan-go/easy"
	"github.com/p4gefau1t/trojan-go/log"
	"github.com/p4gefau1t/trojan-go/proxy"

	_ "github.com/p4gefau1t/trojan-go/build"
)

func main() {
	// Need a main function to make CGO compile package as C shared library
}

var reentrance int64

// Start 启动代理服务,设置可读写的配置文件的目录
//export Start
func Start(b64 *C.char) {
	var b64String = C.GoString(b64)
	decodeBytes, err := base64.StdEncoding.DecodeString(b64String)
	if err != nil {
		log.Info(b64String)
		log.Error(err)
		return
	}

	go func() {
		if atomic.CompareAndSwapInt64(&reentrance, 0, 1) {
			log.Info("trojan-go start.")
			defer atomic.StoreInt64(&reentrance, 0)
		} else {
			log.Info("此Goroutine不可重入")
			return
		}

		common.ConfigRootDir = string(decodeBytes)
		proxy.SetProxyConfigFile(common.GetProgramDir() + "/config.json")
		// jni 调用golib只会执行一次初始化, 所以再次启动需要手动初始化
		if len(common.Handlers) == 0 {
			easy.Register()
			proxy.Register()
		}
		// 不能使用defer释放锁，因为下面代码会阻塞，造成死锁

		for {
			h, err := common.PopOptionHandler()
			if err != nil {
				// 为了android平台加入了手动关闭的机制, 防止golib导致app闪退
				log.Warn("invalid options", err)
				break
			}
			err = h.Handle()
			if err == nil {
				break
			}
		}
	}()

}

type domainInfo struct {
	Domain string
	Email  string
}

// RequestCertWithJson 申请证书
//export RequestCertWithJson
func RequestCertWithJson(b64 *C.char) {
	log.Info("Guide mode: request cert")

	log.Warn("To perform a ACME challenge, trojan-go need the ROOT PRIVILEGE to bind port 80 and 443")
	log.Warn("Please make sure you HAVE sudo this program, and port 80/443 is NOT used by other process at this moment")
	log.Info("Continue? (y/n)")

	var b64String = C.GoString(b64)
	decodeBytes, err := base64.StdEncoding.DecodeString(b64String)
	if err != nil {
		log.Info(b64String)
		log.Error(err)
		return
	}

	info := &domainInfo{}

	if err := json.Unmarshal(decodeBytes, info); err != nil {
		log.Error(common.NewError("failed to parse domain_info.json").Base(err))
		return
	}

	fmt.Printf("Domain: %s, Email: %s\n", info.Domain, info.Email)

	data, err := json.Marshal(info)
	common.Must(err)
	ioutil.WriteFile("domain_info.json", data, os.ModePerm)

	if err := cert.RequestCert(info.Domain, info.Email); err != nil {
		log.Error(common.NewError("Failed to create cert").Base(err))
		return
	}

	log.Info("All done. Certificates have been saved to server.crt and server.key")
	log.Warn("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")
	log.Warn("BACKUP DOMAIN_INFO.JSON, SERVER.KEY, SERVER.CRT AND USER.KEY TO A SAFE PLACE")
	log.Warn("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")
}

// Stop 停止客户端代理服务
//export Stop
func Stop() {
	proxy.CloseChan()
}

var platString *C.char

// GetPlatformInfo 获取系统平台
//export GetPlatformInfo
func GetPlatformInfo() *C.char {
	if platString != nil {
		C.free(unsafe.Pointer(platString))
	}

	platString = C.CString(runtime.GOOS + " " + runtime.GOARCH)
	return platString
}
