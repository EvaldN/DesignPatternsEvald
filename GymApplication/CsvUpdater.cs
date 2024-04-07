using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication
{
    internal class CsvUpdater : IObserver
    {
        private List<Profile> profiles;
        private ListView ProfileListView;

        public CsvUpdater(ListView profileListView, List<Profile> profiles)
        {
            this.ProfileListView = profileListView;
            this.profiles = profiles;
        }
        public void Update()
        {
                ExportWorkoutsToCsv();
                // Refresh the profiles list
                ProfileListView.ItemsSource = null;
                ProfileListView.ItemsSource = profiles;
        }

        private void ExportWorkoutsToCsv()
        {
            // Get the desktop folder path
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Specify the file path
            string filePath = Path.Combine(desktopPath, "profiles.csv");

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
