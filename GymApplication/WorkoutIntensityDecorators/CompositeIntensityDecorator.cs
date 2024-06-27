using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutIntensityDecorators
{
    public class CompositeIntensityDecorator : IntensityDecorator
    {
        private List<WorkoutComponent> _componentsToExecute;

        public CompositeIntensityDecorator(WorkoutComponent component, List<WorkoutComponent> decorators) : base(component)
        {
            this._componentsToExecute = decorators;
        }

        public override int Operation()
        {
            int baseIntensity = _component.Operation();
            int sumOfIntensities = 0;
            foreach (var decorator in _componentsToExecute)
            {
                sumOfIntensities += decorator.Operation();
            }
            return sumOfIntensities / _componentsToExecute.Count;
        }
    }
}
