using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using stardewValleyUWP.Utilities;
using System;
using System.Collections.Generic;
using stardewValleyUWP.Objects;

namespace stardewValleyUWP.Objects
{
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
        private const int itemHeight = 32; // A constant item height for simplicity
        private int hoveredIndex = -1;

        private MouseState previousMouseState;

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
                // Handle scroll wheel
                int scrollDelta = mouse.ScrollWheelValue;
                if (scrollDelta != 0)
                {
                    scrollOffset -= scrollDelta / 120;
                    if (scrollOffset < 0) scrollOffset = 0;
                    int maxOffset = Math.Max(0, Items.Count - VisibleItemCount());
                    if (scrollOffset > maxOffset) scrollOffset = maxOffset;
                }

                int relativeY = mouse.Y - Bounds.Y;
                int index = scrollOffset + (relativeY / itemHeight);

                if (index >= 0 && index < Items.Count)
                {
                    // Calculate the bounds for the entire item row, including sub-buttons.
                    Vector2 textSize = Font.MeasureString(Items[index].Text);
                    float mainButtonWidth = textSize.X + 15;
                    float subButtonAreaWidth = 0;
                    if (Items[index].SubButtons != null)
                    {
                        foreach (var subButton in Items[index].SubButtons)
                        {
                            subButtonAreaWidth += subButton.Size.X + 5;
                        }
                    }

                    Rectangle itemBounds = new Rectangle(
                        Bounds.X,
                        Bounds.Y + (index - scrollOffset) * itemHeight,
                        (int)(mainButtonWidth + subButtonAreaWidth),
                        itemHeight
                    );

                    if (itemBounds.Contains(mouse.Position))
                    {
                        hoveredIndex = index;

                        if (Items[index].SubButtons != null)
                        {
                            float currentX = itemBounds.X + mainButtonWidth + 5;
                            foreach (var subButton in Items[index].SubButtons)
                            {
                                Rectangle subButtonBounds = new Rectangle(
                                    (int)currentX,
                                    itemBounds.Y + (itemBounds.Height - (int)subButton.Size.Y) / 2,
                                    (int)subButton.Size.X,
                                    (int)subButton.Size.Y
                                );

                                if (subButtonBounds.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                                {
                                    subButton.OnClick?.Invoke();
                                    previousMouseState = mouse;
                                    return;
                                }
                                currentX += subButton.Size.X + 5;
                            }
                        }

                        if (mouse.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                        {
                            Items[index].OnClick?.Invoke();
                        }
                    }
                }
            }
            previousMouseState = mouse;
        }

        public void Draw(SpriteBatch spriteBatch, float alpha = 1f)
        {
            var texture = TextureFactory.GetPlainTexture(spriteBatch.GraphicsDevice);

            spriteBatch.Draw(texture, Bounds, BGColor * alpha);

            if (Items.Count == 0)
            {
                string msg = "Create New Save";
                Vector2 size = Font.MeasureString(msg);
                Vector2 pos = new Vector2(
                    Bounds.X + (Bounds.Width - size.X) / 2,
                    Bounds.Y + (Bounds.Height - size.Y) / 2
                );
                spriteBatch.DrawString(Font, msg, pos, TextColor * alpha);
                return;
            }

            int visibleCount = VisibleItemCount();
            for (int i = 0; i < visibleCount; i++)
            {
                int itemIndex = scrollOffset + i;
                if (itemIndex >= Items.Count) break;

                // Measure the main text to determine its width
                string itemText = Items[itemIndex].Text;
                Vector2 textSize = Font.MeasureString(itemText);
                float mainButtonWidth = textSize.X + 15; // Text width + padding

                // Calculate total width required for all sub-buttons
                float subButtonAreaWidth = 0;
                if (Items[itemIndex].SubButtons != null)
                {
                    foreach (var subButton in Items[itemIndex].SubButtons)
                    {
                        subButtonAreaWidth += subButton.Size.X + 5;
                    }
                }

                // Define the bounds for the entire list item row
                Rectangle itemBounds = new Rectangle(
                    Bounds.X,
                    Bounds.Y + i * itemHeight,
                    (int)(mainButtonWidth),
                    itemHeight
                );

                // Draw the background for the entire list item, or a hover background if needed
                if (itemIndex == hoveredIndex)
                {
                    spriteBatch.Draw(texture, itemBounds, HoverColor * alpha);
                }
                else
                {
                    spriteBatch.Draw(texture, itemBounds, BGColor * alpha);
                }

                // Draw the main list item text
                Vector2 textPos = new Vector2(itemBounds.X + 5, itemBounds.Y + (itemBounds.Height - textSize.Y) / 2);
                spriteBatch.DrawString(Font, itemText, textPos, TextColor * alpha);

                // Draw the sub-buttons next to the main text
                if (Items[itemIndex].SubButtons != null)
                {
                    float currentX = itemBounds.X + mainButtonWidth + 5;
                    foreach (var subButton in Items[itemIndex].SubButtons)
                    {
                        Vector2 buttonSize = subButton.Size;
                        Rectangle subButtonBounds = new Rectangle(
                            (int)currentX + 5,
                            itemBounds.Y + (itemBounds.Height - (int)buttonSize.Y) / 2,
                            (int)buttonSize.X,
                            (int)buttonSize.Y
                        );

                        spriteBatch.Draw(texture, subButtonBounds, Color.Red * alpha);

                        Vector2 subTextSize = Font.MeasureString(subButton.Text);
                        Vector2 subButtonSize = subButton.Size;
                        Vector2 subTextPos = new Vector2(
                            subButtonBounds.X + (subButtonBounds.Width - subTextSize.X) / 2,
                            subButtonBounds.Y + (subButtonBounds.Height - subTextSize.Y) / 2
                        );
                        spriteBatch.DrawString(Font, subButton.Text, subTextPos, Color.White * alpha);

                        currentX += buttonSize.X + 5;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) => Draw(spriteBatch, 1f);
    }
}