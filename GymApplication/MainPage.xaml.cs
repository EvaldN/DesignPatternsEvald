using GymApplicatio;
using GymApplication.Observer;
using GymApplication.WorkoutInspection;
using GymApplication.WorkoutLogic;
using GymApplication.WorkoutIntensityDecorators;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Diagnostics;
using System.Text;
using System.Globalization;

namespace GymApplication
{
    public partial class MainPage : ContentPage, IObservable 
    {
        public List<Profile> profiles = new List<Profile>();

        private List<IObserver> observers = new List<IObserver>();

        public MainPage()
        {
            InitializeComponent();
            InitializeData(); // Load default data, aka, the unassigned """profile"""
            ImportWorkouts();

            CsvUpdater csvUpdater = new CsvUpdater(profiles);
            UIUpdater uiUpdater = new UIUpdater(ProfileListView, profiles, TotalProfilesLabel, TotalWorkoutsLabel);
            Attach(csvUpdater);
            Attach(uiUpdater);
            Notify();
        }
        private void InitializeData()
        {
            // Create the unassigned """profile""", easier to keep unassigned workouts this way.
            var unassignedProfile = new Profile { Name = "Unassigned" };

            profiles.Add(unassignedProfile); // Add unassigned """profile""" to profiles list
        }

        private async void OnCreateProfileClicked(object sender, EventArgs e)
        {
            string profileName = await DisplayPromptAsync("Create Profile", "Enter profile name:", "Create", "Cancel", "New Profile");
            if (!string.IsNullOrWhiteSpace(profileName))
            {
                // Create new profile with the entered name
                var newProfile = new Profile { Name = profileName };
                profiles.Add(newProfile);

                Notify();
            }
        }

        private async void OnAssignWorkoutClicked(object sender, EventArgs e)
        {
            // Get the "Unassigned" profile because that's where all un
            var unassignedProfile = profiles.FirstOrDefault(p => p.Name == "Unassigned");
            if (unassignedProfile == null)
            {
                // If somehow "Unassigned" was not created, do nothing
                return;
            }

            // Get unassigned workouts
            var unassignedWorkouts = unassignedProfile.Workouts;

            // Display a pop-up to select a workout from the "Unassigned" profile
            var selectedWorkout = await DisplayActionSheet("Select Workout", "Cancel", null, unassignedWorkouts.Select(workout => workout.Name).ToArray());

            if (selectedWorkout != null && selectedWorkout != "Cancel")
            {
                var workout = unassignedWorkouts.FirstOrDefault(w => w.Name == selectedWorkout);

                // Display a pop-up to select a profile to which to assign the workout
                var selectedProfile = await DisplayActionSheet("Select Profile", "Cancel", null, profiles.Select(profile => profile.Name).ToArray());

                if (selectedProfile != null && selectedProfile != "Cancel")
                {
                    var profile = profiles.FirstOrDefault(p => p.Name == selectedProfile);

                    // Assign the selected workout to the selected profile
                    profile.Workouts.Add(workout);

                    // Remove the workout from the "Unassigned" profile
                    unassignedProfile.Workouts.Remove(workout);
                    
                    // Notify the updater-csv saver
                    Notify();
                }
            }
        }
        
