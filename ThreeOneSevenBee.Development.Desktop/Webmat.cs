using System;
using System.IO;
using System.Linq;
using ThreeOneSevenBee.Model.UI;
using TOSBGame = ThreeOneSevenBee.Model.Game;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

using Nuclex.Fonts;
using Nuclex.Graphics;

namespace ThreeOneSevenBee.Development.Desktop
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Webmat : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		TextBatch textBatch;
		DesktopContext context;
		SpriteFont font;
		VectorFont _font;
		MouseState oldMouseState, currentMouseState;
		Keys[] lastKeys;

		public Webmat ()
		{
			this.IsMouseVisible = true;
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
		}

		

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			graphics.PreferredBackBufferWidth = 1360;  // set this value to the desired width of your window
			graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
			graphics.ApplyChanges();

			base.Initialize ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);
//			textBatch = new TextBatch (GraphicsDevice);
			font = Content.Load<SpriteFont>("Georgia");

			context = new DesktopContext(graphics, spriteBatch, font, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

			Stream stream;
			stream = new StreamReader (Path.GetFullPath (@"../../Content/img/star.png")).BaseStream;
			context.imageCache.Add("star.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../Content/img/masterofalgebra.png")).BaseStream;
			context.imageCache.Add("masterofalgebra.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../Content/img/potensv2.png")).BaseStream;
			context.imageCache.Add("potensv2.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../Content/img/brøkbadge.png")).BaseStream;
			context.imageCache.Add("brøkbadge.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../Content/img/restart.png")).BaseStream;
			context.imageCache.Add("restart.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../Content/img/star_activated.png")).BaseStream;
			context.imageCache.Add("star_activated.png", Texture2D.FromStream (GraphicsDevice, stream));

			TOSBGame.IGameAPI gameAPI = new DesktopGameAPI();

			TOSBGame.Game game = new TOSBGame.Game (context, gameAPI);
			game.Start ();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__ &&  !__TVOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape))
				Exit ();
			#endif

			oldMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();
			if (currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) {
				context.contentView.Click (currentMouseState.Position.X, currentMouseState.Position.Y, context);
			}
			KeyboardState state = Keyboard.GetState();
			foreach(Keys key in state.GetPressedKeys () ) {
				bool brk = false;
				bool upperCase = false;
				if(lastKeys != null) { // Some logic to avoid sending a keypress every frame
					foreach (Keys lastKey in lastKeys) {
						if (key.ToString () == lastKey.ToString () )
							brk = true;
						if (lastKey.ToString () == "RightShift" || lastKey.ToString () == "LeftShift")
							upperCase = true;
					}
				}
				if(!brk) {
					string keys;
					// Add special keys here
					if (key.ToString ().Length == 1) {
						if (upperCase)
							keys = key.ToString ().ToUpper ();
						else
							keys = key.ToString ().ToLower ();
					} else
						keys = key.ToString ();
					context.contentView.KeyPressed (keys, context);

				}
			}
			lastKeys = state.GetPressedKeys ();
		
			// TODO: Add your update logic here
            
			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{

			context.graphicsDeviceManager = graphics;
			context.Draw ();
			//TODO: Add your drawing code here


			base.Draw (gameTime);
		}
	}
}

