using ContactBook.Core.ViewModel.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace ContactBook.Core.Database
{
    public class DatabaseConfig : DbContext
    {
       public DbSet<PearsonViewModel> Pearson { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Skonfiguruj połączenie z bazą danych SQLite
            optionsBuilder.UseSqlite("Data Source=DemoDB.db");
        }
    }
}
