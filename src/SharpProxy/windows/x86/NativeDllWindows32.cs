using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpProxy.windows.x86
{
    class NativeDllWindows32
    {
        /// <summary>
        /// 启动Trojan go客户端服务
        /// </summary>
        /// <param name="p0">配置文件的目录</param>
        [DllImport("libs/windows/x86/trojan_go_32.dll", EntryPoint = "Start", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Start(IntPtr p0);

        /// <summary>
        /// 申请证书
        /// </summary>
        /// <param name="p0">json配置</param>
        [DllImport("libs/windows/x64/trojan_go_32.dll", EntryPoint = "RequestCertWithJson")]
        public static extern void RequestCertWithJson(IntPtr p0);

        /// <summary>
        /// 停止Trojan go客户端服务
        /// </summary>
        [DllImport("libs/windows/x86/trojan_go_32.dll", EntryPoint = "Stop", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Stop();


        /// <summary>
        /// 获取当前运行平台的特定信息
        /// </summary>
        /// <returns></returns>
        [DllImport("libs/windows/x86/trojan_go_32.dll", EntryPoint = "GetPlatformInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetPlatformInfo();
    }
}
