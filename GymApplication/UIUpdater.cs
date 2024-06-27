using GymApplication.Observer;
using GymApplication.WorkoutLogic;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GymApplication
{
    internal class UIUpdater : IObserver
    {
        private ObservableCollection<Profile> profiles;
        private ListView _profileListView;
        private Label _totalProfilesLabel;
        private Label _totalWorkoutsLabel;

        public UIUpdater(ListView profileListView, ObservableCollection<Profile> profiles, Label totalProfilesLabel, Label totalWorkoutsLabel)
        {
            this._profileListView = profileListView;
            this.profiles = profiles;
            _totalProfilesLabel = totalProfilesLabel;
            _totalWorkoutsLabel = totalWorkoutsLabel;

            // Initial UI update
            Update();
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

                    // The ObservableCollection automatically updates the ListView, no need to set ItemsSource to null and reassign
                    _profileListView.ItemsSource = profiles;

                    // Update labels
                    _totalProfilesLabel.Text = $"Total profiles: {profiles.Count - 1}";

                    int workoutCount = 0;
                    foreach (Profile profile in profiles)
                    {
                        workoutCount += profile.Workouts.Count;
                    }
                    _totalWorkoutsLabel.Text = $"Total workouts: {workoutCount}";
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
