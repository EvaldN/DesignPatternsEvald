﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication
{
    public class StrengthWorkout : IWorkout
    {
        public string Name { get; private set; }
        public int Intensity { get; private set; }
        public List<Exercise> Exercises { get; private set; }

        public StrengthWorkout(string name, int intensity, List<Exercise> exercises)
        {
            Name = name;
            Intensity = intensity;
            Exercises = exercises;
        }
    }
}
