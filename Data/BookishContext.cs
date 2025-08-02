using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using NewBookish.Models.Entities;

namespace NewBookish.Data
{
    public class BookishContext : DbContext
    {
        // Put all the tables you want in your database here
        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Librarian> Librarians { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This is the configuration used for connecting to the dataase
            Env.Load();
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DbConnectionString") ??
        throw new InvalidOperationException("Connection string not found."));
        }
    }
}