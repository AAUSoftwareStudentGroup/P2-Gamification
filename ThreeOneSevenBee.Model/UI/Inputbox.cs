using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Euclidean;

namespace ThreeOneSevenBee.Model.UI
{
    public class Inputbox : LabelView
    {
        private string emptyString;
		int cursorPos;
        public Inputbox(string emptyString) : base(emptyString)
        {
            this.emptyString = emptyString;
            BackgroundColor = new Color(0, 255, 100);
			cursorPos = 0;
        }

		public override void Click (double x, double y)
		{
			base.Click (x, y);
			if (ContainsPoint (x, y)) {
				double textWidth;
				for(int i = 0; i < Text.Length; i++) {
					
				}
			}
		}

        public override void KeyPressed(string key)
        {
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
					}
					break;
				}
			}
        }

		public override void DrawWithContext(IContext context, double offsetX, double offsetY)
		{
			base.DrawWithContext(context, offsetX, offsetY);
			//context.DrawText(X + offsetX, Y + offsetY, Width, Height, Text, TextColor);
			if (Active) {
				double textWidth = 0;
				if (cursorPos > 0)
					textWidth = context.GetTextDimensions (Text.Substring (0, cursorPos), Width, Height).X;
				context.DrawLine (new Vector2 (textWidth + offsetX + X + 1, Y + offsetY), new Vector2 (textWidth + offsetX + X + 1, offsetY + Y + Height), new Color (0, 0, 0), 1);
			}
		}
    }
}
