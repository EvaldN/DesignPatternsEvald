﻿using GymApplication.Observer;
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

            Device.BeginInvokeOnMainThread(async ()  =>
            {
                try
                {
                    // Clear and update ItemsSource
                    _profileListView.ItemsSource = null;
                    _profileListView.ItemsSource = profiles; // Set new items

                    // Update other UI elements
                    _totalProfilesLabel.Text = "Total profiles: " + (profiles.Count - 1);

                    int workoutCount = profiles.Sum(profile => profile.Workouts.Count);
                    _totalWorkoutsLabel.Text = "Total workouts: " + workoutCount.ToString();
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
