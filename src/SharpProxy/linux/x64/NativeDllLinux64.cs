using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpProxy.linux.x64
{
    class NativeDllLinux64
    {
        /// <summary>
        /// 启动Trojan go客户端服务
        /// </summary>
        /// <param name="p0">配置文件的目录</param>
        [DllImport("libs/linux/x64/trojan_go_64.so", EntryPoint = "Start")]
        public static extern void Start(IntPtr p0);

        /// <summary>
        /// 申请证书
        /// </summary>
        /// <param name="p0">json配置</param>
        [DllImport("libs/linux/x64/trojan_go_64.so", EntryPoint = "RequestCertWithJson")]
        public static extern void RequestCertWithJson(IntPtr p0);

        /// <summary>
        /// 停止Trojan go客户端服务
        /// </summary>
        [DllImportAttribute("libs/linux/x64/trojan_go_64.so", EntryPoint = "Stop")]
        public static extern void Stop();


        /// <summary>
        /// 获取当前运行平台的特定信息
        /// </summary>
        /// <returns></returns>
        [DllImportAttribute("libs/linux/x64/trojan_go_64.so", EntryPoint = "GetPlatformInfo")]
        public static extern IntPtr GetPlatformInfo();
    }
}
