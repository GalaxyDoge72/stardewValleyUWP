using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using stardewValleyUWP.Objects;
using System.Collections.Generic;
using stardewValleyUWP.Utilities;
using System.Threading.Tasks;

namespace stardewValleyUWP.Handlers
{
    public class UIManager
    {
        private class UIElementInfo
        {
            public IUIElement Element;
            public int Row;
            public int Col;
            public float WidthPercent;
            public float HeightPercent;
        }

        private List<UIElementInfo> elementInfos = new List<UIElementInfo>();
        private Grid grid;

        public UIManager(Grid grid) => this.grid = grid;

        public async Task AddElementAsync(IUIElement element, int row, int col, float widthPercent = 1f, float heightPercent = 1f)
        {
            if (row <= 0 || col <= 0 || row > grid.rowCount || col > grid.columnCount)
            {
                await MessageBox.Show("Attempted to add UI element with invalid column or row value.\nThis object will not appear on the screen and cannot be interacted with.", "Invalid Object Placement");
            }

            elementInfos.Add(new UIElementInfo
            {
                Element = element,
                Row = row,
                Col = col,
                WidthPercent = widthPercent,
                HeightPercent = heightPercent
            });

            var lastElement = elementInfos[elementInfos.Count - 1];

            RecalculateElement(lastElement);
        }

        private void RecalculateElement(UIElementInfo info)
        {
            info.Element.Bounds = grid.GetCellScaled(info.Row, info.Col, info.WidthPercent, info.HeightPercent);
        }

        public void RecalculatePositions()
        {
            foreach (var info in elementInfos)
                RecalculateElement(info);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var info in elementInfos)
                info.Element.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var info in elementInfos)
                info.Element.Draw(spriteBatch);
        }
    }
}
