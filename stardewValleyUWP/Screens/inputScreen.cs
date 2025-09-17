using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using stardewValleyUWP.Objects;
using stardewValleyUWP.Utilities;
using System;
using System.Text;

namespace stardewValleyUWP.Screens
{
    public class InputScreen : Screen
    {
        private Action<string> onInputComplete;
        private string prompt;
        private StringBuilder currentText;
        private SpriteFont font;
        private KeyboardState lastKeyboardState;

        public InputScreen(string prompt, Action<string> onInputComplete)
        {
            this.prompt = prompt;
            this.onInputComplete = onInputComplete;
            this.currentText = new StringBuilder();
        }

        public override void LoadContent()
        {
            font = GameServices.Content.Load<SpriteFont>("Fonts/MediumBoldPixels");
            lastKeyboardState = Keyboard.GetState();
        }

        public override void UnloadContent()
        {
            // Nothing to unload
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = keyboardState.GetPressedKeys();
            Keys[] lastPressedKeys = lastKeyboardState.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if (!lastKeyboardState.IsKeyDown(key))
                {
                    if (key == Keys.Back && currentText.Length > 0)
                    {
                        currentText.Remove(currentText.Length - 1, 1);
                    }
                    else if (key == Keys.Enter)
                    {
                        onInputComplete?.Invoke(currentText.ToString());
                        GameServices.ScreenManager.PopScreen();
                    }
                    else if (key.ToString().Length == 1 && char.IsLetterOrDigit(key.ToString()[0]))
                    {
                        currentText.Append(key.ToString().ToLower());
                    }
                }
            }
            lastKeyboardState = keyboardState;
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = GameServices.SpriteBatch;
            var viewport = spriteBatch.GraphicsDevice.Viewport;
            var texture = TextureFactory.GetPlainTexture(spriteBatch.GraphicsDevice);

            spriteBatch.Draw(texture, viewport.Bounds, Color.Black * 0.7f);

            Vector2 promptSize = font.MeasureString(prompt);
            Vector2 promptPos = new Vector2(viewport.Width / 2 - promptSize.X / 2, viewport.Height / 2 - 50);
            spriteBatch.DrawString(font, prompt, promptPos, Color.White);

            Rectangle inputRect = new Rectangle(viewport.Width / 2 - 150, viewport.Height / 2, 300, 40);
            spriteBatch.Draw(texture, inputRect, Color.Gray);

            Vector2 textPos = new Vector2(inputRect.X + 5, inputRect.Y + 5);
            spriteBatch.DrawString(font, currentText.ToString(), textPos, Color.White);
        }
    }
}