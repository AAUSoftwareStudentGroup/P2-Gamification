using System;
using ThreeOneSevenBee.Model.UI;
using ThreeOneSevenBee.Model.Game;

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

			context = new DesktopContext(spriteBatch, font, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

			IGameAPI gameAPI = new DesktopGameAPI();

			GameModel gameModel;

			gameAPI.GetCurrentPlayer((u) =>
				{
					gameAPI.GetPlayers((p) =>
						{
							gameModel = new GameModel(u, p)
							{
								OnSaveLevel = (level) =>
									gameAPI.SaveUserLevelProgress
									(
										level.LevelID,
										level.CurrentExpression,
										level.Stars,
										(success) => Console.WriteLine(success)
									)
							};

							new GameView(gameModel, context);
						});
				});
			//TODO: use this.Content to load your game content here 
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
				context.contentView.Click (currentMouseState.Position.X, currentMouseState.Position.Y);
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
					context.contentView.KeyPressed (keys);

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
			graphics.GraphicsDevice.Clear (Microsoft.Xna.Framework.Color.CornflowerBlue);
			context.Draw ();

			//TODO: Add your drawing code here
            
			base.Draw (gameTime);
		}
	}
}

