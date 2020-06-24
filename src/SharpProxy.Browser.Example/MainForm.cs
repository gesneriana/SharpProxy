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
            this.panel1.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.LifeSpanHandler = new OpenPageSelf(); // 不用新窗口打开url
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            this.panel1.Left = 10;
            this.panel1.Width = this.Width - 40;
            this.panel1.Top = 40;
            this.panel1.Height = this.Height - 90;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            var html = chromeBrowser.GetMainFrame().GetSourceAsync().GetAwaiter().GetResult();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            // 不同网站的HTML文档结构不同, 自行修改 GetElementbyId 的参数
            var chapterNode = doc.GetElementbyId("j_chapterBox");
            if (chapterNode == null)
            {
                Console.WriteLine("请确认打开了第一章的url");
                return; // 完结
            }

            var chapterTitleText = chapterNode.Descendants().Where(x => x.HasClass("j_chapterName")).FirstOrDefault()?.InnerText ?? string.Empty;
            var chapterHtml = chapterNode?.OuterHtml ?? string.Empty;
            var chapterText = chapterNode?.InnerText ?? string.Empty;

            // 写入sqlite数据库中
            var books = BooksUtils.AddBooks(new List<Book>() { new Book() { BookName = "道君", Author = "跃千愁" } });
            var book = books.FirstOrDefault(x => x.BookName.Equals("道君") && x.Author.Equals("跃千愁"));
            if (book.Id <= 0)
            {
                return;
            }
            this.panel1.Tag = book;
            ChaptersUtils.AddChapters(new List<Chapter> { new Chapter() { ChapterTitle = chapterTitleText, BookId = book.Id, Text = chapterText, Html = chapterHtml } });

            chromeBrowser.FrameLoadEnd += ChromeBrowser_FrameLoadEnd;

            // 翻页, j_chapterNext
            chromeBrowser.GetMainFrame().ExecuteJavaScriptAsync("document.getElementById('j_chapterNext').click();");
        }

        private void ChromeBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                chromeBrowser.GetSourceAsync().ContinueWith(taskHtml =>
                {
                    var html = taskHtml.Result;
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var chapterNode = doc.GetElementbyId("j_chapterBox");
                    if (chapterNode == null)
                    {
                        return; // 完结
                    }

                    var chapterTitleText = chapterNode.Descendants().Where(x => x.HasClass("j_chapterName")).FirstOrDefault()?.InnerText ?? string.Empty;
                    var chapterHtml = chapterNode?.OuterHtml ?? string.Empty;
                    var chapterText = chapterNode?.InnerText ?? string.Empty;

                    // 写入sqlite数据库中
                    var book = this.panel1.Tag as Book ?? new Book();
                    if (book.Id <= 0)
                    {
                        return;
                    }
                    ChaptersUtils.AddChapters(new List<Chapter> { new Chapter() { ChapterTitle = chapterTitleText, BookId = book.Id, Text = chapterText, Html = chapterHtml } });

                    // 翻页, j_chapterNext
                    chromeBrowser.GetMainFrame().ExecuteJavaScriptAsync("document.getElementById('j_chapterNext').click();");
                });
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            BooksUtils.ExportBooks(new List<Book> { new Book { BookName = "道君", Author = "跃千愁" } });
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                chromeBrowser.Reload();
            }
            else
            {
                chromeBrowser.Load(txtUrl.Text);
            }
        }
    }
}
