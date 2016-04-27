using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeOneSevenBee.Model.UI
{
    public class LoginView : CompositeView
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
			username.X = 300;
			username.Y = 100;
			username.Width = 300;
			username.Height = 24;

			Inputbox password = new Inputbox("Password", true);
			password.X = 300;
			password.Y = 150;
			password.Width = 300;
			password.Height = 24;

			ButtonView submit = new ButtonView ("Log in", () => {
				if(OnLogin != null)
					OnLogin(username.Text, password.Text);
			});
			submit.X = 300;
			submit.Y = 200;
			submit.Width = 80;
			submit.Height = 30;
			submit.BackgroundColor = new Color (200, 200, 200);

			Status = new LabelView (" ");
			Status.X = 300;
			Status.Y = 250;
			Status.Width = 200;
			Status.Height = 30;

			Children = new List<View>() {
                username,
				password,
				submit,
				Status
            };
        }
    }
}
