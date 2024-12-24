using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Kosov_backend.Models;

namespace Kosov_backend.Data
{
    public class Kosov_backendContext : DbContext
    {
        public Kosov_backendContext(DbContextOptions<Kosov_backendContext> options)
            : base(options)
        {
        }

        public DbSet<Athlete> Athelets { get; set; } = default!;
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; } = default!;
        public DbSet<Exercise> Exercises { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-Many: WorkoutPlan ⇔ Exercise
            modelBuilder.Entity<WorkoutPlan>()
                .HasMany(wp => wp.Exercises)
                .WithMany(e => e.WorkoutPlans)
                .UsingEntity<Dictionary<string, object>>(
                    "WorkoutPlanExercise",
                    join => join
                        .HasOne<Exercise>()
                        .WithMany()
                        .HasForeignKey("ExerciseId"),
                    join => join
                        .HasOne<WorkoutPlan>()
                        .WithMany()
                        .HasForeignKey("WorkoutPlanId")
                );

            // One-to-Many: Athelet → WorkoutPlan
            modelBuilder.Entity<WorkoutPlan>()
                .HasOne(wp => wp.Athelet)
                .WithMany(u => u.WorkoutPlans)
                .HasForeignKey(wp => wp.AtheletId);
        }

        // Включение ленивой загрузки
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveConversion<string>();
        }
    }
}
