using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using stardewValleyUWP.Objects;
using stardewValleyUWP.Handlers;
using stardewValleyUWP.Utilities;

namespace stardewValleyUWP.Screens
{
    public class titleScreen : Screen
    {
        private readonly ScreenManager _screenManager;

        private Button startButton;
        private SpriteFont buttonFont;

        private SpriteFont titleFont;

        public titleScreen(ScreenManager manager)
        {
            _screenManager = manager;
        }

        public override void LoadContent()
        {
            buttonFont = GameServices.Content.Load<SpriteFont>("Fonts/boldPixels");

            startButton = new Button(new Rectangle(300, 200, 200, 60), "Begin", buttonFont)
            {
                BGColor = Color.DarkBlue,
                HoverColor = Color.Blue,
                onClick = () => _screenManager.SwitchScreen("Gameplay")
            };
        }

        public override void Update(GameTime gameTime)
        {
            startButton.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = GameServices.SpriteBatch;
            spriteBatch.Begin();
            startButton.Draw(spriteBatch);
            spriteBatch.End();
        }


    }
}
