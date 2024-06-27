using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutIntensityDecorators
{
    // Component interface defines the contract for concrete components and decorators
    public abstract class WorkoutComponent
    {
        public abstract int Operation();
    }
}
