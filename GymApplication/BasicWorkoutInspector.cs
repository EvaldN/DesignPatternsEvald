using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication
{
    public class BasicWorkoutInspector : IWorkoutInspector
    {
        public string Inspect(IWorkout workout)
        {
            // Display just the excercises
            return $"Workout: {workout.Name}\nExercises: {string.Join(", ", workout.Exercises.Select(exercise => exercise.Name))}";
        }
    }
}
