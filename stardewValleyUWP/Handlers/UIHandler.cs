using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using stardewValleyUWP.Objects;
using stardewValleyUWP.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            info.Element.Bounds = grid.GetCellScaled(info.Row, info.Col, info.WidthPercent, info.HeightPercent, margin: 5);
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

        public void HandleTouchInput(Vector2 touchPosition)
        {
            foreach (var elementInfo in elementInfos) // Changed variable name for clarity
            {
                // Access the IUIElement from the UIElementInfo object
                if (elementInfo.Element is Button button)
                {
                    if (button.Bounds.Contains(touchPosition))
                    {
                        button.onClick?.Invoke();
                        return; // Exit after the first button is clicked
                    }
                }
            }
        }
    }
}
