using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Kosov_backend.Models;
using Kosov_backend.Managers;
using Kosov_backend.Data;

namespace Kosov_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutPlansController : ControllerBase
    {
        private readonly WorkoutPlansManager _workoutPlansManager;

        public WorkoutPlansController(WorkoutPlansManager workoutPlansManager)
        {
            _workoutPlansManager = workoutPlansManager;
        }

        // Get all workout plans
        [HttpGet]
        public async Task<IActionResult> GetAllWorkoutPlans()
        {
            var workoutPlans = await _workoutPlansManager.GetAllWorkoutPlansAsync();
            return Ok(new { Message = "Workout plans retrieved successfully.", Data = workoutPlans });
        }

        // Get workout plan by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutPlanById(int id)
        {
            var workoutPlan = await _workoutPlansManager.GetWorkoutPlanByIdAsync(id);
            if (workoutPlan == null)
                return NotFound(new { Message = "Workout plan not found." });

            return Ok(new { Message = "Workout plan retrieved successfully.", Data = workoutPlan });
        }

        // Add a new workout plan
        [HttpPost]
        public async Task<IActionResult> AddWorkoutPlan([FromBody] WorkoutPlan workoutPlan)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid workout plan data.", Errors = ModelState });

            var createdWorkoutPlan = await _workoutPlansManager.AddWorkoutPlanAsync(workoutPlan);
            return CreatedAtAction(nameof(GetWorkoutPlanById), new { id = createdWorkoutPlan.Id }, new { Message = "Workout plan added successfully.", Data = createdWorkoutPlan });
        }

        // Update an existing workout plan
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkoutPlan(int id, [FromBody] WorkoutPlan workoutPlan)
        {
            if (id != workoutPlan.Id)
                return BadRequest(new { Message = "Workout plan ID mismatch." });

            var success = await _workoutPlansManager.UpdateWorkoutPlanAsync(workoutPlan);
            if (!success)
                return NotFound(new { Message = "Workout plan not found." });

            return NoContent();
        }

        // Delete a workout plan
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkoutPlan(int id)
        {
            var success = await _workoutPlansManager.DeleteWorkoutPlanAsync(id);
            if (!success)
                return NotFound(new { Message = "Workout plan not found." });

            return Ok(new { Message = "Workout plan deleted successfully." });
        }

        // Get exercises for a workout plan
        [HttpGet("{workoutPlanId}/exercises")]
        public async Task<IActionResult> GetExercisesForWorkoutPlan(int workoutPlanId)
        {
            var exercises = await _workoutPlansManager.GetExercisesForWorkoutPlanAsync(workoutPlanId);
            return Ok(new { Message = "Exercises retrieved successfully for the workout plan.", Data = exercises });
        }

        // Add exercises to a workout plan
        [HttpPost("{workoutPlanId}/exercises")]
        public async Task<IActionResult> AddExercisesToWorkoutPlan(int workoutPlanId, [FromBody] List<int> exerciseIds)
        {
            var success = await _workoutPlansManager.AddExercisesToWorkoutPlanAsync(workoutPlanId, exerciseIds);
            if (!success)
                return NotFound(new { Message = "Workout plan not found." });

            return Ok(new { Message = "Exercises added to workout plan successfully." });
        }
    }
}