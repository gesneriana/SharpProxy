using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpProxy.Browser.Example.Model
{
    public class CrawlerRuleUtils
    {
        public static void SaveCrawlerRule(CrawlerRule rule)
        {
            if (!Directory.Exists("CrawlerRules"))
            {
                Directory.CreateDirectory("CrawlerRules");
            }

            File.WriteAllText($"./CrawlerRules/{rule.RuleName}.json", JsonConvert.SerializeObject(rule));
        }

        public static List<CrawlerRule> GetAllCrawlerRules()
        {
            if (!Directory.Exists("CrawlerRules"))
            {
                return new List<CrawlerRule>();
            }

            var list = new List<CrawlerRule>();
            var filePath = Directory.GetFiles("CrawlerRules") ?? new string[0];
            foreach (var rulePath in filePath)
            {
                if (!rulePath.ToLower().EndsWith(".json"))
                {
                    Console.WriteLine($"扫描到了奇怪的文件,{rulePath}");
                    continue;
                }

                var json = File.ReadAllText(rulePath);
                var rule = JsonConvert.DeserializeObject<CrawlerRule>(json) ?? new CrawlerRule();
                if (!string.IsNullOrWhiteSpace(rule.RuleName))
                {
                    list.Add(rule);
                }
            }

            return list;
        }
    }
}
