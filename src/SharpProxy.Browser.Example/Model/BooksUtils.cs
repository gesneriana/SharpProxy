using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpProxy.Browser.Example.Model
{
    public class BooksUtils
    {
        public static Book AddBook(Book book)
        {
            using (var db = new SqliteDbContext())
            {
                db.Database.EnsureCreated();
                var oldBook = db.Books.Where(x => x.BookName.Equals(book.BookName) && x.Author.Equals(book.Author)).FirstOrDefault();

                if (oldBook != null)
                {
                    oldBook.CrawlerRuleName = book.CrawlerRuleName;
                    oldBook.StartUrl = book.StartUrl;
                }
                else
                {
                    var id = db.Books.OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0;
                    book.Id = ++id;
                    db.Books.Add(book);
                    oldBook = book;
                }

                db.SaveChanges();
                return oldBook;
            }
        }

        public static void ExportBooks(List<Book> books)
        {
            var ids = books.Select(x => x.Id).ToList();
            using (var db = new SqliteDbContext())
            {
                db.Database.EnsureCreated();
                var oldBooks = db.Books.Where(x => ids.Contains(x.Id)).ToList();

                var sbTxt = new StringBuilder();
                var sbHtml = new StringBuilder();
                foreach (var item in oldBooks)
                {
                    var chaps = db.Chapters.Where(x => x.BookId == item.Id).ToList() ?? new List<Chapter>();
                    if (chaps.Count == 0)
                    {
                        Console.WriteLine($"{item.BookName}-{item.Author} 没有获取到章节列表, 请先执行爬取任务");
                    }

                    var bookTxtPath = $"./ExportBooks/{item.BookName}-{item.Author}.txt";
                    var bookHtmlPath = $"./ExportBooks/{item.BookName}-{item.Author}.html";

                    if (!Directory.Exists("ExportBooks"))
                    {
                        Directory.CreateDirectory("ExportBooks");
                    }
                    var txtFile = File.Create(bookTxtPath);
                    txtFile.Close();
                    var htmlFile = File.Create(bookHtmlPath);
                    htmlFile.Close();

                    var txtFileStream = new FileStream(bookTxtPath, FileMode.Append);
                    var htmlFileStream = new FileStream(bookHtmlPath, FileMode.Append);
                    foreach (var chap in chaps)
                    {
                        var txtLines = chap.Text.Split('。');
                        var htmlLines = chap.Html.Split('。');
                        foreach (var txtLine in txtLines)
                        {
                            sbTxt.AppendLine($"{txtLine}。");
                        }
                        foreach (var htmlLine in htmlLines)
                        {
                            sbHtml.AppendLine($"{htmlLine}。");
                        }
                        sbTxt.AppendLine();
                        sbHtml.AppendLine();

                        var bytesTxt = Encoding.UTF8.GetBytes(sbTxt.ToString());
                        var bytesHtml = Encoding.UTF8.GetBytes(sbHtml.ToString());
                        txtFileStream.Write(bytesTxt, 0, bytesTxt.Length);
                        htmlFileStream.Write(bytesHtml, 0, bytesHtml.Length);
                        sbTxt.Clear();
                        sbHtml.Clear();
                    }
                    txtFileStream.Close();
                    htmlFileStream.Close();
                }
            }
        }

        public static List<Book> GetAllBookConfig()
        {
            using (var db = new SqliteDbContext())
            {
                db.Database.EnsureCreated();
                return db.Books.ToList() ?? new List<Book>();
            }
        }

    }
}
