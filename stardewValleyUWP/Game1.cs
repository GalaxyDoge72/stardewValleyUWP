using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using stardewValleyUWP.Handlers;
using stardewValleyUWP.Screens;
using stardewValleyUWP.Utilities;
using Windows.Graphics.Display;
using Windows.System.Profile;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;

namespace stardewValleyUWP
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private ScreenManager _screenManager;

        private KeyboardState prevKeyState;
        private bool isWindowsPhone = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            isWindowsPhone = AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";

            if (isWindowsPhone)
            {
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
            }
        }

        protected override void Initialize()
        {
            _screenManager = new ScreenManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            GameServices.Content = Content;
            GameServices.SpriteBatch = new SpriteBatch(GraphicsDevice);

            _screenManager.AddScreen("MainMenu", new titleScreen(_screenManager));
            _screenManager.AddScreen("LoadScreen", new LoadScreen(_screenManager));
            _screenManager.SwitchScreen("MainMenu");
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
