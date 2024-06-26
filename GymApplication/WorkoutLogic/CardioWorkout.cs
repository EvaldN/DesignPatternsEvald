using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutLogic
{
    public class CardioWorkout : IWorkout
    {
        public string Name { get; private set; }
        public double Intensity { get; private set; }
        public List<Exercise> Exercises { get; private set; }

        public CardioWorkout(string name, double intensity, List<Exercise> exercises)
        {
            Name = name;
            Intensity = intensity;
            Exercises = exercises;
        }
    }
}
