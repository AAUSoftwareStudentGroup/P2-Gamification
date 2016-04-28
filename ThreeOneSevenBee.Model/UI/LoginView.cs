using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class LoginView : FrameView
    {
        public Action<string, string> OnLogin;

		protected LabelView Status;
        public void ShowLoginError()
        {
			Status.Text = "Forkert brugernavn eller kode";
        }

        public LoginView(double width, double height) : base(width, height)
        {

            Inputbox username = new Inputbox("Username");
            username.Text = "TannerHelland";
            username.X = 75;
            username.Y = 100;
            username.Width = 450;
            username.Height = 36;
            username.Align = TextAlignment.Left;

            Inputbox password = new Inputbox("Password", true);
            password.Text = "adminadmin";
            password.X = 75;
            password.Y = 160;
            password.Width = 450;
            password.Height = 36;
            password.Align = TextAlignment.Left;

            ButtonView submit = new ButtonView("Log in", () => {
                if (OnLogin != null)
                    OnLogin(username.Text, password.Text);
            });
            submit.X = 220;
            submit.Y = 220;
            submit.Width = 160;
            submit.Height = 60;
            submit.BackgroundColor = new Color(40, 175, 100);

            Status = new LabelView(" ");
            Status.X = 75;
            Status.Y = 200;
            Status.Width = 200;
            Status.Height = 30;

            setContent(new CompositeView(600, 400)
            {
                Children = new List<View>() {
                    username,
                    password,
                    submit,
                    Status
                }
            });
        }
    }
}
