using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutLogic
{
    public class Profile
    {
        public string Name { get; set; }
        public List<IWorkout> Workouts { get; set; } = new List<IWorkout>();
    }
}
