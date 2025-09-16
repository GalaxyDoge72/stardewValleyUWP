using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using stardewValleyUWP.Utilities;

namespace stardewValleyUWP.Objects
{
    public class Label : IUIElement
    {
        public Rectangle Bounds { get; set; }
        public string Text { get; set; }
        public SpriteFont Font { get; set; }
        public Color TextColor { get; set; } = Color.White;

        public Label(string text, SpriteFont font)
        {
            Text = text;
            Font = font;
        }

        public void Update(GameTime gameTime)
        {
            // Static label, no updates
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var size = Font.MeasureString(Text);
            var pos = new Vector2(Bounds.X + (Bounds.Width - size.X) / 2,
                                  Bounds.Y + (Bounds.Height - size.Y) / 2);
            spriteBatch.DrawString(Font, Text, pos, TextColor);
        }
    }
}
