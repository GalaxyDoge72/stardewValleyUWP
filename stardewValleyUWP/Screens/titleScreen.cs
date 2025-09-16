using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using stardewValleyUWP.Handlers;
using stardewValleyUWP.Objects;
using stardewValleyUWP.Utilities;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;

namespace stardewValleyUWP.Screens
{
    public class titleScreen : Screen
    {
        private UIManager uiManager;
        private Grid grid;
        private SpriteFont buttonFont;
        private ScreenManager _screenManager;

        private int lastWidth;
        private int lastHeight;

        public titleScreen(ScreenManager manager)
        {
            _screenManager = manager;

            // Subscribe to UWP keyboard events
            var coreWindow = CoreApplication.GetCurrentView().CoreWindow;
            coreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.G)
                grid.Visible = !grid.Visible;
        }

        public override void LoadContent()
        {
            buttonFont = GameServices.Content.Load<SpriteFont>("Fonts/boldPixels");
            var viewport = GameServices.SpriteBatch.GraphicsDevice.Viewport;

            grid = new Grid(5, 5, viewport.Width, viewport.Height,
                            GameServices.SpriteBatch.GraphicsDevice);
            lastWidth = viewport.Width;
            lastHeight = viewport.Height;

            uiManager = new UIManager(grid);

            var startButton = new Button(Rectangle.Empty, "Begin", buttonFont)
            {
                BGColor = Color.DarkBlue,
                HoverColor = Color.Blue,
                onClick = () => _screenManager.SwitchScreen("Gameplay")
            };

            uiManager.AddElementAsync(startButton, 1, 1);
        }

        public override void Update(GameTime gameTime)
        {
            var viewport = GameServices.SpriteBatch.GraphicsDevice.Viewport;

            // Detect resize
            if (viewport.Width != lastWidth || viewport.Height != lastHeight)
            {
                lastWidth = viewport.Width;
                lastHeight = viewport.Height;

                grid.Width = viewport.Width;
                grid.Height = viewport.Height;

                uiManager.RecalculatePositions();
            }

            uiManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = GameServices.SpriteBatch;
            sb.Begin();
            uiManager.Draw(sb);
            grid.Draw(sb, Color.Red * 0.5f);
            sb.End();
        }
    }
}
