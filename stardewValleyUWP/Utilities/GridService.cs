using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace stardewValleyUWP.Utilities
{
    public class Grid
    {
        private readonly int rows;
        private readonly int cols;
        private readonly GraphicsDevice graphicsDevice;

        public int Width { get; set; }
        public int Height { get; set; }
        public bool Visible { get; set; } = false;

        // Padding defines unusable outer area
        public int PaddingLeft { get; set; } = 20;
        public int PaddingTop { get; set; } = 20;
        public int PaddingRight { get; set; } = 20;
        public int PaddingBottom { get; set; } = 20;

        private Texture2D pixel;

        public Grid(int rows, int cols, int width, int height, GraphicsDevice graphicsDevice)
        {
            this.rows = rows;
            this.cols = cols;
            Width = width;
            Height = height;
            this.graphicsDevice = graphicsDevice;

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

        // Get a rectangle for a cell, **only inside padded area**
        public Rectangle GetCell(int row, int col)
        {
            int usableWidth = Width - PaddingLeft - PaddingRight;
            int usableHeight = Height - PaddingTop - PaddingBottom;

            int cellWidth = usableWidth / cols;
            int cellHeight = usableHeight / rows;

            int x = PaddingLeft + col * cellWidth;
            int y = PaddingTop + row * cellHeight;

            return new Rectangle(x, y, cellWidth, cellHeight);
        }

        // Scale element within cell, clamped to padded area
        public Rectangle GetCellScaled(int row, int col, float widthPercent = 1f, float heightPercent = 1f, int margin = 5)
        {
            var cell = GetCell(row, col);

            int w = (int)(cell.Width * widthPercent);
            int h = (int)(cell.Height * heightPercent);

            int x = cell.X + (cell.Width - w) / 2;
            int y = cell.Y + (cell.Height - h) / 2;

            int maxX = Width - PaddingRight - margin;
            int maxY = Height - PaddingBottom - margin;

            x = Math.Max(x, PaddingLeft + margin);
            y = Math.Max(y, PaddingTop + margin);

            w = Math.Min(w, maxX - x);
            h = Math.Min(h, maxY - y);

            return new Rectangle(x, y, w, h);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (!Visible) return;

            int usableWidth = Width - PaddingLeft - PaddingRight;
            int usableHeight = Height - PaddingTop - PaddingBottom;

            int cellWidth = usableWidth / cols;
            int cellHeight = usableHeight / rows;

            // Vertical lines
            for (int c = 0; c <= cols; c++)
                spriteBatch.Draw(pixel, new Rectangle(PaddingLeft + c * cellWidth, PaddingTop, 1, usableHeight), color);

            // Horizontal lines
            for (int r = 0; r <= rows; r++)
                spriteBatch.Draw(pixel, new Rectangle(PaddingLeft, PaddingTop + r * cellHeight, usableWidth, 1), color);
        }
    }
}
