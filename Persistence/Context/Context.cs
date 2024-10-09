using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Persistence.Context
{
    [ExcludeFromCodeCoverage]
    public class Context : DbContext
    {
        private readonly IConfiguration _configuration;
        const string _connectionString = "DefaultConnection";

        public DbSet<MotocycleBike> MotocycleBikes { get; set; }
        public DbSet<Lease> Leases { get; set; }
        public DbSet<Deliver> Delivers { get; set; }

        public Context(IConfiguration configuration, DbContextOptions<Context> options) : base(options) { 
            _configuration = configuration;            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString(_connectionString), a=> a.EnableRetryOnFailure());
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityColumns();
            modelBuilder.Entity<MotocycleBike>().HasIndex(p => p.Plate).IsUnique();            
            modelBuilder.Entity<Deliver>().HasIndex(p=> p.Identifier).IsUnique();
            modelBuilder.Entity<Deliver>().HasIndex(p=> p.DriverLicenseNumber).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
