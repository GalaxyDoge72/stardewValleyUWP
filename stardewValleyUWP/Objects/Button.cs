using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace stardewValleyUWP.Objects
{
    public class Button
    {
        private MouseState _previousMouse;

        public Rectangle Bounds { get; set; }
        public string Text { get; set; }
        public SpriteFont Font { get; set; }

        public Color BGColor { get; set; } = Color.DarkRed;
        public Color HoverColor { get; set; } = Color.Gray;
        public Color TextColor { get; set; } = Color.White;

        public Action onClick { get; set; }

        public Button(Rectangle bounds, string text, SpriteFont font)
        {
            Bounds = bounds;
            Text = text;
            Font = font;
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();

            if (Bounds.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
            {
                onClick?.Invoke();
            }

            _previousMouse = mouse;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var mouse = Mouse.GetState();
            var color = Bounds.Contains(mouse.Position) ? HoverColor : BGColor;

            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });
            spriteBatch.Draw(rect, Bounds, color);

            if (Font != null && !string.IsNullOrEmpty(Text))
            {
                var textSize = Font.MeasureString(Text);
                var textPos = new Vector2(
                    Bounds.X + (Bounds.Width - textSize.X) / 2,
                    Bounds.Y + (Bounds.Height - textSize.Y) / 2);

                spriteBatch.DrawString(Font, Text, textPos, TextColor);
            }
        }

    }
}