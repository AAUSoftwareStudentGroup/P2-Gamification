using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Euclidean;
#if BRIDGE
using Bridge.Html5;
#endif

namespace ThreeOneSevenBee.Model.UI
{
    public class Inputbox : LabelView
    {
        private string placeholder;
		private int cursorPos;
		private bool hidden;
		private Color placeholderColor;

		public Inputbox(string placeholder, bool hidden, Color placeholderColor) : base(placeholder)
		{
			this.placeholderColor = placeholderColor;
			this.placeholder = placeholder;
			this.hidden = hidden;
			this.BackgroundColor = new Color(200, 200, 200);
			cursorPos = 0;
		}

		public Inputbox(string placeholder, bool hidden) : this(placeholder, hidden, new Color(100,100,100)) {}
		public Inputbox (string placeholder) : this (placeholder, false) {}

		public override void Click (double x, double y, IContext context)
		{
            if (ContainsPoint(x, y))
            {
                if (OnClick != null)
                {
                    OnClick();
                }
                Active = true;
            }
            else
            {
                Active = false;
            }
            if (Active == false && Text.Length == 0)
				Text = placeholder;
			if (ContainsPoint (x, y)) {
				if (Active == true && string.Compare (Text, placeholder) == 0)
					Text = "";
				for(int i = 0; i <= Text.Length; i++) {
					if(context.GetTextDimensions (hidden? GetHiddenText().Substring(0, i) : Text.Substring(0, i), Width, Height).X > x - this.X) {
						cursorPos = i-1;
						break;
					}
					if (i == Text.Length)
						cursorPos = i;
				}
			}
		}

		public bool IsDefault() {
			return string.Compare (Text, placeholder) == 0;
		}

		public override void KeyPressed(string key, IContext context)
        {
			base.KeyPressed(key, context);
			if(Active) {
				switch (key) {
				case "Back":
					if(cursorPos > 0)
						Text = Text.Remove(--cursorPos, 1);
					break;
				case "Right":
					if (cursorPos < Text.Length)
						cursorPos++;
					break;
				case "Space":
					Text = Text.Insert (cursorPos, " ");
					cursorPos++;
					break;
				case "Left":
					if (cursorPos > 0)
						cursorPos--;
					break;
				default:
					if (key.Length == 1) {
						Text = Text.Insert (cursorPos, key);
						cursorPos++;
						if (context.GetTextDimensions (hidden ? GetHiddenText () : Text, Width, Height).X > Width - 5) {
							KeyPressed ("Back", context);
						}
					}
					break;
				}
			}
        }

		protected string GetHiddenText() {
			string hiddenText = "";
			for (int i = 0; i < Text.Length; i++) {
				hiddenText += "*";
			}
			return hiddenText;
		}

		public override void DrawWithContext(IContext context, double offsetX, double offsetY)
		{
			Color backupColor = TextColor;
			if (IsDefault ())
				TextColor = placeholderColor;
			
			if (!hidden)
				base.DrawWithContext (context, offsetX, offsetY);
			else {
				string TextBackup = Text;

				if (Text == placeholder)
					Text = placeholder;
				else
					Text = GetHiddenText ();
				
				base.DrawWithContext (context, offsetX, offsetY);
				Text = TextBackup;
			}
			if (Active) {
				double textWidth = 0;
				if (cursorPos > 0 && Text.Length != 0) {
					textWidth = context.GetTextDimensions (hidden? GetHiddenText().Substring(0, cursorPos) : Text.Substring (0, cursorPos), Width, Height).X;
				}
				context.DrawLine (new Vector2 (textWidth + offsetX + X + 1, Y + offsetY), new Vector2 (textWidth + offsetX + X + 1, offsetY + Y + Height), new Color (0, 0, 0), 1);
			}
			if (IsDefault ())
				TextColor = backupColor;
		}
    }
}
