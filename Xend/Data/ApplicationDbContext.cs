using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Xend.Models;

namespace Xend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Define DbSet properties for your entities
        public DbSet<Client> Clients { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        // Override OnModelCreating method if needed to configure entity relationships, constraints, etc.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity relationships, constraints, etc.
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Client)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Client>()
                .HasData(
                new Client { ClientId = "1", Name = "John Doe", Email = "john.doe@example.com" },
                new Client { ClientId = "2", Name = "Jane Smith", Email = "jane.smith@example.com" },
                new Client { ClientId = "3", Name = "Mike Johnson", Email = "mike.johnson@example.com" },
                new Client { ClientId = "4", Name = "Sarah Williams", Email = "sarah.williams@example.com" },
                new Client { ClientId = "5", Name = "Robert Brown", Email = "robert.brown@example.com" },
                new Client { ClientId = "6", Name = "Emily Davis", Email = "emily.davis@example.com" },
                new Client { ClientId = "7", Name = "David Wilson", Email = "david.wilson@example.com" },
                new Client { ClientId = "8", Name = "Jennifer Taylor", Email = "jennifer.taylor@example.com" },
                new Client { ClientId = "9", Name = "Michael Anderson", Email = "michael.anderson@example.com" },
                new Client { ClientId = "10", Name = "Jessica Clark", Email = "jessica.clark@example.com" });

        }
    }
}

