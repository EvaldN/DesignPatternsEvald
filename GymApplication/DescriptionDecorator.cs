using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication
{
    public class DescriptionDecorator : IWorkoutInspector
    {
        private readonly IWorkoutInspector _workoutInspector;

        public DescriptionDecorator(IWorkoutInspector workoutInspector)
        {
            _workoutInspector = workoutInspector;
        }

        public string Inspect(IWorkout workout)
        {
            // Get the basic inspection
            var basicInspection = _workoutInspector.Inspect(workout);

            // Create description code
            var description = string.Join("\n", workout.Exercises.Select(exercise => $"{exercise.Name} - {exercise.Description}"));

            // Append the base with the description code
            return $"{basicInspection}\n{description}";
        }
    }
}
