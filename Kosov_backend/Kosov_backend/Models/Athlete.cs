using System.ComponentModel.DataAnnotations;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using NuGet.Protocol.Plugins;
using NuGet.Common;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Kosov_backend.Models
{
    public class Athlete
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }

        public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
    }
}

