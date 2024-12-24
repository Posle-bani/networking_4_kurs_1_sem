using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kosov_backend.Data;
using Kosov_backend.Managers;
using Kosov_backend.Models;

namespace Kosov_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly ExercisesManager _exercisesManager;

        public ExercisesController(ExercisesManager exercisesManager)
        {
            _exercisesManager = exercisesManager;
        }

        // Get all exercises
        [HttpGet]
        public async Task<IActionResult> GetAllExercises()
        {
            var exercises = await _exercisesManager.GetAllExercisesAsync();
            return Ok(new { Message = "Exercises retrieved successfully.", Data = exercises });
        }

        // Get exercise by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExerciseById(int id)
        {
            var exercise = await _exercisesManager.GetExerciseByIdAsync(id);
            if (exercise == null)
                return NotFound(new { Message = "Exercise not found." });

            return Ok(new { Message = "Exercise retrieved successfully.", Data = exercise });
        }

        // Add a new exercise
        [HttpPost]
        public async Task<IActionResult> AddExercise([FromBody] Exercise exercise)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid exercise data.", Errors = ModelState });

            var createdExercise = await _exercisesManager.AddExerciseAsync(exercise);
            return CreatedAtAction(nameof(GetExerciseById), new { id = createdExercise.Id }, new { Message = "Exercise added successfully.", Data = createdExercise });
        }

        // Update an existing exercise
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExercise(int id, [FromBody] Exercise exercise)
        {
            if (id != exercise.Id)
                return BadRequest(new { Message = "Exercise ID mismatch." });

            var success = await _exercisesManager.UpdateExerciseAsync(exercise);
            if (!success)
                return NotFound(new { Message = "Exercise not found." });

            return NoContent();
        }

        // Delete an exercise
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            var success = await _exercisesManager.DeleteExerciseAsync(id);
            if (!success)
                return NotFound(new { Message = "Exercise not found." });

            return Ok(new { Message = "Exercise deleted successfully." });
        }

        // Get workout plans containing an exercise
        [HttpGet("{exerciseId}/workoutplans")]
        public async Task<IActionResult> GetWorkoutPlansForExercise(int exerciseId)
        {
            var workoutPlans = await _exercisesManager.GetWorkoutPlansForExerciseAsync(exerciseId);
            return Ok(new { Message = "Workout plans retrieved successfully for the exercise.", Data = workoutPlans });
        }

        // Get exercises by type
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetExercisesByType(string type)
        {
            var exercises = await _exercisesManager.GetExercisesByTypeAsync(type);
            return Ok(new { Message = "Exercises retrieved successfully by type.", Data = exercises });
        }
    }
}
