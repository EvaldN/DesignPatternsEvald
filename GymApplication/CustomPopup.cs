using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication
{
    public class CustomPopup : ContentView
    {
        public CustomPopup(string imagePath)
        {
            Content = new StackLayout
            {
                Children = {
                new Image { Source = imagePath }
            }
            };
        }
    }
}
