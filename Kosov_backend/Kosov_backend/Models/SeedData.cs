using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Kosov_backend.Data;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Kosov_backend.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Kosov_backendContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<Kosov_backendContext>>()))
            {

                // Add Athelets if not exists
                if (!context.Athelets.Any())
                {
                    context.Athelets.AddRange(
                        new Athlete
                        {
                            Name = "John Doe",
                            Age = 30,
                            Weight = 75.0,
                            Height = 180.0
                        },
                        new Athlete
                        {
                            Name = "Jane Smith",
                            Age = 28,
                            Weight = 60.0,
                            Height = 165.0
                        }
                    );
                }

                // Add exercises if not exists
                if (!context.Exercises.Any())
                {
                    context.Exercises.AddRange(
                        new Exercise { Name = "Push-ups", Type = "Strength", CaloriesBurned = 100 },
                        new Exercise { Name = "Running", Type = "Cardio", CaloriesBurned = 300 },
                        new Exercise { Name = "Plank", Type = "Strength", CaloriesBurned = 50 },
                        new Exercise { Name = "Cycling", Type = "Cardio", CaloriesBurned = 250 }
                    );
                }

                // Add workout plans if not exists
                if (!context.WorkoutPlans.Any())
                {
                    context.WorkoutPlans.AddRange(
                        new WorkoutPlan
                        {
                            Name = "Morning Routine",
                            Difficulty = "Medium",
                            AtheletId = 1 // Assuming the Athelet exists
                        },
                        new WorkoutPlan
                        {
                            Name = "Evening Cardio",
                            Difficulty = "Hard",
                            AtheletId = 2 // Assuming the Athelet exists
                        }
                    );
                }

                context.SaveChanges();
            }
        }
    }
}