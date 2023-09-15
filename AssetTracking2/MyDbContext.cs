using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking2
{
    internal class MyDbContext : DbContext
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Asset;Integrated Security=True";
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Office> Offices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // We tell the app to use the connectionstring.
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {
            ModelBuilder.Entity<Office>().HasData(new Office { Id = 1, Name = "Sweden", Currency = "SEK" });
            ModelBuilder.Entity<Office>().HasData(new Office { Id = 2, Name = "Spain", Currency = "EUR" });
            ModelBuilder.Entity<Office>().HasData(new Office { Id = 3, Name = "Usa", Currency = "USD" });
        }
    }
}
