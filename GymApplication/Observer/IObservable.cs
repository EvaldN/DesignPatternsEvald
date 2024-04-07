using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.Observer
{
    internal interface IObservable
    {
        private void Attach(IObserver observer)
        {
        }

        private void Detach(IObserver observer)
        {
        }

        private void Notify()
        {
        }
    }
}
