using System.Linq;
using System.Windows.Forms;
using FarseerPhysics;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace ggj2015
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager _graphics;
		SpriteBatch _spriteBatch;
		private DebugViewXNA _debugView;

		public Game1()
			: base()
		{
			_graphics = new GraphicsDeviceManager(this);
			Globals.Input = new InputManager(Services, Window.Handle);
			Components.Add(Globals.Input);

			Content.RootDirectory = "Content";

			_graphics.PreferredBackBufferWidth = Globals.RenderWidth;
			_graphics.PreferredBackBufferHeight = Globals.RenderHeight;
			_graphics.IsFullScreen = true;

			IsMouseVisible = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();


#if truet
			var form = (Form)Control.FromHandle(Window.Handle);
			form.FormBorderStyle = FormBorderStyle.None;
			_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
			_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
			_graphics.ApplyChanges();

			form.Left = 0;
			form.Top = 0;
			form.Focus();
#endif

			// TODO: Add your initialization logic here

			Globals.World = new World(new Vector2(0, -9.8f));
			_debugView = new DebugViewXNA(Globals.World);
			_debugView.LoadContent(GraphicsDevice, Content);
			_debugView.Flags = DebugViewFlags.Shape;//(DebugViewFlags)0xff;

			BodyFactory.CreateCircle(Globals.World, 4, 1, new Vector2(0, 10), BodyType.Dynamic);
			//BodyFactory.CreateCircle(Globals.World, 20, 1, new Vector2(10, 50), BodyType.Dynamic);
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
			Resources.Load(Content);
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			//Looks like keyboards[4] is the one (windowmessage keyboard)
			var states = Globals.Input.Keyboards.Select(x => x.GetState()).ToArray();
			if (Globals.Input.GamePads[0].GetState().Buttons.Back == ButtonState.Pressed || states.Any(s => s.IsKeyDown(Keys.Escape)))
				Exit();

			// TODO: Add your update logic here
			Globals.World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);


		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here

			//var projection = Matrix.CreateOrthographicOffCenter(0, ConvertUnits.ToSimUnits(GraphicsDevice.PresentationParameters.BackBufferWidth), 0, ConvertUnits.ToSimUnits(GraphicsDevice.PresentationParameters.BackBufferHeight), -1, 1);
			var projection = Matrix.CreateOrthographicOffCenter(0, ConvertUnits.ToSimUnits(Globals.RenderWidth), 0, ConvertUnits.ToSimUnits(Globals.RenderHeight), -1, 1);
			_debugView.RenderDebugData(projection, Matrix.Identity);

			_spriteBatch.Begin(transformMatrix: Matrix.CreateScale((float)GraphicsDevice.PresentationParameters.BackBufferWidth / Globals.RenderWidth, (float)GraphicsDevice.PresentationParameters.BackBufferHeight / Globals.RenderHeight, 1));

			_spriteBatch.Draw(Resources.Test, new Vector2(100, 100));

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
