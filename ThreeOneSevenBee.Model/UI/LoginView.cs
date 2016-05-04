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

            BackgroundColor = new Color(255, 255, 255);

            Inputbox username = new Inputbox("Username");
            username.Text = "Morten R";
            username.X = 75;
            username.Y = 100;
            username.Width = 450;
            username.Height = 36;
            username.Align = TextAlignment.Left;

            Inputbox password = new Inputbox("Password", true);
            password.Text = "morten";
            password.X = 75;
            password.Y = 160;
            password.Width = 450;
            password.Height = 36;
            password.Align = TextAlignment.Left;

            ButtonView submit = new ButtonView("Log ind", () => {
                if (OnLogin != null)
                    OnLogin(username.Text, password.Text);
            });
            submit.X = 220;
            submit.Y = 250;
            submit.Width = 160;
            submit.Height = 60;
            submit.BackgroundColor = new Color(40, 175, 100);
            submit.TextColor = new Color(255, 255, 255);

            Status = new LabelView(" ");
            Status.X = 75;
            Status.Y = 200;
            Status.Width = 200;
            Status.Height = 30;
            Status.TextColor = new Color(200, 0, 0);

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
