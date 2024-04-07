using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutLogic
{
    public class Exercise
    {
        public string Name { get; set; }
        public double IntensityEffect { get; set; } // Effect of intensity on the exercise, so, example, the weight of the bench press
        public string IntensitySpecification { get; set; } // Description of what the intensity represents, for example, the kilograms of the bench press
        public string Description { get; set; } // Description of the exercise
        public string ImagePath => $"../images/{Name.Replace(" ", string.Empty)}.jpg"; // Image path for the exercise
    }
}
