package common

import (
	"bufio"
	"crypto/sha256"
	"fmt"
	"io"
	"log"
	"os"
	"path/filepath"
	"runtime"
)

const (
	Version = "v0.4.7"
)

// ConfigRootDir android程序的资源目录
var ConfigRootDir = "/"

type Runnable interface {
	Run() error
	Close() error
}

func NewBufioReadWriter(rw io.ReadWriter) *bufio.ReadWriter {
	if bufrw, ok := rw.(*bufio.ReadWriter); ok {
		return bufrw
	}
	return bufio.NewReadWriter(bufio.NewReader(rw), bufio.NewWriter(rw))
}

func SHA224String(password string) string {
	hash := sha256.New224()
	hash.Write([]byte(password))
	val := hash.Sum(nil)
	str := ""
	for _, v := range val {
		str += fmt.Sprintf("%02x", v)
	}
	return str
}

func GetProgramDir() string {
	if runtime.GOOS == "android" {
		return ConfigRootDir // android 没办法正常获取路径，必须从app原生代码传入
	}
	if len(ConfigRootDir) > 0 {
		return ConfigRootDir // windows共享库 配置文件放在二级目录中
	}

	dir, err := filepath.Abs(filepath.Dir(os.Args[0]))
	if err != nil {
		log.Fatal(err)
	}
	return dir + "/libs"
}
