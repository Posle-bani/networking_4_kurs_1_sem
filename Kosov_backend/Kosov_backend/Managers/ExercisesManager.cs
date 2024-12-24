using System.Linq;
using Kosov_backend.Data;
using Kosov_backend.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace Kosov_backend.Managers
{
    public class ExercisesManager
    {
        private readonly Kosov_backendContext _context;

        public ExercisesManager(Kosov_backendContext context)
        {
            _context = context;
        }

        // Get all exercises
        public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
        {
            return await _context.Exercises
                .Include(e => e.WorkoutPlans)
                .ToListAsync();
        }

        // Get exercise by ID
        public async Task<Exercise?> GetExerciseByIdAsync(int id)
        {
            return await _context.Exercises
                .Include(e => e.WorkoutPlans)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Add a new exercise
        public async Task<Exercise> AddExerciseAsync(Exercise exercise)
        {
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
            return exercise;
        }

        // Update an existing exercise
        public async Task<bool> UpdateExerciseAsync(Exercise exercise)
        {
            var existingExercise = await _context.Exercises.FindAsync(exercise.Id);
            if (existingExercise == null)
                return false;

            existingExercise.Name = exercise.Name;
            existingExercise.Type = exercise.Type;
            existingExercise.CaloriesBurned = exercise.CaloriesBurned;

            await _context.SaveChangesAsync();
            return true;
        }

        // Delete an exercise
        public async Task<bool> DeleteExerciseAsync(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
                return false;

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get workout plans containing an exercise
        public async Task<IEnumerable<WorkoutPlan>> GetWorkoutPlansForExerciseAsync(int exerciseId)
        {
            return await _context.WorkoutPlans
                .Where(wp => wp.Exercises.Any(e => e.Id == exerciseId))
                .ToListAsync();
        }

        // Get exercises by type
        public async Task<IEnumerable<Exercise>> GetExercisesByTypeAsync(string type)
        {
            return await _context.Exercises
                .Where(e => e.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
    }
}
