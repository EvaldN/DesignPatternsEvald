using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutIntensityDecorators
{
    public class FitnessLevelIntensityDecorator : IntensityDecorator
    {
        private string _fitnessLevel;

        public FitnessLevelIntensityDecorator(WorkoutComponent component, string fitnessLevel) : base(component)
        {
            this._fitnessLevel = fitnessLevel;
        }

        public override int Operation()
        {
            int baseIntensity = _component.Operation();
            switch (_fitnessLevel.ToLower())
            {
                case "beginner":
                    return baseIntensity - 1;
                case "intermediate":
                    return baseIntensity + 2;
                case "advanced":
                    return baseIntensity + 4;
                default:
                    return baseIntensity;
            }
        }
    }
}
