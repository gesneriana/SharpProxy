using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpProxy.Browser.Example.Model
{
    public class ChaptersUtils
    {
        public static List<Chapter> AddChapters(List<Chapter> chapters)
        {
            var titles = chapters.Select(x => x.ChapterTitle).ToList();
            using (var db = new SqliteDbContext())
            {
                var oldChapters = db.Chapters.Where(x => titles.Contains(x.ChapterTitle)).ToList();
                var id = db.Chapters.OrderByDescending(x => x.Id).FirstOrDefault()?.Id ?? 0;
                foreach (var item in chapters)
                {
                    var oldBook = oldChapters.FirstOrDefault(x => x.ChapterTitle.Equals(item.ChapterTitle)) ?? new Chapter();
                    if (item.ChapterTitle.Equals(oldBook.ChapterTitle))
                    {
                        continue;   // 同名章节,跳过
                    }
                    item.Id = ++id;
                    db.Chapters.Add(item);
                }
                db.SaveChanges();
            }
            return chapters;
        }
    }
}
