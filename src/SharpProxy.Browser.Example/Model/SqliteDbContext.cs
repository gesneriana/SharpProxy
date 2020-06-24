using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpProxy.Browser.Example.Model
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=books.db;");
        }
    }
}
