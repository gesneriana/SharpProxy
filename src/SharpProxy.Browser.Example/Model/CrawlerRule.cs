using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpProxy.Browser.Example.Model
{
    /// <summary>
    /// 爬虫规则
    /// </summary>
    public class CrawlerRule
    {
        /// <summary>
        /// 规则名称, 可以用网站的域名
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// 章节的 xpath 或者 Id
        /// </summary>
        public string ChapterRootPath { get; set; }

        /// <summary>
        /// 章节标题的 选择器 类型  xpath , Id 或者 class
        /// </summary>
        public string ChapterTitleType { get; set; }

        /// <summary>
        /// 章节标题的 选择器
        /// </summary>
        public string ChapterTitlePath { get; set; }

        /// <summary>
        /// 翻页按钮的 JS path
        /// </summary>
        public string NextPagePath { get; set; }

        /// <summary>
        /// 采集延时
        /// </summary>
        public int AwaitTime { get; set; }
    }
}
