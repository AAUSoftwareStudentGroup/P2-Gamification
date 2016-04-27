using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class LoginView : CompositeView
    {
        Inputbox username;
        public Action<string, string> OnLogin;

        public void ShowLoginError()
        {

        }

        public LoginView(double width, double height) : base(width, height)
        {
            username = new Inputbox("Username");
            username.X = 100;
            username.Y = 100;
            username.Width = 300;
			username.Height = 24;
            Children = new List<View>() {
                username
            };
        }
    }
}
