using Microsoft.EntityFrameworkCore;

namespace Books.Models
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
            optionsBuilder.UseSqlServer(@"");

        public DbSet<Category> Categories { get; set; }

        public DbSet<Book> Books { get; set; }

    }
}
