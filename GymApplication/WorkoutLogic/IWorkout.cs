using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymApplication.WorkoutCreation;

namespace GymApplication
{
    public interface IWorkout
    {
        string Name { get; }
        int Intensity { get; }
        List<Exercise> Exercises { get; }
    }
}
