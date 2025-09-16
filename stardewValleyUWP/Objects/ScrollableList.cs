using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using stardewValleyUWP.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace stardewValleyUWP.Objects
{
    public class ListItem
    {
        public string Text { get; set; }
        public Action OnClick { get; set; }
    }
    public class ScrollableList : IUIElement
    {
        public Rectangle Bounds { get; set; }
        public SpriteFont Font { get; set; }
        public Color TextColor { get; set; } = Color.White;
        public Color BGColor { get; set; } = Color.DarkSlateGray;
        public Color HoverColor { get; set; } = Color.DimGray;

        public List<ListItem> Items { get; private set; } = new List<ListItem>();
        public Action<string> OnItemClick { get; set; }

        private int scrollOffset = 0;
        private int itemHeight = 32;
        private int hoveredIndex = -1;

        public ScrollableList(SpriteFont font)
        {
            Font = font;
        }

        private int VisibleItemCount()
        {
            return Bounds.Height / itemHeight;
        }

        public void SetItems(List<ListItem> items)
        {
            Items = items ?? new List<ListItem>();
            scrollOffset = 0;
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            hoveredIndex = -1;

            if (Bounds.Contains(mouse.Position))
            {
                if (mouse.ScrollWheelValue != 0)
                {
                    int delta = mouse.ScrollWheelValue / 120;
                    scrollOffset -= delta;
                    if (scrollOffset < 0) scrollOffset = 0;
                    int maxOffset = Math.Max(0, Items.Count - VisibleItemCount());
                    if (scrollOffset > maxOffset) scrollOffset = maxOffset;
                }

                int relativeY = mouse.Y - Bounds.Y;
                int index = scrollOffset + (relativeY / itemHeight);

                if (index >= 0 && index < Items.Count)
                {
                    hoveredIndex = index;
                    
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        Items[index].OnClick?.Invoke();
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, float alpha = 1f)
        {
            Texture2D texture = TextureFactory.GetPlainTexture(spriteBatch.GraphicsDevice);
            spriteBatch.Draw(texture, Bounds, BGColor * alpha);

            if (Items.Count == 0)
            {
                string msg = "Create New Save";
                Vector2 size = Font.MeasureString(msg);
                Vector2 pos = new Vector2(
                    Bounds.X + (Bounds.Width - size.X) /2,
                    Bounds.Y + (Bounds.Height -size.Y) /2
                );
                spriteBatch.DrawString(Font, msg, pos, TextColor * alpha);
                return;
            }

            int visibleCount = VisibleItemCount();
            for (int i = 0; i < visibleCount; i++)
            {
                int itemIndex = scrollOffset + i;
                if (itemIndex >= Items.Count) break;

                Rectangle itemBounds = new Rectangle(
                    Bounds.X,
                    Bounds.Y + i * itemHeight,
                    Bounds.Width,
                    itemHeight
                );

                if (itemIndex == hoveredIndex)
                {
                    spriteBatch.Draw(texture, itemBounds, HoverColor * alpha);
                }

                spriteBatch.DrawString(Font, Items[itemIndex].Text,
                    new Vector2(itemBounds.X + 5, itemBounds.Y + 5),
                    TextColor * alpha);
            }
        }

        public void Draw(SpriteBatch spriteBatch) => Draw(spriteBatch, 1f);
    }
}