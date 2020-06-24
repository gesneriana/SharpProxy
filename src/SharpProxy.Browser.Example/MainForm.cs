using CefSharp;
using CefSharp.WinForms;
using SharpProxy.Browser.Example.CefSetting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using SharpProxy.Browser.Example.Model;
using SharpProxy.Browser.Example.Utils;
using System.Threading;

namespace SharpProxy.Browser.Example
{
    public partial class MainForm : Form
    {
        public ChromiumWebBrowser chromeBrowser;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitializeChromium();
        }

        //初始化浏览器并启动
        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            // 启用缓存和cookie
            settings.CachePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\Cache";
            settings.PersistSessionCookies = true;

            // trojan go 客户端代理端口可以在配置文件中修改
            settings.CefCommandLineArgs.Add("proxy-server", "socks5://127.0.0.1:1082");

            // dir是Trojan go客户端配置文件的目录
            SharpProxyTrojanGo.Start(Environment.CurrentDirectory + "/libs");

            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser("https://www.google.com/");
            // Add it to the form and fill it to the form window.
            this.panelBrowser.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.LifeSpanHandler = new OpenPageSelf(); // 不用新窗口打开url
            chromeBrowser.KeyboardHandler = new CefKeyBoardHander();
            this.Width = 1024;
            this.Height = 800;

            var bookConfigs = BooksUtils.GetAllBookConfig();
            bookConfigs.ForEach(x =>
            {
                x.BookName = $"{x.BookName}-{x.Author}";
            });
            cbxBookConfig.DataSource = bookConfigs;
            cbxBookConfig.DisplayMember = nameof(Book.BookName);
            cbxBookConfig.ValueMember = nameof(Book.Id);
            cbxBookConfig.SelectedIndex = 0;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.panelBrowser.Left = 10;
            this.panelBrowser.Width = this.Width - 40;
            this.panelBrowser.Top = 40;
            this.panelBrowser.Height = this.Height - 90;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cbxBookConfig.DataSource is List<Book> books)
            {
                var id = cbxBookConfig.SelectedValue as int? ?? 0;
                if (id == 0)
                {
                    Console.WriteLine("请选择有效的书名");
                    return;
                }
                var book = books.FirstOrDefault(x => x.Id == id);
                this.panelBrowser.Tag = book;

                // 读取最新的爬虫规则
                var rules = CrawlerRuleUtils.GetAllCrawlerRules();
                cbxBookConfig.Tag = rules;

                chromeBrowser.FrameLoadEnd += ChromeBrowser_FrameLoadEnd;
                chromeBrowser.Load(book.StartUrl);
            }
        }

        private void ChromeBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            // 浏览器刚打开, 还没有开始抓取网页
            var book = this.panelBrowser.Tag as Book ?? new Book();
            if (book.Id <= 0)
            {
                return;
            }

            var rules = this.cbxBookConfig.Tag as List<CrawlerRule> ?? new List<CrawlerRule>();
            var rule = rules.FirstOrDefault(x => x.RuleName.Equals(book.CrawlerRuleName));
            if (rule == null)
            {
                Console.WriteLine($"{book.BookName} 没有找到匹配的爬虫规则 {book.CrawlerRuleName}");
                return;
            }

            if (e.Frame.IsMain)
            {
                chromeBrowser.GetSourceAsync().ContinueWith(taskHtml =>
                {
                    Thread.Sleep(rule.AwaitTime * 1000);

                    var html = taskHtml.Result;
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    //*[@id="tsf"]/div[2]/div[1]/div[1]/div/div[2]/input
                    HtmlNode chapterRootNode = null;
                    if (rule.ChapterRootPath.Contains("/"))
                    {
                        chapterRootNode = doc.DocumentNode.SelectSingleNode(rule.ChapterRootPath);
                    }
                    else
                    {
                        chapterRootNode = doc.GetElementbyId(rule.ChapterRootPath);
                    }

                    if (chapterRootNode == null)
                    {
                        Console.WriteLine("请确认打开了第一章的url");
                        chromeBrowser.FrameLoadEnd -= ChromeBrowser_FrameLoadEnd;
                        return; // 爬取到了最后一章,完结
                    }

                    var chapterTitleText = string.Empty;
                    if (rule.ChapterTitleType.ToLower().Equals("xpath"))
                    {
                        chapterTitleText = doc.DocumentNode.SelectSingleNode(rule.ChapterTitlePath)?.InnerText ?? string.Empty;
                    }
                    else if (rule.ChapterTitleType.ToLower().Equals("id"))
                    {
                        chapterTitleText = doc.GetElementbyId(rule.ChapterTitlePath)?.InnerText ?? string.Empty;
                    }
                    else if (rule.ChapterTitleType.ToLower().Equals("class"))
                    {
                        chapterTitleText = chapterRootNode.Descendants().Where(x => x.HasClass(rule.ChapterTitlePath)).FirstOrDefault()?.InnerText ?? string.Empty;
                    }

                    if (string.IsNullOrWhiteSpace(chapterTitleText))
                    {
                        Console.WriteLine($"爬虫获取章节标题的规则匹配失败, 请检查 {rule.ChapterTitleType} {rule.ChapterTitlePath}");
                        chromeBrowser.FrameLoadEnd -= ChromeBrowser_FrameLoadEnd;
                        return; // 爬取到了最后一章,完结
                    }

                    var chapterHtml = chapterRootNode.OuterHtml ?? string.Empty;
                    var chapterText = chapterRootNode.InnerText ?? string.Empty;

                    // 写入sqlite数据库中
                    ChaptersUtils.AddChapters(new List<Chapter>
                    {
                        new Chapter() { ChapterTitle = chapterTitleText, BookId = book.Id, Text = chapterText, Html = chapterHtml } },
                        book.Id
                    );

                    // 翻页, j_chapterNext
                    chromeBrowser.GetMainFrame().ExecuteJavaScriptAsync($"{rule.NextPagePath}.click();");
                });
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // 浏览器刚打开, 还没有开始抓取网页
            var id = this.cbxBookConfig.SelectedValue as int? ?? 0;
            if (id <= 0)
            {
                Console.WriteLine("请选择导出的书名");
                return;
            }

            if(cbxBookConfig.DataSource is List<Book> books)
            {
                var book = books.FirstOrDefault(x => x.Id == id);
                Console.WriteLine("正在导出,请稍后");
                Task.Factory.StartNew(() =>
                {
                    BooksUtils.ExportBooks(new List<Book> { book });
                    var exportDir = Environment.CurrentDirectory + "\\ExportBooks";
                    Console.WriteLine($"导出完成,{exportDir}");
                    System.Diagnostics.Process.Start("explorer.exe", exportDir);
                });
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                Console.WriteLine("请输入url到地址栏");
            }
            else
            {
                chromeBrowser.Load(txtUrl.Text);
            }
        }

        private void btnEditRule_Click(object sender, EventArgs e)
        {
            var crawlerRuleForm = new CrawlerRuleForm();
            crawlerRuleForm.ShowDialog();

            var bookConfigs = BooksUtils.GetAllBookConfig();
            bookConfigs.ForEach(x =>
            {
                x.BookName = $"{x.BookName}-{x.Author}";
            });
            cbxBookConfig.DataSource = bookConfigs;
            cbxBookConfig.DisplayMember = nameof(Book.BookName);
            cbxBookConfig.ValueMember = nameof(Book.Id);
            cbxBookConfig.SelectedIndex = 0;

            this.cbxBookConfig.Tag = CrawlerRuleUtils.GetAllCrawlerRules();
        }
    }
}
