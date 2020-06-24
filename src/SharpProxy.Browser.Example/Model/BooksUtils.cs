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
        public static List<Book> AddBooks(List<Book> books)
        {
            var names = books.Select(x => x.BookName).ToList();
            using (var db = new SqliteDbContext())
            {
                db.Database.EnsureCreated();
                var oldBooks = db.Books.Where(x => names.Contains(x.BookName)).ToList();
                var id = db.Books.OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0;
                foreach (var item in books)
                {
                    var oldBook = oldBooks.FirstOrDefault(x => x.BookName.Equals(item.BookName)) ?? new Book();
                    if (item.BookName.Equals(oldBook.BookName) && item.Author.Equals(oldBook.Author))
                    {
                        item.Id = oldBook.Id;
                        continue;   // 同名书籍,跳过
                    }
                    item.Id = ++id;
                    db.Books.Add(item);
                }
                db.SaveChanges();
            }
            return books;
        }

        public static void ExportBooks(List<Book> books)
        {
            var names = books.Select(x => x.BookName).ToList();
            using (var db = new SqliteDbContext())
            {
                db.Database.EnsureCreated();
                var oldBooks = db.Books.Where(x => names.Contains(x.BookName)).ToList();

                var sbTxt = new StringBuilder();
                var sbHtml = new StringBuilder();
                foreach (var item in oldBooks)
                {
                    var chaps = db.Chapters.Where(x => x.BookId == item.Id).ToList();

                    var bookTxtPath = $"{item.BookName}-{item.Author}.txt";
                    var bookHtmlPath = $"{item.BookName}-{item.Author}.html";

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

    }
}
