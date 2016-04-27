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
	public class DesktopContext : Context
	{
		double width, height;

		SpriteBatch spriteBatch;
		SpriteFont font;
		public XNA.GraphicsDeviceManager graphicsDeviceManager;

		public DesktopContext (XNA.GraphicsDeviceManager gdm, SpriteBatch sb, SpriteFont vf, double width, double height) : base(width, height)
		{
			this.width = width;
			this.height = height;
			spriteBatch = sb;
			font = vf;
			graphicsDeviceManager = gdm;
		}

		public override void Clear()
		{
			graphicsDeviceManager.GraphicsDevice.Clear(XNA.Color.White);
		}

		public override void DrawPNGImage(string fileName, double x, double y, double width, double height) {
		}

		public override void DrawLine(Vector2 first, Vector2 second, Color lineColor, double lineWidth)
		{
			spriteBatch.Begin ();
			spriteBatch.DrawLine ((int)first.X, (int)first.Y, (int)second.X, (int)second.Y, ConvertColor (lineColor), (float)lineWidth);
			spriteBatch.End();
		}

		public override void DrawRectangle(double x, double y, double width, double height, Color fillColor, Color lineColor, double lineWidth)
		{
			spriteBatch.Begin ();
			spriteBatch.FillRectangle (new XNA.Rectangle ((int)x, (int)y, (int)width, (int)height), ConvertColor (fillColor));
			spriteBatch.DrawRectangle (new XNA.Rectangle ((int)x, (int)y, (int)width, (int)height), ConvertColor (lineColor), (float)lineWidth);
			spriteBatch.End ();
		}

		public override void DrawPolygon(Vector2[] path, Color fillColor, Color lineColor, double lineWidth)
		{
			VertexPositionColor[] _vertices = new VertexPositionColor[path.Length];
			int[] indicies = new int[_vertices.Length];

			for (int i = 0; i < path.Length; i++) {
				_vertices [i] = new VertexPositionColor (new XNA.Vector3 ((float)path [i].X, (float)path [i].Y, 0), ConvertColor (fillColor));
				indicies [i] = i;
			}

			graphicsDeviceManager.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>
			(
				PrimitiveType.TriangleStrip, 
				_vertices, 0, _vertices.Length,
				indicies,  0, _vertices.Length-2
			);

			for (int i = 0; i < path.Length-1; i++) {
				this.DrawLine (path [i], path [i + 1], fillColor, lineWidth);
			}
		}
		public override void DrawText(double x, double y, double width, double height, string text, Color textColor)
		{
			XNA.Vector2 stringSize = font.MeasureString (text);
			float widthScale = (float)width / stringSize.X;
			float heightScale = (float)height / stringSize.Y;
			float scale = Math.Min (widthScale, heightScale);
			spriteBatch.Begin ();
			spriteBatch.DrawString (font, text, new XNA.Vector2 ((float)x, (float)y), ConvertColor (textColor), 0, new XNA.Vector2 (0, 0), scale, SpriteEffects.None, 0.5f); 
			spriteBatch.End ();
		}

		public override Vector2 GetTextDimensions(string text, double maxWidth, double maxHeight) {
			XNA.Vector2 stringSize = font.MeasureString (text);
			double widthScale = maxWidth / stringSize.X;
			double heightScale = maxHeight / stringSize.Y;
			double scale = Math.Min (widthScale, heightScale);

			return new Vector2 (stringSize.X * scale, stringSize.Y * scale);
		}

		private XNA.Color ConvertColor(Color c)
		{
			return new XNA.Color ((int)c.Red, (int)c.Green, (int)c.Blue, (int)(c.Alpha * 255)); 
		}
	}
}
