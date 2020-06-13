using System;
using System.IO;
using System.Threading;

namespace SharpProxy.Server.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // linux下可能需要手动复制SharpProxy项目中的libs文件夹, 不然会报错找不到 config.json 等配置文件
            // 如果报错注意查看Console输出的错误信息, trojan go lib有详细的日志输出
            // 即使以GUI程序启动, 调试模式也会在visual studio中输出控制台日志
            Console.WriteLine(SharpProxyTrojanGo.GetPlatformInfo());

            var serverConfigFile = Environment.CurrentDirectory + "/libs/server.json";
            if (!File.Exists(serverConfigFile))
            {
                Console.WriteLine($"{serverConfigFile} 不存在");
                return;
            }
            else
            {
                var serverConfigJson = File.ReadAllText(serverConfigFile);
                var runConfigFile = Environment.CurrentDirectory + "/libs/config.json";
                File.WriteAllText(runConfigFile, serverConfigJson);
            }

            var certFile = Environment.CurrentDirectory + "/server.crt";
            if (!File.Exists(certFile))
            {
                Console.WriteLine("请输入域名(不带https/http),例如: jp.kizuna.top");
                var domain = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(domain))
                {
                    domain = "jp.kizuna.top";
                }
                SharpProxyTrojanGo.RequestCertWithJson(domain, "981941940@qq.com");
            }

            var dir = Environment.CurrentDirectory + "/libs";
            SharpProxyTrojanGo.Start(dir);

            Thread.Sleep(1000);
            Console.WriteLine("输入Q终止程序");
            while (true)
            {
#if Debug
                var s = Console.ReadLine(); // 如果不阻塞进程,main程序退出之后golang的后台线程会被关闭
                if (s.ToLower() == "q")
                {
                    break;
                }
#else
                Thread.Sleep(1000);
#endif

            }

            SharpProxyTrojanGo.Stop();
            Thread.Sleep(1000);
        }
    }
}
