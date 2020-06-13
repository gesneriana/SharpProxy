using SharpProxy.linux.x64;
using SharpProxy.windows.x64;
using SharpProxy.windows.x86;
using System;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;

namespace SharpProxy
{
    /// <summary>
    /// Trojan Go实现的代理, PInvoke调用
    /// </summary>
    public class SharpProxyTrojanGo
    {
        /// <summary>
        /// 启动Trojan go
        /// config.json将会决定trojan go的工作模式, 比如以客户端, 或者服务器端启动
        /// </summary>
        /// <param name="dir">config.json配置文件的目录</param>
        public static void Start(string dir)
        {
            // 中文目录会导致乱码,因为封送字符串是用ANSI编码, 所以需要先base64编码, 
            // *C.char 类型对UTF-8和Unicode支持不太好, GoString更是会导致程序崩溃(我已经在ubuntu和windows中测试过)
            var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(dir));
            Console.WriteLine(b64);
            var ptr = Marshal.StringToHGlobalAnsi(b64);
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (Environment.Is64BitProcess)
                {
                    NativeDllWindows64.Start(ptr);
                }
                else
                {
                    NativeDllWindows32.Start(ptr);
                }
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                if (Environment.Is64BitProcess)
                {
                    NativeDllLinux64.Start(ptr);
                }
            }

            Marshal.FreeHGlobal(ptr);
        }

        /// <summary>
        /// 申请证书
        /// </summary>
        /// <param name="Domain"></param>
        /// <param name="Email"></param>
        public static void RequestCertWithJson(string Domain, string Email)
        {
            // 中文字符会导致乱码,因为封送字符串是用ANSI编码, 所以需要先base64编码, 
            // *C.char 类型对UTF-8和Unicode支持不太好, GoString更是会导致程序崩溃(我已经在ubuntu和windows中测试过)
            var config = JsonConvert.SerializeObject(new { Domain, Email });
            var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(config));
            Console.WriteLine(b64);
            var ptr = Marshal.StringToHGlobalAnsi(b64);
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (Environment.Is64BitProcess)
                {
                    NativeDllWindows64.RequestCertWithJson(ptr);
                }
                else
                {
                    NativeDllWindows32.RequestCertWithJson(ptr);
                }
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                if (Environment.Is64BitProcess)
                {
                    NativeDllLinux64.RequestCertWithJson(ptr);
                }
            }

            Marshal.FreeHGlobal(ptr);
        }

        /// <summary>
        /// 停止Trojan go
        /// </summary>
        public static void Stop()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (Environment.Is64BitProcess)
                {
                    NativeDllWindows64.Stop();
                }
                else
                {
                    NativeDllWindows32.Stop();
                }
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                if (Environment.Is64BitProcess)
                {
                    NativeDllLinux64.Stop();
                }
            }
        }

        /// <summary>
        /// 获取当前平台信息, 操作系统和处理器架构, 例如 windows, amd64
        /// </summary>
        /// <returns></returns>
        public static string GetPlatformInfo()
        {
            // 不要尝试手动释放CGo的 *C.char指针, 因为golang有自己的内存管理机制, golang采用动态栈, 由golang底层自己释放内存比较安全
            var ptr = IntPtr.Zero;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (Environment.Is64BitProcess)
                {
                    ptr = NativeDllWindows64.GetPlatformInfo();
                }
                else
                {
                    ptr = NativeDllWindows32.GetPlatformInfo();
                }
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                if (Environment.Is64BitProcess)
                {
                    ptr = NativeDllLinux64.GetPlatformInfo();
                }
            }
            // 这里没有中文字符, 所以不需要base64编码处理, 如果有乱码需要在 golang 导出函数中进行base64编码
            var platInfoString = Marshal.PtrToStringAnsi(ptr) ?? "";
            return platInfoString;
        }
    }

}
