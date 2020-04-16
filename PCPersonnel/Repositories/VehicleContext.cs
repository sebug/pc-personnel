using System;
using Microsoft.EntityFrameworkCore;
using PCPersonnel.Entities;

namespace PCPersonnel.Repositories
{
    public class VehicleContext : DbContext
    {
        public DbSet<Vehicle> Vehicle { get; set; }

        public VehicleContext(DbContextOptions<VehicleContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            base.OnModelCreating(modelBuilder);
        }
    }
}
