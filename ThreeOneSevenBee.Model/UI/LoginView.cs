using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class LoginView : CompositeView
    {
        Inputbox username;

        public LoginView() : base(600, 400)
        {
            username = new Inputbox("Username");
            username.X = 100;
            username.Y = 100;
            username.Width = 100;
            Children = new List<View>() {
                username
            };
        }
    }
}
