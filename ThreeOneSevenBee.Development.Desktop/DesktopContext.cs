using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ThreeOneSevenBee.Model.Euclidean;
using ThreeOneSevenBee.Model.UI;

using C3.XNA;
using XNA = Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

using Nuclex.Graphics;
using Nuclex.Fonts;

namespace ThreeOneSevenBee.Development.Desktop
{
	public class DesktopContext : IContext
	{
		public View contentView;
		double width, height;

		SpriteBatch spriteBatch;
		SpriteFont font;

		public DesktopContext (SpriteBatch sb, SpriteFont vf, double width, double height)
		{
			contentView = null;
			this.width = width;
			this.height = height;
			spriteBatch = sb;
			font = vf;
		}

		public double Width { get { return width; } }
		public double Height { get { return height; } }

		public void Clear()
		{
			
		}

		public void SetContentView(View view)
		{
			contentView = view;	
		}
		public void Draw()
		{
			spriteBatch.Begin ();
//			textBatch.Begin ();

			if (contentView != null)
				contentView.DrawWithContext (this, 0, 0);

//			textBatch.End ();
			spriteBatch.End();
		}


		public void DrawPNGImage(string fileName, double x, double y, double width, double height) {
		}

		public void DrawLine(Vector2 first, Vector2 second, Color lineColor, double lineWidth)
		{
			spriteBatch.DrawLine ((int)first.X, (int)first.Y, (int)second.X, (int)second.Y, ConvertColor (lineColor), (float)lineWidth);

		}
		public void DrawRectangle(double x, double y, double width, double height, Color fillColor)
		{
			this.DrawRectangle (x, y, width, height, fillColor, fillColor, 0);
		}
		public void DrawRectangle(double x, double y, double width, double height, Color fillColor, Color lineColor, double lineWidth)
		{
			spriteBatch.FillRectangle (new XNA.Rectangle ((int)x, (int)y, (int)width, (int)height), ConvertColor (fillColor));
			spriteBatch.DrawRectangle (new XNA.Rectangle ((int)x, (int)y, (int)width, (int)height), ConvertColor (lineColor), (float)lineWidth);
		}
		public void DrawPolygon(Vector2[] path, Color fillColor)
		{
			this.DrawPolygon (path, fillColor, fillColor, 1);
		}
		public void DrawPolygon(Vector2[] path, Color fillColor, Color lineColor, double lineWidth)
		{
			for (int i = 0; i < path.Length-1; i++) {
				this.DrawLine (path [i], path [i + 1], fillColor, lineWidth);
			}
		}
		public void DrawText(double x, double y, double width, double height, string text, Color textColor)
		{
//			textBatch.ViewProjection;
//			Text t = font.Fill(text);
//			this.textBatch.DrawText (t, XNA.Color.White);

			XNA.Vector2 stringSize = font.MeasureString (text);
			float widthScale = (float)width / stringSize.X;
			float heightScale = (float)height / stringSize.Y;
			float scale = Math.Min (widthScale, heightScale);
			spriteBatch.DrawString (font, text, new XNA.Vector2 ((float)x, (float)y), ConvertColor (textColor), 0, new XNA.Vector2 (0, 0), scale, SpriteEffects.None, 0.5f); 
		}

		private XNA.Color ConvertColor(Color c)
		{
			return new XNA.Color ((int)c.Red, (int)c.Green, (int)c.Blue, (int)(c.Alpha * 255)); 
		}
	}
}

