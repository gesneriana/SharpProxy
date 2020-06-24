using Newtonsoft.Json;
using SharpProxy.Browser.Example.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpProxy.Browser.Example
{
    public partial class CrawlerRuleForm : Form
    {
        public CrawlerRuleForm()
        {
            InitializeComponent();
        }

        private void CrawlerRuleForm_Load(object sender, EventArgs e)
        {
            cbxChapterTitleType.SelectedIndex = 0;
            var bookConfigs = BooksUtils.GetAllBookConfig();
            cbxBookName.DataSource = bookConfigs;
            cbxBookName.DisplayMember = nameof(Book.BookName);
            cbxBookName.ValueMember = nameof(Book.Id);

            var rules = CrawlerRuleUtils.GetAllCrawlerRules();
            cbxSelectedRule.DataSource = rules;
            cbxSelectedRule.DisplayMember = nameof(CrawlerRule.RuleName);

            if (bookConfigs.Count > 0)
            {
                var book = bookConfigs.FirstOrDefault();
                cbxBookName.Text = book.BookName;
                txtAuthor.Text = book.Author;
                cbxSelectedRule.Text = book.CrawlerRuleName;
                txtStartUrl.Text = book.StartUrl;
            }

            cbxRuleName.DataSource = JsonConvert.DeserializeObject<List<CrawlerRule>>(JsonConvert.SerializeObject(rules));
            cbxRuleName.DisplayMember = nameof(CrawlerRule.RuleName);
        }

        /// <summary>
        /// 添加一本书的配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveBookRules_Click(object sender, EventArgs e)
        {
            var bookName = cbxBookName.Text;
            var author = txtAuthor.Text;
            var selectedRules = cbxSelectedRule.Text;
            var startUrl = txtStartUrl.Text;

            if (string.IsNullOrWhiteSpace(bookName) || string.IsNullOrWhiteSpace(author)
                    || string.IsNullOrWhiteSpace(selectedRules) || string.IsNullOrWhiteSpace(startUrl))
            {
                Console.WriteLine("爬虫配置填写不完整,请输入完整的配置");
                MessageBox.Show("爬虫配置填写不完整,请输入完整的配置");
                return;
            }

            BooksUtils.AddBook(new Book
            {
                BookName = bookName,
                Author = author,
                CrawlerRuleName = selectedRules,
                StartUrl = startUrl
            });
            Console.WriteLine("爬虫配置保存成功");
        }

        /// <summary>
        /// 保存爬虫规则
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveRules_Click(object sender, EventArgs e)
        {
            var chapterRootPath = txtChapterRootPath.Text;
            var chapterTitleType = cbxChapterTitleType.Text;
            var chapterTitlePath = txtChapterTitlePath.Text;
            var nextPagePath = txtNextPagePath.Text;
            var ruleName = cbxRuleName.Text;
            var awaitTimeText = txtAwaitTime.Text;
            var awaitTime = 1;

            if (string.IsNullOrWhiteSpace(chapterRootPath) || string.IsNullOrWhiteSpace(chapterTitlePath)
                || string.IsNullOrWhiteSpace(nextPagePath) || string.IsNullOrWhiteSpace(ruleName)
                || string.IsNullOrWhiteSpace(awaitTimeText))
            {
                Console.WriteLine("爬虫规则填写不完整,请输入完整的规则");
                MessageBox.Show("爬虫规则填写不完整,请输入完整的规则");
                return;
            }

            if (int.TryParse(awaitTimeText, out var t))
            {
                if (t >= 0)
                {
                    awaitTime = t;
                }
            }

            CrawlerRuleUtils.SaveCrawlerRule(new CrawlerRule
            {
                RuleName = ruleName,
                ChapterRootPath = chapterRootPath,
                ChapterTitlePath = chapterTitlePath,
                ChapterTitleType = chapterTitleType,
                NextPagePath = nextPagePath,
                AwaitTime = awaitTime
            });

            var rules = CrawlerRuleUtils.GetAllCrawlerRules();

            var selectedRuleText = cbxSelectedRule.Text;

            cbxSelectedRule.DataSource = rules;
            cbxSelectedRule.DisplayMember = nameof(CrawlerRule.RuleName);
            if (!string.IsNullOrWhiteSpace(selectedRuleText))
            {
                cbxSelectedRule.Text = selectedRuleText;
            }
        }

        /// <summary>
        /// 切换规则时同步更改其他输入框和下拉框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRuleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRuleName.DataSource is List<CrawlerRule> rules)
            {
                if (string.IsNullOrEmpty(cbxRuleName.Text))
                {
                    return;
                }
                var rule = rules.FirstOrDefault(x => x.RuleName.Equals(cbxRuleName.Text));
                if (rule == null)
                {
                    return;
                }
                txtChapterRootPath.Text = rule.ChapterRootPath;
                cbxChapterTitleType.Text = rule.ChapterTitleType;
                txtChapterTitlePath.Text = rule.ChapterTitlePath;
                txtNextPagePath.Text = rule.NextPagePath;
                txtAwaitTime.Text = rule.AwaitTime.ToString();
            }
        }

        private void cbxBookName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxBookName.DataSource is List<Book> books)
            {
                var id = cbxBookName.SelectedValue as int? ?? 0;
                if (id == 0)
                {
                    // 初始化默认选中一个值, 虽然触发了世间, 但是不是用户手动触发的,  所以获取不到
                    return;
                }

                var book = books.FirstOrDefault(x => x.Id == id);
                if (book == null)
                {
                    Console.WriteLine("书名和Id不匹配");
                    return;
                }

                txtAuthor.Text = book.Author;
                cbxSelectedRule.Text = book.CrawlerRuleName;
                txtStartUrl.Text = book.StartUrl;
            }
        }

        private void btnImportRules_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"请把规则文件复制到此目录");
            var exportDir = Environment.CurrentDirectory + "\\CrawlerRules";
            System.Diagnostics.Process.Start("explorer.exe", exportDir);
        }
    }
}
