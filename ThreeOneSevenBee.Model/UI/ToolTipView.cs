﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Euclidean;

namespace ThreeOneSevenBee.Model.UI
{
    public class ToolTipView : LabelView
    {
        public Color BoxColor { get; set; }

        public ToolTipView(string text) : base(" " + text + " ")
        {
            BoxColor = new Color(40, 130, 120);
        }

        public override void DrawWithContext(IContext context, double offsetX, double offsetY)
        {
            if (Visible == true)
            {
                if (Position == position.upperLeft)
                {

                    context.DrawPolygon(
                        new Vector2[]{
                    new Vector2(X + offsetX, Y + offsetY + 10),
                    new Vector2(X + offsetX + 10, Y + offsetY),
                    new Vector2(X + offsetX + 20, Y + offsetY + 10),
                    new Vector2(X + offsetX + Width, Y + offsetY + 10),
                    new Vector2(X + offsetY + Width, Y + offsetY + Height),
                    new Vector2(X + offsetX, Y + offsetX + Height)
                        },
                        BoxColor
                    );
                    context.DrawText(X + offsetX, Y + offsetY + 10, Width, Height - 10, Text, TextColor);
                }
                if (Position == position.upperRight)
                {
                    context.DrawPolygon(
                    new Vector2[]{
                    new Vector2(X + offsetX, Y+ offsetY + 10),
                    new Vector2(X + offsetX + 10, Y + offsetY),
                    new Vector2(X + offsetX + 20, Y + offsetY + 10),
                    new Vector2(X + offsetX + 20, Y + offsetY + Height),
                    new Vector2(X + offsetY - Width, Y + offsetY + Height),
                    new Vector2(X + offsetX - Width, Y + offsetY + 10)
                        },
                        BoxColor
                    );
                    context.DrawText(X + offsetX - Width, Y + offsetY + 10, Width, Height - 10, Text, TextColor);
                }
                if (Position == position.bottomMiddle)
                {
                    context.DrawPolygon(
                    new Vector2[]{
                    new Vector2(X + offsetX, Y+ offsetY + 10),
                    new Vector2(X + offsetX + Width / 2 - 10, Y + offsetY + 10),
                    new Vector2(X + offsetX + Width / 2, Y + offsetY),
                    new Vector2(X + offsetX + Width / 2 + 10, Y + offsetY + 10),
                    new Vector2(X + offsetY + Width, Y + offsetY + 10),
                    new Vector2(X + offsetX + Width, Y + offsetY),
                    new Vector2(X + offsetX + Width, Y + offsetY + Height),
                    new Vector2(X + offsetX, Y + offsetY + Height)
                        },
                        BoxColor
                    );
                    context.DrawText(X + offsetX, Y + offsetY - Height / 4 + 10, Width, Height - 10, Text, TextColor);
                    context.DrawText(X + offsetX, Y + offsetY - Height / 4 + 40, Width, Height - 10, Description, TextColor);
                }
            }
        }

        public position Position { get; set; }
        public string Description { get; set; }

    }
    public enum position { upperLeft, upperRight, bottomMiddle }
}
