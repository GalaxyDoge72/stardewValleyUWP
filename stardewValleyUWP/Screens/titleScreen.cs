using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using stardewValleyUWP.Objects;
using stardewValleyUWP.Utilities;
using stardewValleyUWP.Handlers;
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
        private SpriteFont titleFont;
        private ScreenManager _screenManager;

        private int lastWidth;
        private int lastHeight;

        public titleScreen(ScreenManager manager)
        {
            _screenManager = manager;
            var coreWindow = CoreApplication.GetCurrentView().CoreWindow;
            coreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (grid == null) return; // safe check
            if (args.VirtualKey == VirtualKey.G)
                grid.Visible = !grid.Visible;
        }

        public override void LoadContent()
        {
            buttonFont = GameServices.Content.Load<SpriteFont>("Fonts/MediumBoldPixels");
            titleFont = GameServices.Content.Load<SpriteFont>("Fonts/LargeBoldPixels");

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
            uiManager.AddElementAsync(startButton, 4, 1); // row 4, col 1

            var titleLabel = new Label("Stardew Valley UWP", titleFont);
            uiManager.AddElementAsync(titleLabel, 0, 2); // row 0, col 2
        }

        public override void Update(GameTime gameTime)
        {
            var viewport = GameServices.SpriteBatch.GraphicsDevice.Viewport;
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