        private async void OnDeleteProfileClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var action = await DisplayActionSheet("Confirm removing the profile?", "Cancel", "Yes");
                if (action == "Yes")
                {
                var profileName = button.CommandParameter as string;
                    if (profileName != null && profileName != "Unassigned")
                    {
                        var profileToDelete = profiles.FirstOrDefault(p => p.Name == profileName);
                        if (profileToDelete != null)
                        {
                            // Reassign all workouts from the profile to the "Unassigned" profile
                            var unassignedProfile = profiles.FirstOrDefault(p => p.Name == "Unassigned");
                            if (unassignedProfile != null)
                            {
                                foreach (var workout in profileToDelete.Workouts)
                                {
                                    unassignedProfile.Workouts.Add(workout);
                                }
                            }

                            // Remove the profile from the list
                            profiles.Remove(profileToDelete);

                            Notify();
                        }
                    }
                }
            }
        }

        private async void OnCreateWorkoutClicked(object sender, EventArgs e)
        {
            // Enter workout name
            string name = await DisplayPromptAsync("Create Workout", "Enter workout name:");
            if (string.IsNullOrWhiteSpace(name))
                return; // Do nothing if the name is empty

            // Enter workout type (can choose from the enums found in WorkoutFactory, pretty scalable)
            var selectedType = await DisplayActionSheet("Select Workout Type", "Cancel", null, Enum.GetNames(typeof(WorkoutType)));
            if (selectedType == "Cancel")
                return;

            // Parse the selected type
            if (!Enum.TryParse(selectedType, out WorkoutType type))
                return;

            // Choose intensity calculation method
            var selectedMethod = await DisplayActionSheet("Select Intensity Calculation Method", "Cancel", null, "Basic", "BMI-based", "Fitness-based", "Combined");
            if (selectedMethod == "Cancel")
                return;

            double intensity = 1;
            WorkoutComponent workoutComponent = new BasicIntensity();

            // Basic should always return 3
            if (selectedMethod == "Basic")
            {
                intensity = workoutComponent.Operation();
            }
            // Based on input, BMI is calculated and assigned the easiest to underweight, medium to overweight and hardest to standard.
            else if (selectedMethod == "BMI-based")
            {
                // Grab height and weight
                string heightInput = await DisplayPromptAsync("Create Workout", "Enter height (meters):");
                string weightInput = await DisplayPromptAsync("Create Workout", "Enter weight (kilograms):");

                // Attempt to parse the inputs using different cultures, so things like x.x or y,y are accounted for
                if (double.TryParse(heightInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double height) ||
                    double.TryParse(heightInput, NumberStyles.Any, CultureInfo.GetCultureInfo("fr-FR"), out height))
                {
                    if (double.TryParse(weightInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double weight) ||
                        double.TryParse(weightInput, NumberStyles.Any, CultureInfo.GetCultureInfo("fr-FR"), out weight))
                    {
                        Debug.WriteLine(height);
                        Debug.WriteLine(weight);
                        // Run the BMI intensity decorator
                        workoutComponent = new BMIIntensityDecorator(workoutComponent, height, weight);
                        intensity = workoutComponent.Operation();
                    }
                    else
                    {
                        // Invalid weight input handling
                        await DisplayAlert("Invalid Input", "Please enter valid numbers for weight.", "OK");
                        return;
                    }
                }
                else
                {
                    // Invalid height input handling
                    await DisplayAlert("Invalid Input", "Please enter valid numbers for height.", "OK");
                    return;
                }
            }

            // Assigns harder intensity to more fit choice, base is 2, intermediate 5, advanced 7.
            else if (selectedMethod == "Fitness-based")
            {
                // Grab the experience level string
                string experienceLevel = await DisplayActionSheet("Select your fitness level", "Cancel", null, "Beginner", "Intermediate", "Advanced");
                // Run the fitness intensity decorator
                workoutComponent = new FitnessLevelIntensityDecorator(workoutComponent, experienceLevel);
                intensity = workoutComponent.Operation();
            }
            // Combines the two previous decorators into the composite one and based on input returns their average.
            else if (selectedMethod == "Combined")
            {
                // Grab all previously needed inputs.
                string heightInput = await DisplayPromptAsync("Create Workout", "Enter height (meters):");
                string weightInput = await DisplayPromptAsync("Create Workout", "Enter weight (kilograms):");
                string experienceLevel = await DisplayActionSheet("Select your fitness level", "Cancel", null, "Beginner", "Intermediate", "Advanced");
                // Parse height and weight into doubles
                if (double.TryParse(heightInput, out double height) && double.TryParse(weightInput, out double weight))
                {
                    // Create a list of decorators
                    var decorators = new List<IntensityDecorator>
                    {
                        new BMIIntensityDecorator(workoutComponent, height, weight),
                        new FitnessLevelIntensityDecorator(workoutComponent, experienceLevel),
                    };
                    // Execute the composite intensity decorator
                    workoutComponent = new CompositeIntensityDecorator(workoutComponent, decorators);

                    intensity = workoutComponent.Operation();
                }
                else
                {
                    // Invalid input handling
                    await DisplayAlert("Invalid Input", "Please enter valid numbers for height and weight.", "OK");
                    return;
                }

            }

            Debug.WriteLine(intensity);

            // Create the workout based on the selected type, intensity, and chosen method
            IWorkout workout = WorkoutFactory.CreateWorkout(type, name, intensity);

            // Assign the workout to the "Unassigned" profile
            var unassignedProfile = profiles.FirstOrDefault(p => p.Name == "Unassigned");
            unassignedProfile?.Workouts.Add(workout);

            Notify();
        }


        private async void OnInspectWorkoutClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var workout = button?.CommandParameter as IWorkout;

            if (workout == null)
                return;

            IWorkoutInspector inspector = new BasicWorkoutInspector(); // Base inspector

            var options = await DisplayActionSheet("Choose inspection options", "Cancel", null, "Basic", "Add Descriptions", "Add Images", "Add Descriptions and Images");

            // Couldn't change this into check marks
            switch (options)
            {
                case "Basic":
                    //base decorator used
                    break;
                case "Add Descriptions":
                    inspector = new DescriptionDecorator(inspector); // Add descriptions decorator
                    break;
                case "Add Images":
                    inspector = new ImageDecorator(inspector); // Add images decorator
                    break;
                case "Add Descriptions and Images":
                    inspector = new ImageDecorator(new DescriptionDecorator(inspector)); // Add both descriptions and images decorators
                    break;
                default:
                    break;
            }
            var inspectionResult = inspector.Inspect(workout);
            await DisplayAlert("Workout Inspection", inspectionResult, "OK");
        }
        
        private async void OnDeleteWorkoutClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            var selectedProfile = await DisplayActionSheet("Select Profile", "Cancel", null, profiles.Select(profile => profile.Name).ToArray());

            if (selectedProfile != null && selectedProfile != "Cancel")
            {
                var profile = profiles.FirstOrDefault(w => w.Name == selectedProfile);
                var selectedWorkout = await DisplayActionSheet("Select Workout", "Cancel", null, profile.Workouts.Select(workout => workout.Name).ToArray());
                if (selectedProfile != null && selectedProfile != "Cancel")
                {
                    var workout = profile.Workouts.FirstOrDefault(w => w.Name == selectedWorkout);
                    var action = await DisplayActionSheet("Confirm removing the workout?", "Cancel", "Yes");
                    if (action == "Yes")
                    {
                        profile.Workouts.Remove(workout);
                        workout = null;
                        Notify();
                    }
                }
            }
        }

        private string GetProjectDirectory()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string targetPath = Path.GetDirectoryName(basePath);

            while (Path.GetFileName(targetPath) != "GymApplication" && !string.IsNullOrEmpty(targetPath))
            {
                targetPath = Path.GetDirectoryName(targetPath); // Go up one more directory
            }
            return targetPath;
        }

        private void ImportWorkouts()
        {

            // Specify the file path
            string filePath = Path.Combine(GetProjectDirectory(), "profiles.csv");

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                return;
            }

            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(filePath);

            // Skip the header line
            var dataLines = lines.Skip(1);

            // Iterate through each line
            foreach (var line in dataLines)
            {
                // Split the line into fields
                var fields = line.Split(',');

                // Extract profile and workout information
                string profileName = fields[0];
                string workoutType = fields[1];
                string workoutName = fields[2];
                string intensity = fields[3];
                string[] exerciseNames = fields[4].Split(',');

                workoutType = workoutType.Substring(0, workoutType.IndexOf("Workout"));
                // Use the extracted information to create and add workouts to the profiles

                bool profileExists = profiles.Any(profile => profile.Name == profileName);

                if (!profileExists)
                {
                    // Create new profile with the entered name
                    var newProfile = new Profile { Name = profileName };
                    profiles.Add(newProfile);

                }
                else
                {
                    // Do nothing, assuming the profile was already created and the workouts with its name are going to be populated in it.
                }

                if (!Enum.TryParse(workoutType, out WorkoutType type))
                {
                    return;
                }

                IWorkout workout = WorkoutFactory.CreateWorkout(type, workoutName, int.Parse(intensity));

                if (workout == null)
                {
                    return;
                }

                var profile = profiles.FirstOrDefault(p => p.Name == profileName);

                if (profile == null)
                {
                    return;
                }

                // Assign the selected workout to the selected profile
                profile.Workouts.Add(workout);

                // Refresh the profiles list, still doing this on launch
                ProfileListView.ItemsSource = null;
                ProfileListView.ItemsSource = profiles;

            }
        }
        private void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        private void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        private void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }
    }
}

