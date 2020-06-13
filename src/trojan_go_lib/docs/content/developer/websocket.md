---
title: "Websocket"
draft: false
weight: 40
---

由于使用CDN中转时，HTTPS对CDN透明，CDN可以审查Websocket传输内容，而Trojan协议明文的头部特征过于明显，而TLS握手特征也很明显。为了保证Websocket传输的隐蔽和安全，默认情况下还会进行一次AES加密(混淆层)和TLS连接(双重TLS)。其中TLS用于保证传输的安全性，AES加密仅仅只是用于混淆流量。

**如果你使用的是国内的CDN，务必保证两者均开启。最坏情况下也应当保持混淆和双重TLS之一是打开的。**

开启Websocket模块的客户端可以使用```obfuscation```字段开启混淆，以及使用```double_tls```启用双重TLS以确保连接安全性和隐蔽性。

混淆层使用AES-CTR-128密码系统。加密主密钥派生自```obfuscation_password```，使用16字节的盐和sha1散列算法，使用pbkdf2对```obfuscation_password```密码进行32次迭代，派生得到16字节的主密钥。盐为硬编码的随机字节，定义为

```go
salt := []byte{48, 149, 6, 18, 13, 193, 247, 116, 197, 135, 236, 175, 190, 209, 146, 48}
```

在每次连接开始时，生成16字节的随机IV。根据主密钥和IV，使用AES-CTR-128密码系统对后续流量进行加密。Websocket承载的客户端到服务端的头部结构如下（IV随着Payload一起发送，避免了长度特征）：

```text
+----+-------------------+
| IV | Encrypted Payload |
+----+-------------------+
| 16 |     Variable      |
+----+-------------------+
```

服务端用同样的方法得到主密钥，接收IV后进行解密和后续的双向通讯。服务端到客户端的流量无头部，均为密文。

```text
+------------------------+
|    Encrypted Payload   |
+------------------------+
|        Variable        |
+------------------------+
```

注意，这层加密作用仅仅是增加数据流的熵，混淆流量特征，而不是保护数据安全。CTR加密模式不保证数据完整性和身份认证，因此可能遭受CDN或者中间人的重放攻击。如果CDN不可信，或者遭受了基于HTTPS劫持的中间人攻击，应启用双重TLS保证数据传输安全。

如果使用了双重TLS，握手造成的延迟可能略有增加，但是只要开启```session_reuse```，```session_ticket```复用TLS连接，以及开启```mux```启用TLS多路复用，只会在开启Trojan-Go时的最初几秒察觉明显的延迟。

当Websocket握手成功，但认证失败（混淆密码错误，用户密码sha224错误，TLS设置不一致），服务端将尝试与remote_addr:remote_port的服务器进行相同的Websocket握手过程，并将此websocket连接代理给它。如果它的Websocket握手失败，服务端将关闭该Websocket连接。

开启Websocket支持后，协议栈如下：

|协议              |备注       |
|-----------------|----------|
|真实流量|
|SimpleSocks      |如果使用mux|
|smux             |如果使用mux|
|Trojan|
|TLS              |如果开启双重TLS|
|混淆层            |如果开启混淆|
|Websocket|
|TLS|
|TCP|
