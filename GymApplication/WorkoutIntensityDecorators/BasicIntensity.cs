using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutIntensityDecorators
{
    // Concrete component provides the basic implementation
    public class BasicIntensity : WorkoutComponent
    {
        public override int Operation()
        {
            return 3;
        }
    }
}
