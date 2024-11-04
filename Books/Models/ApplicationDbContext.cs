using Microsoft.EntityFrameworkCore;

namespace Books.Models
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => 
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Books;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;");

        public DbSet<Category> Categories { get; set; }

        public DbSet<Book> Books { get; set; }

    }
}
