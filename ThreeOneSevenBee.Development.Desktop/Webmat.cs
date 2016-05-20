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
		DesktopContext context;
		SpriteFont font;
		MouseState oldMouseState, currentMouseState;
		Keys[] lastKeys;
		MouseDevice mouseDevice;

		public Webmat ()
		{
			this.IsMouseVisible = true;
			this.Window.AllowUserResizing = true;

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
			//graphics.PreferredBackBufferWidth = 1360;  // set this value to the desired width of your window
			//graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
			//graphics.ApplyChanges();

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
			font = Content.Load<SpriteFont>("Arial");

			context = new DesktopContext(graphics, spriteBatch, font, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
			this.Window.ClientSizeChanged += (sender, e) => {
				context.ContentView.Width = context.Width = Window.ClientBounds.Width;
				context.ContentView.Height = context.Height = Window.ClientBounds.Height;

			};


			Stream stream;
			stream = new StreamReader (Path.GetFullPath (@"../../../ThreeOneSevenBee.Frontend.Website/img/star.png")).BaseStream;
			context.imageCache.Add("star.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../../ThreeOneSevenBee.Frontend.Website/img/master_of_algebrabadge.png")).BaseStream;
			context.imageCache.Add("master_of_algebrabadge.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../../ThreeOneSevenBee.Frontend.Website/img/potens_badge.png")).BaseStream;
			context.imageCache.Add("potens_badge.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../../ThreeOneSevenBee.Frontend.Website/img/brøkbadge.png")).BaseStream;
			context.imageCache.Add("brøkbadge.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../../ThreeOneSevenBee.Frontend.Website/img/restart.png")).BaseStream;
			context.imageCache.Add("restart.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../../ThreeOneSevenBee.Frontend.Website/img/star_activated.png")).BaseStream;
			context.imageCache.Add("star_activated.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../../ThreeOneSevenBee.Frontend.Website/img/parenthesis_badge.png")).BaseStream;
			context.imageCache.Add("parenthesis_badge.png", Texture2D.FromStream (GraphicsDevice, stream));
			stream = new StreamReader (Path.GetFullPath (@"../../../ThreeOneSevenBee.Frontend.Website/img/tutorial_badge.png")).BaseStream;
			context.imageCache.Add("tutorial_badge.png", Texture2D.FromStream (GraphicsDevice, stream));

			TOSBGame.IGameAPI gameAPI = new DesktopGameAPI();

			TOSBGame.Game game = new TOSBGame.Game (context, gameAPI);
			game.Start ();
			mouseDevice = new MouseDevice (GraphicsDevice);
			mouseDevice.ButtonReleased += (button, state) => {
				if (button == MouseButtons.Left)
					context.ContentView.Click (state.X, state.Y, context);
			};
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
			mouseDevice.Update ();

			/*
			oldMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();
			if (currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) {
				context.ContentView.Click (currentMouseState.X, currentMouseState.Y, context);
			}
			*/
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
					string keys = null;
					int a;
					if (keys == null && key.ToString ().StartsWith ("D")) {
						try {
							if (Int32.TryParse (key.ToString ().Substring (1), out a)) {
								keys = key.ToString ().Substring (1);
							}
						} catch (FormatException) {
							// Nope.
						}
					}
					if(keys == null && key.ToString().StartsWith("NumPad")) {
						try {
							if (Int32.TryParse (key.ToString ().Substring (6), out a))
								keys = key.ToString ().Substring (6);
						} catch (FormatException) {
							// Also nope.
						}
					}
					// Add special keys here
					if (keys == null && key.ToString ().Length == 1) {
						if (upperCase)
							keys = key.ToString ().ToUpper ();
						else
							keys = key.ToString ().ToLower ();
					} else if(keys == null)
						keys = key.ToString ();
					
					context.ContentView.KeyPressed (keys, context);
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

