using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutIntensityDecorators
{
    public class CompositeIntensityDecorator : IntensityDecorator
    {
        private List<IntensityDecorator> _decorators;

        public CompositeIntensityDecorator(WorkoutComponent component, List<IntensityDecorator> decorators) : base(component)
        {
            this._decorators = decorators;
        }

        public override int Operation()
        {
            int baseIntensity = _component.Operation();
            int sumOfIntensities = 0;
            foreach (var decorator in _decorators)
            {
                sumOfIntensities += decorator.Operation();
            }
            return sumOfIntensities / _decorators.Count;
        }
    }
}
