using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication
{
    public interface IWorkoutInspector
    {
        string Inspect(IWorkout workout);
    }
}
