using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Euclidean;

namespace ThreeOneSevenBee.Model.UI
{
    public class ToolTipView : CompositeView
    {
        public Color BoxColor
        {
            get
            {
                return labelView.BackgroundColor;
            }

            set
            {
                labelView.BackgroundColor = value;
                arrow.BackgroundColor = value;
            }
        }
        public Color TextColor
        {
            get
            {
                return labelView.TextColor;
            }

            set
            {
                labelView.TextColor = value;
            }
        }

        public string Text
        {
            get
            {
                return labelView.Text;
            }
            set
            {
                labelView.Text = value;
            }
        }

        double arrowPosition;
        public double ArrowPosition
        {
            get
            {
                return arrowPosition;
            }

            set
            {
                arrowPosition = value;
                switch (ArrowDirection)
                {
                    case Direction.Top:
                        arrow.X = arrowPosition;
                        break;
                    case Direction.right:
                        arrow.Y = arrowPosition;
                        break;
                    case Direction.Left:
                        arrow.Y = arrowPosition;
                        break;
                    case Direction.Bottom:
                        arrow.X = arrowPosition;
                        break;
                    default:
                        break;
                }
            }
        }

        LabelView labelView { get; set; }
        VectorImageView arrow { get; set; }


        public ToolTipView(string text, double width, double height) : base(width, height)
        {
            arrow = new VectorImageView(0, 0, 20, 11)
            {
                { 0, 11 },
                { 10, 0 },
                { 20, 11 }
            };
            labelView = new LabelView(text)
            {
                Y = 10,
                Height = height - 10,
                Width = width
            };
            Text = text;
            BoxColor = new Color(40, 130, 120);
            TextColor = new Color(255, 255, 255);
            ArrowDirection = Direction.Top;
            ArrowPosition = 0;
            Children.Add(arrow);
            Children.Add(labelView);
        }

        Direction arrowDirection;
        public Direction ArrowDirection
        {
            get
            {
                return arrowDirection;
            }
            set
            {
                arrowDirection = value;
                arrowPosition = 0;
                labelView.X = 0;
                labelView.Y = 0;
                labelView.Height = Height;
                labelView.Width = Width;
                switch (arrowDirection)
                {
                    case Direction.Top:
                        arrow = new VectorImageView(0, 0, 20, 10)
                        {
                            { 0, 11 },
                            { 10, 0 },
                            { 20, 11 }
                        };
                        
                        labelView.Y = 10;
                        labelView.Height = Height - 10;
                        break;
                    case Direction.right:
                        arrow = new VectorImageView(Width - 10, 0, 10, 20)
                        {
                            { -1, 0 },
                            { 10, 10 },
                            { -1, 20 }
                        };
                        labelView.Width = Width - 10;
                        break;
                    case Direction.Left:
                        arrow = new VectorImageView(0, 0, 10, 20)
                        {
                            { 11, 0 },
                            { 0, 10  },
                            { 11, 20 }
                        };
                        labelView.X = 10;
                        labelView.Width = Width - 10;
                        break;
                    case Direction.Bottom:
                        arrow = new VectorImageView(0, Height - 10, 20, 10)
                        {
                            { 0, -1 },
                            { 20, -1 },
                            { 10, 10 }
                        };
                        labelView.Height = Height - 10;
                        break;
                    default:
                        break;
                }
                arrow.BackgroundColor = BoxColor;
            }
        }

    }
    public enum Direction { Top, right, Left, Bottom }
}
