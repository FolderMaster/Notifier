using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Data
{
    public class DataBaseContext : DbContext
    {
        public readonly string _connectionString;

        public DbSet<UserData> UserData { get; set; }

        public DataBaseContext(string connectionString)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(connectionString, nameof(connectionString));
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite(_connectionString).EnableDetailedErrors();
        }
    }
}
