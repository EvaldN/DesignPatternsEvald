using Microsoft.Maui.Controls;

namespace GymApplication
{
    public partial class MainPage : ContentPage
    {
        private List<Profile> profiles = new List<Profile>();

        public MainPage()
        {
            InitializeComponent();
            InitializeData(); // Load default data, aka, the unassigned """profile"""
            PopulateProfiles();
        }

        private void InitializeData()
        {
            // Create the unassigned """profile""", easier to keep unassigned workouts this way.
            var unassignedProfile = new Profile { Name = "Unassigned" };

            profiles.Add(unassignedProfile); // Add unassigned """profile""" to profiles list
        }

        private void PopulateProfiles()
        {
            // Need this for front-end, populates it with currently existing profiles
            ProfileListView.ItemsSource = profiles;
        }

        private async void OnCreateProfileClicked(object sender, EventArgs e)
        {
            string profileName = await DisplayPromptAsync("Create Profile", "Enter profile name:", "Create", "Cancel", "New Profile");
            if (!string.IsNullOrWhiteSpace(profileName))
            {
                // Create new profile with the entered name
                var newProfile = new Profile { Name = profileName };
                profiles.Add(newProfile);

                // Refresh the profiles list
                ProfileListView.ItemsSource = null;
                ProfileListView.ItemsSource = profiles;
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

                    // Refresh the profiles list
                    ProfileListView.ItemsSource = null;
                    ProfileListView.ItemsSource = profiles;
                }
            }
        }
        private void OnDeleteProfileClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
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

                        // Refresh the profiles list
                        ProfileListView.ItemsSource = null;
                        ProfileListView.ItemsSource = profiles;
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

            // Enter workout intensity
            int intensity;
            bool intensityValid = int.TryParse(await DisplayPromptAsync("Create Workout", "Enter workout intensity (1-10):"), out intensity);
            if (!intensityValid || intensity < 1 || intensity > 10)
                return; // Do nothing if intensity is not valid

            // Create the workout based on the selected type and intensity
            IWorkout workout = WorkoutFactory.CreateWorkout(type, name, intensity);

            // Assign the workout to the "Unassigned" profile
            var unassignedProfile = profiles.FirstOrDefault(p => p.Name == "Unassigned");
            unassignedProfile?.Workouts.Add(workout);

            // Refresh the profiles list
            ProfileListView.ItemsSource = null;
            ProfileListView.ItemsSource = profiles;
        }
        private async void OnInspectWorkoutClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var workout = button?.CommandParameter as IWorkout;

            if (workout == null)
                return;

            IWorkoutInspector inspector = new BasicWorkoutInspector(); // Base inspector

            var options = await DisplayActionSheet("Choose inspection options", "Cancel", null, "Basic", "Add Descriptions", "Add Images", "Add Descriptions and Images");

            //Probably going to change this entire system to use radio buttons, this is a bit scuffed
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
    }
}
