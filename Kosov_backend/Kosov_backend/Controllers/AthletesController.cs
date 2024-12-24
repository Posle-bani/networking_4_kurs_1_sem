using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kosov_backend.Data;
using Kosov_backend.Models;
using Kosov_backend.Managers;

namespace Kosov_backend.Controllers
{
    //public class UpdateAtheletRequest
    //{
    //    public string? TagTelegram { get; set; }
    //    public string? FirstName { get; set; }
    //    public string? LastName { get; set; }
    //    public bool? IsActive { get; set; }
    //    public string? Password { get; set; }
    //}
     
    [Route("api/[controller]")]
    [ApiController]
    public class AthletesController : ControllerBase
    {
        private readonly AthletesManager _atheletsManager;

        public AthletesController(AthletesManager atheletsManager)
        {
            _atheletsManager = atheletsManager;
        }

        // Get all Athelets
        [HttpGet]
        public async Task<IActionResult> GetAllAthelets()
        {
            var athelets = await _atheletsManager.GetAllAtheletsAsync();
            return Ok(new { Message = "Athelets retrieved successfully.", Data = athelets });
        }

        // Get Athelet by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAtheletById(int id)
        {
            var athelet = await _atheletsManager.GetAtheletByIdAsync(id);
            if (athelet == null)
                return NotFound(new { Message = "Athelet not found." });

            return Ok(new { Message = "Athelet retrieved successfully.", Data = athelet });
        }

        // Add a new athelet
        [HttpPost]
        public async Task<IActionResult> AddAthelet([FromBody] Athlete athelet)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid Athelet data.", Errors = ModelState });

            var createdAthelet = await _atheletsManager.AddAtheletAsync(athelet);
            return CreatedAtAction(nameof(GetAtheletById), new { id = createdAthelet.Id }, new { Message = "Athelet added successfully.", Data = createdAthelet });
        }

        // Update an existing athelet
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAthelet(int id, [FromBody] Athlete athelet)
        {
            if (id != athelet.Id)
                return BadRequest(new { Message = "Athelet ID mismatch." });

            var success = await _atheletsManager.UpdateAtheletAsync(athelet);
            if (!success)
                return NotFound(new { Message = "Athelet not found." });

            return NoContent();
        }

        // Delete a Athelet
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAthelet(int id)
        {
            var success = await _atheletsManager.DeleteAtheletAsync(id);
            if (!success)
                return NotFound(new { Message = "Athelet not found." });

            return Ok(new { Message = "Athelet deleted successfully." });
        }

        // Get workout plans for a Athelet
        [HttpGet("{atheletId}/workoutplans")]
        public async Task<IActionResult> GetWorkoutPlansForAthelet(int atheletId)
        {
            var workoutPlans = await _atheletsManager.GetWorkoutPlansForAtheletAsync(atheletId);
            return Ok(new { Message = "Workout plans retrieved successfully.", Data = workoutPlans });
        }
    }
}
