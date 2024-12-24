using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Kosov_backend.Data;
using Kosov_backend.Models;
using System.Collections.Generic;

namespace Kosov_backend.Managers 
{
    public class AthletesManager
    {
        private readonly Kosov_backendContext _context;

        public AthletesManager(Kosov_backendContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Athlete>> GetAllAtheletsAsync()
        {
            return await _context.Athelets
                .Include(u => u.WorkoutPlans)
                .ThenInclude(wp => wp.Exercises)
                .ToListAsync();
        }

        // Get Athelet by ID
        public async Task<Athlete?> GetAtheletByIdAsync(int id)
        {
            return await _context.Athelets
                .Include(u => u.WorkoutPlans)
                .ThenInclude(wp => wp.Exercises)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Add a new Athelet
        public async Task<Athlete> AddAtheletAsync(Athlete Athelet)
        {
            _context.Athelets.Add(Athelet);
            await _context.SaveChangesAsync();
            return Athelet;
        }

        // Update an existing Athelet
        public async Task<bool> UpdateAtheletAsync(Athlete Athelet)
        {
            var existingAthelet = await _context.Athelets.FindAsync(Athelet.Id);
            if (existingAthelet == null)
                return false;

            existingAthelet.Name = Athelet.Name;
            existingAthelet.Age = Athelet.Age;
            existingAthelet.Weight = Athelet.Weight;
            existingAthelet.Height = Athelet.Height;

            await _context.SaveChangesAsync();
            return true;
        }

        // Delete a Athelet
        public async Task<bool> DeleteAtheletAsync(int id)
        {
            var Athelet = await _context.Athelets.FindAsync(id);
            if (Athelet == null)
                return false;

            _context.Athelets.Remove(Athelet);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get workout plans for a Athelet
        public async Task<IEnumerable<WorkoutPlan>> GetWorkoutPlansForAtheletAsync(int AtheletId)
        {
            return await _context.WorkoutPlans
                .Where(wp => wp.AtheletId == AtheletId)
                .Include(wp => wp.Exercises)
                .ToListAsync();
        }

    }
}

