using GymApplication.Observer;
using GymApplication.WorkoutLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication
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

            MainThread.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    Debug.WriteLine("Inside BeginInvokeOnMainThread");
                    if (_profileListView == null)
                    {
                        Debug.WriteLine("_profileListView is null");
                        return;
                    }

                    if (profiles == null)
                    {
                        Debug.WriteLine("profiles is null");
                        return;
                    }

                    // Refresh the profiles list
                    Debug.WriteLine("Setting ItemsSource to null");
                    _profileListView.ItemsSource = null;

                    Debug.WriteLine("Setting ItemsSource to profiles");
                    _profileListView.ItemsSource = profiles;

                    // Refresh the totals labels according to the change of the main page's profiles state
                    _totalProfilesLabel.Text = string.Empty;
                    _totalProfilesLabel.Text = "Total profiles: " + (profiles.Count - 1);

                    Debug.WriteLine(profiles);

                    int workoutCount = 0;
                    foreach (Profile profile in profiles)
                    {
                        workoutCount += profile.Workouts.Count;
                    }
                    _totalWorkoutsLabel.Text = string.Empty;
                    _totalWorkoutsLabel.Text = "Total workouts: " + workoutCount.ToString();
                }
                catch (System.Runtime.InteropServices.COMException comEx)
                {
                    Debug.WriteLine("COMException occurred: " + comEx.Message);
                    Debug.WriteLine("Stack Trace: " + comEx.StackTrace);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception occurred: " + ex.Message);
                    Debug.WriteLine("Stack Trace: " + ex.StackTrace);
                }
            });
        }
    }
}
