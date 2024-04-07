using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymApplication.WorkoutCreation;

namespace GymApplication.WorkoutInspection
{
    public interface IWorkoutInspector
    {
        string Inspect(IWorkout workout);
    }
}
