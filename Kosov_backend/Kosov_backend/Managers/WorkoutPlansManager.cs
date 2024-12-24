using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Kosov_backend.Data;
using Kosov_backend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Kosov_backend.Managers
{
    public class WorkoutPlansManager
    {
        private readonly Kosov_backendContext _context;

        public WorkoutPlansManager(Kosov_backendContext context)
        {
            _context = context;
        }

        // Get all workout plans
        public async Task<IEnumerable<WorkoutPlan>> GetAllWorkoutPlansAsync()
        {
            return await _context.WorkoutPlans
                .Include(wp => wp.Exercises)
                .ToListAsync();
        }

        // Get a workout plan by ID
        public async Task<WorkoutPlan?> GetWorkoutPlanByIdAsync(int id)
        {
            return await _context.WorkoutPlans
                .Include(wp => wp.Exercises)
                .FirstOrDefaultAsync(wp => wp.Id == id);
        }

        // Add a new workout plan
        public async Task<WorkoutPlan> AddWorkoutPlanAsync(WorkoutPlan workoutPlan)
        {
            _context.WorkoutPlans.Add(workoutPlan);
            await _context.SaveChangesAsync();
            return workoutPlan;
        }

        // Update an existing workout plan
        public async Task<bool> UpdateWorkoutPlanAsync(WorkoutPlan workoutPlan)
        {
            var existingPlan = await _context.WorkoutPlans.FindAsync(workoutPlan.Id);
            if (existingPlan == null)
                return false;

            existingPlan.Name = workoutPlan.Name;
            existingPlan.Difficulty = workoutPlan.Difficulty;
            existingPlan.AtheletId = workoutPlan.AtheletId;

            await _context.SaveChangesAsync();
            return true;
        }

        // Delete a workout plan
        public async Task<bool> DeleteWorkoutPlanAsync(int id)
        {
            var workoutPlan = await _context.WorkoutPlans.FindAsync(id);
            if (workoutPlan == null)
                return false;

            _context.WorkoutPlans.Remove(workoutPlan);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get exercises for a workout plan
        public async Task<IEnumerable<Exercise>> GetExercisesForWorkoutPlanAsync(int workoutPlanId)
        {
            return await _context.Exercises
                .Where(e => e.WorkoutPlans.Any(wp => wp.Id == workoutPlanId))
                .ToListAsync();
        }

        // Add exercises to a workout plan
        public async Task<bool> AddExercisesToWorkoutPlanAsync(int workoutPlanId, List<int> exerciseIds)
        {
            var workoutPlan = await _context.WorkoutPlans.FindAsync(workoutPlanId);
            if (workoutPlan == null)
                return false;

            foreach (var exerciseId in exerciseIds)
            {
                var exercise = await _context.Exercises.FindAsync(exerciseId);
                if (exercise != null)
                {
                    workoutPlan.Exercises.Add(exercise);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
