﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutLogic
{
    public class FlexibilityWorkout : IWorkout
    {
        public string Name { get; private set; }
        public double Intensity { get; private set; }
        public List<Exercise> Exercises { get; private set; }

        public FlexibilityWorkout(string name, double intensity, List<Exercise> exercises)
        {
            Name = name;
            Intensity = intensity;
            Exercises = exercises;
        }
    }
}
