using ACE_PC.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace ACE_PC.Database.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Roles Seed Data
            modelBuilder.Entity<Role>().HasData(
                        new Role { RoleId = 1, Name = "user" },
                        new Role { RoleId = 2, Name = "admin" }
                    );

            modelBuilder.Entity<Category>().HasData(
                    new Category { CategoryId = 1 , Name = "Test Category One"},
                    new Category { CategoryId = 2 , Name = "Test Category Two"},
                    new Category { CategoryId = 3 , Name = "Test Category Three"}
                );

            //
            modelBuilder.Entity<Like>()
                .HasKey(l => new { l.UserId, l.QuoteId });

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict); // keep cascade here;

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Quote)
                .WithMany(q => q.Likes)
                .HasForeignKey(l => l.QuoteId)
                .OnDelete(DeleteBehavior.Cascade); // keep cascade here;

            //
            modelBuilder.Entity<Comment>()
              .HasOne(c => c.Quote)
              .WithMany(q => q.Comments)
              .HasForeignKey(c => c.QuoteId)
              .OnDelete(DeleteBehavior.Cascade); // keep cascade here

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(q=>q.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // prevent multiple cascade paths
        }

    }
}

