using GymApplication.Observer;
using GymApplication.WorkoutLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplicatio
{
    internal class UIUpdater : IObserver
{
        private List<Profile> profiles;
        private ListView _profileListView;
        private Label _totalProfilesLabel;
        private Label _totalWorkoutsLabel;
        public UIUpdater(ListView ProfileListView, List<Profile> profiles, Label totalProfilesLabel, Label totalWorkoutsLabel)
        {
            this._profileListView = ProfileListView;
            this.profiles = profiles;
            _totalProfilesLabel = totalProfilesLabel;
            _totalWorkoutsLabel = totalWorkoutsLabel;
        }
        public void Update()
        {
            Debug.WriteLine("Updating front-end by listening");
            // Refresh the profiles list
            _profileListView.ItemsSource = null;
            _profileListView.ItemsSource = profiles;

            _totalProfilesLabel.Text = string.Empty;
            _totalProfilesLabel.Text = "Total profiles: " + (profiles.Count - 1);
            Debug.WriteLine(profiles);
            int workoutCount = 0;
            foreach (Profile profile in profiles) { workoutCount += profile.Workouts.Count; }
            _totalWorkoutsLabel.Text = string.Empty;
            _totalWorkoutsLabel.Text = "Total workouts: " + workoutCount.ToString();
        }
    }
}
