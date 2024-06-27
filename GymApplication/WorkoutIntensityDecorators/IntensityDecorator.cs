using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutIntensityDecorators
{
    public abstract class IntensityDecorator : WorkoutComponent
    {
        protected WorkoutComponent _component;

        public IntensityDecorator(WorkoutComponent component)
        {
            this._component = component;
        }

        public override int Operation()
        {
            if (_component != null)
            {
                return _component.Operation();
            }
            else
            {
                return -1;
            }
        }
    }
}
