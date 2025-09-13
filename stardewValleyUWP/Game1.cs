using Microsoft.Xna.Framework;
using stardewValleyUWP.Handlers;
using stardewValleyUWP.Screens;

namespace stardewValleyUWP
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private ScreenManager _screenManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _screenManager = new ScreenManager();

            // Register screens
            _screenManager.AddScreen("MainMenu", new titleScreen(_screenManager));

            // Start at MainMenu
            _screenManager.SwitchScreen("MainMenu");

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _screenManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _screenManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
