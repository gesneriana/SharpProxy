using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpProxy.Browser.Example.Model
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 书名
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public string BookName { get; set; } = string.Empty;

        /// <summary>
        /// 作者
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// 爬虫使用的规则
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public string CrawlerRuleName { get; set; } = string.Empty;

        /// <summary>
        /// 第一章的Url, 也可以是中间某个章节的Url
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public string StartUrl { get; set; } = string.Empty;
    }
}
