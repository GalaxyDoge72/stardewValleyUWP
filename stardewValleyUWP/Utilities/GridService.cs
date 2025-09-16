using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace stardewValleyUWP.Objects
{
    public class Grid
    {
        private readonly int rows;
        private readonly int cols;
        private readonly GraphicsDevice graphicsDevice;

        public int Width { get; set; }
        public int Height { get; set; }
        public bool Visible { get; set; } = false;
        public int rowCount { get; }
        public int columnCount { get; }
        private Texture2D pixel;

        // Add padding
        public int PaddingLeft { get; set; } = 10;
        public int PaddingTop { get; set; } = 10;
        public int PaddingRight { get; set; } = 10;
        public int PaddingBottom { get; set; } = 10;

        public Grid(int rows, int cols, int width, int height, GraphicsDevice graphicsDevice)
        {
            this.rows = rows;
            this.cols = cols;
            this.columnCount = cols;
            this.rowCount = rows;
            Width = width;
            Height = height;
            this.graphicsDevice = graphicsDevice;

            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
        }

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

        public Rectangle GetCellScaled(int row, int col, float widthPercent = 1f, float heightPercent = 1f)
        {
            var cell = GetCell(row, col);

            int w = (int)(cell.Width * widthPercent);
            int h = (int)(cell.Height * heightPercent);
            int x = cell.X + (cell.Width - w) / 2;
            int y = cell.Y + (cell.Height - h) / 2;

            return new Rectangle(x, y, w, h);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            if (!Visible) return;

            int usableWidth = Width - PaddingLeft - PaddingRight;
            int usableHeight = Height - PaddingTop - PaddingBottom;

            int cellWidth = usableWidth / cols;
            int cellHeight = usableHeight / rows;

            for (int c = 0; c <= cols; c++)
                spriteBatch.Draw(pixel, new Rectangle(PaddingLeft + c * cellWidth, PaddingTop, 1, usableHeight), color);

            for (int r = 0; r <= rows; r++)
                spriteBatch.Draw(pixel, new Rectangle(PaddingLeft, PaddingTop + r * cellHeight, usableWidth, 1), color);
        }
    }

}
