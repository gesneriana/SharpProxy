package proxy

import (
	"io/ioutil"
	"os"
	"sync"

	"github.com/p4gefau1t/trojan-go/common"
	"github.com/p4gefau1t/trojan-go/conf"
	"github.com/p4gefau1t/trojan-go/log"
)

// CloseChan 特殊的channel，它不能被写入任何数据，只有通过close()函数进行关闭操作，才能进行输出操作,不占用任何内存
var closeChan chan struct{}
var lock sync.Mutex
var _proxyOption *proxyOption

type proxyOption struct {
	args *string
	common.OptionHandler
}

func (*proxyOption) Name() string {
	return "proxy"
}

func (*proxyOption) Priority() int {
	return 0
}

func (c *proxyOption) Handle() error {
	log.Info("Trojan-Go", common.Version, "initializing")
	log.Info("loading config file from", *c.args)

	//exit code 23 stands for initializing error, and systemd will not trying to restart it
	data, err := ioutil.ReadFile(*c.args)
	if err != nil {
		log.Error(common.NewError("failed to read config file").Base(err))
		os.Exit(23)
	}
	config, err := conf.ParseJSON(data)
	if err != nil {
		log.Error(common.NewError("failed to parse config file").Base(err))
		os.Exit(23)
	}
	proxy, err := NewProxy(config)
	if err != nil {
		log.Error(common.NewError("failed to launch proxy").Base(err))
		os.Exit(23)
	}
	errChan := make(chan error)
	go func() {
		log.Info("trojan-go is started.")
		errChan <- proxy.Run()
	}()

	closeChan = make(chan struct{})
	select {
	case <-closeChan:
		proxy.Close()
		log.Info("closeChan close.")
		return nil
	case err := <-errChan:
		log.Error(err)
		return err
	}
}

func SetProxyConfigFile(config string) {
	_proxyOption.args = &config
}

func init() {
	Register()
}

func Register() {
	var c = common.GetProgramDir() + "/config.json"
	_proxyOption = &proxyOption{
		args: &c,
	}
	common.RegisterOptionHandler(_proxyOption)
}

func CloseChan() {
	lock.Lock()
	defer lock.Unlock()

	if closeChan != nil {
		close(closeChan)
		closeChan = nil
		log.Info("trojan-go Stop.")
	} else {
		log.Info("trojan-go is Stoped.")
	}
}
