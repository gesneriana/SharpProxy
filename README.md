# SharpProxy

.net P/Invoke calls Trojan go proxy packaged dynamic link library

此项目的上游项目来自 [trojan-go](https://github.com/p4gefau1t/trojan-go)

完整介绍请参考上游项目的文档, 我只是用.Net做了简单的封装, 只封装了几个必须的方法, 申请证书, 启动, 停止

## 文件夹说明

trojan-go上游仓库代码文件夹在 [trojan_go_lib](https://github.com/gesneriana/SharpProxy/tree/master/src/trojan_go_lib) 

SharpProxy 文件夹是封装的.net standard库, 目录在 [SharpProxy](https://github.com/gesneriana/SharpProxy/tree/master/src/SharpProxy) 

虽然.net standard库支持很多平台, 但是我只编译了windows x86,x64以及linux x64的动态链接库,只有在这几个平台是直接能用的, 其他平台需要自己自行编译

推荐使用liteide, 也可以使用命令行编译, 需要自己安装gcc和gosdk
例如: go build -buildmode=c-shared -o trojan_go_64.dll main.go

## 移植性

golang基本上可以在绝大多数平台上运行, 包括android和ios
如果需要移植到移动端需要使用gomobile技术, android上是打包为 aar包,ios是framework包
.net 也能运行在大多数平台上, 但是比golang少, 目前在mips上基本上无法正常使用, 不如直接用纯go代码开发

## 联系方式

如果你遇到了什么问题, 欢迎加入电报群骚扰
[我的Telegram小号](https://t.me/suki6)
[电报群](https://t.me/joinchat/ND5TURNP-1cTOLDmzB55Vg)

## 致谢

[trojan-go](https://github.com/p4gefau1t/trojan-go)