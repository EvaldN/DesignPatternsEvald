using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutLogic
{
    public interface IWorkout
    {
        string Name { get; }
        int Intensity { get; }
        List<Exercise> Exercises { get; }
    }
}
