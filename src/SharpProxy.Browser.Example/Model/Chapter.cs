
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpProxy.Browser.Example.Model
{
    /// <summary>
    /// 章节数据
    /// </summary>
    [Table("Chapter")]
    public class Chapter
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 书的Id
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// 章节标题
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public string ChapterTitle { get; set; } = string.Empty;

        /// <summary>
        /// 章节文本
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// 章节html
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public string Html { get; set; } = string.Empty;
    }
}
