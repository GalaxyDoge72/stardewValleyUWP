using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using stardewValleyUWP.Objects;
using stardewValleyUWP.Handlers;
using System;
using stardewValleyUWP.Utilities;
using Windows.Media.SpeechRecognition;

namespace stardewValleyUWP.Screens
{
    public class LoadScreen : Screen
    {
        private UIManager uiManager;
        private Grid grid;
        private SpriteFont font;
        private ScreenManager screenManager;

        private int lastWidth;
        private int lastHeight;

        private SaveListUI saveListUI;

        public LoadScreen(ScreenManager manager)
        {
            screenManager = manager;
        }

        public override void LoadContent()
        {
            font = GameServices.Content.Load<SpriteFont>("Fonts/MediumBoldPixels");

            var viewPort = GameServices.SpriteBatch.GraphicsDevice.Viewport;
            grid = new Grid(5,5, viewPort.Width, viewPort.Height, GameServices.SpriteBatch.GraphicsDevice);

            lastWidth = viewPort.Width;
            lastHeight = viewPort.Height;

            uiManager = new UIManager(grid);

            Rectangle saveListBounds = grid.GetCellScaled(1, 0, 3, 5);
            saveListUI = new SaveListUI(font, saveListBounds);

            uiManager.AddElementAsync(saveListUI, 1, 0, 3f, 5f);
        }

        public override void Update(GameTime gameTime)
        {
            var viewPort = GameServices.SpriteBatch.GraphicsDevice.Viewport;
            if (viewPort.Width != lastWidth || viewPort.Height != lastHeight)
            {
                lastWidth = viewPort.Width;
                lastHeight = viewPort.Height;

                grid.Width = viewPort.Width;
                grid.Height = viewPort.Height;

                uiManager.RecalculatePositions();
            }
            uiManager.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var sb = GameServices.SpriteBatch;
            sb.Begin();
            uiManager.Draw(sb);
            sb.End();
        }
    }
}