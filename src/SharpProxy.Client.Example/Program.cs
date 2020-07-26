using System;
using System.IO;
using System.Threading;

namespace SharpProxy.Client.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // linux下可能需要手动复制SharpProxy项目中的libs文件夹, 不然会报错找不到 config.json 等配置文件
            // 如果报错注意查看Console输出的错误信息, trojan go lib有详细的日志输出
            // 即使以GUI程序启动, 调试模式也会在visual studio中输出控制台日志
            Console.WriteLine(SharpProxyTrojanGo.GetPlatformInfo());

            var dir = Environment.CurrentDirectory + "/libs";
            SharpProxyTrojanGo.Start(dir);


            Console.WriteLine("输入Q终止程序");
            while (true)
            {
                var s = Console.ReadLine(); // 如果不阻塞进程,main程序退出之后golang的后台线程会被关闭
                if (s != null && s.ToLower() == "q")
                {
                    break;
                }
                else if (s == null)
                {
                    Console.WriteLine("标准输入流为null, 可能是被其他进程启动, 作为子进程运行.");
                    Thread.Sleep(1000 * 60 * 60);
                    continue;
                }
            }

            SharpProxyTrojanGo.Stop();
            Thread.Sleep(1000);
        }
    }
}
