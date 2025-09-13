using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using stardewValleyUWP.Objects;
using stardewValleyUWP.Handlers;

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
            titleFont = GameServices.Content.Load<SpriteFont>("Fonts/DefaultFont");
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {

        }


    }
}
