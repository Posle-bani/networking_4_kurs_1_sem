using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Kosov_backend.Models
{
    public class WorkoutPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Difficulty { get; set; }

        public int AtheletId { get; set; }
        public virtual Athlete Athelet { get; set; }

        public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}
