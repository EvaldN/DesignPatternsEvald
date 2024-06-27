using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymApplication.Observer;
using GymApplication.WorkoutLogic;

namespace GymApplication
{
    internal class CsvUpdater : IObserver
    {
        private List<Profile> profiles;

        public CsvUpdater(List<Profile> profiles)
        {
            this.profiles = profiles;
        }
        public void Update()
        {
            Debug.WriteLine("Updating csv by listening");
            ExportWorkoutsToCsv();
        }
        private string GetProjectDirectory()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string targetPath = Path.GetDirectoryName(basePath);

            while (Path.GetFileName(targetPath) != "GymApplication" && !string.IsNullOrEmpty(targetPath))
            {
                targetPath = Path.GetDirectoryName(targetPath); // Go up one more directory, helps when launching the project in debug mode
            }
            return targetPath;
        }
        private void ExportWorkoutsToCsv()
        {

            // Specify the file path
            string filePath = Path.Combine(GetProjectDirectory(), "profiles.csv");

            // Define the CSV content
            var csvContent = new StringBuilder();

            // Append header
            csvContent.AppendLine("Profile Name,Workout Type,Workout Name,Intensity,Exercises");

            // Iterate through each profile
            foreach (var profile in profiles)
            {
                // Iterate through each workout in the profile
                foreach (var workout in profile.Workouts)
                {
                    // Get the type of the workout
                    var workoutType = workout.GetType().Name;

                    // Extract exercise names
                    var exerciseNames = workout.Exercises.Select(exercise => exercise.Name);

                    // Append profile and workout information
                    csvContent.AppendLine($"{profile.Name},{workoutType},{workout.Name},{workout.Intensity},{string.Join(",", exerciseNames)}");
                }
            }

            // Write CSV content to file (replacing existing content)
            File.WriteAllText(filePath, csvContent.ToString());
        }
    }
}
