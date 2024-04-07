using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymApplication.WorkoutCreation;

namespace GymApplication.WorkoutInspection
{
    public class ImageDecorator : IWorkoutInspector
    {
        private readonly IWorkoutInspector _workoutInspector;

        public ImageDecorator(IWorkoutInspector workoutInspector)
        {
            _workoutInspector = workoutInspector;
        }

        public string Inspect(IWorkout workout)
        {
            // Get the basic inspection
            var basicInspection = _workoutInspector.Inspect(workout);

            // Create image inspection
            var images = string.Join("\n", workout.Exercises.Select(exercise => $"{exercise.Name}\n{exercise.ImagePath}"));

            // Append the base with the image view
            return $"{basicInspection}\n{images}";
        }
    }
}
