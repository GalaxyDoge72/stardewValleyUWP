using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using stardewValleyUWP.Utilities;

namespace stardewValleyUWP.Objects
{
    public class Button : IUIElement
    {
        public Rectangle Bounds { get; set; }
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color BGColor { get; set; } = Color.Gray;
        public Color HoverColor { get; set; } = Color.LightGray;
        public Action onClick { get; set; }

        private bool hovered;

        public Button(Rectangle bounds, string text, SpriteFont font)
        {
            Bounds = bounds;
            Text = text;
            Font = font;
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            hovered = Bounds.Contains(mouse.Position);

            if (hovered && mouse.LeftButton == ButtonState.Pressed)
                onClick?.Invoke();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureFactory.GetPlainTexture(spriteBatch.GraphicsDevice),
                             Bounds,
                             hovered ? HoverColor : BGColor);

            var textSize = Font.MeasureString(Text);
            Vector2 textPos = new Vector2(
                Bounds.X + (Bounds.Width - textSize.X) / 2,
                Bounds.Y + (Bounds.Height - textSize.Y) / 2
            );
            spriteBatch.DrawString(Font, Text, textPos, Color.White);
        }
    }
}
