using System;
using System.Collections.Generic;
using System.Text;


    using Microsoft.EntityFrameworkCore;
    using RaceDay.Models;
   

    namespace RaceDay.Data
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Event> Events { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Enrollment> Enrollments { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Prevent cascade delete issues on Enrollment
                modelBuilder.Entity<Enrollment>()
                    .HasOne(e => e.Event)
                    .WithMany()
                    .HasForeignKey(e => e.EventId)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<Enrollment>()
                    .HasOne(e => e.Participant)
                    .WithMany(u => u.Enrollments)
                    .HasForeignKey(e => e.ParticipantId)
                    .OnDelete(DeleteBehavior.NoAction);

                base.OnModelCreating(modelBuilder);
            }
        }
    }