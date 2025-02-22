using Microsoft.EntityFrameworkCore;
using EFCoreWebApi.Models;

namespace EFCoreWebApi.Data
{
    public class AppDbContext : DbContext
    {
        // Add this constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        // Remove the OnConfiguring method
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlite("Data Source=app.db");
        // }
    }
}