using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.WorkoutIntensityDecorators
{
    public class BMIIntensityDecorator : IntensityDecorator
    {
        private double _height;
        private double _weight;

        public BMIIntensityDecorator(WorkoutComponent component, double height, double weight) : base(component)
        {
            this._height = height;
            this._weight = weight;
        }

        public override int Operation()
        {
            int baseIntensity = _component.Operation();
            double bmi = _weight / (_height * _height);
            Debug.Write(bmi);

            if (bmi < 18.5)
            {
                return baseIntensity + 1;
            }
            else if (bmi >= 18.5 && bmi < 25)
            {
                return baseIntensity + 3;
            }
            else
            {
                return baseIntensity + 2;
            }
        }
    }
}
