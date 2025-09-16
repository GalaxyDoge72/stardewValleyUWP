using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using stardewValleyUWP.Utilities;
using System;
using System.Collections.Generic;

namespace stardewValleyUWP.Objects
{
    public class SaveListUI : IUIElement
    {
        public Rectangle Bounds { get; set; }
        private ScrollableList list;
        private SpriteFont font;

        public SaveListUI(SpriteFont font, Rectangle bounds)
        {
            this.font = font;
            this.Bounds = bounds;
            list = new ScrollableList(font)
            {
                Bounds = bounds,
                OnItemClick = LoadSave
            };
            PopulateList();
        }

        public void PopulateList()
        {
            var saves = SaveHandler.getSaveFileNames();

            if (saves.Count > 0)
            {
                list.SetItems(saves);
            }
            else
            {
                list.SetItems(new List<string> { "Create New Save" });
                list.OnItemClick = (name) =>
                {
                    SaveHandler.CreateNewSave("NewSave1");
                    PopulateList(); // refresh list
                };
            }
        }

        private void LoadSave(string saveName)
        {
            string data = SaveHandler.LoadSave(saveName);
            // TODO: parse data & load game
        }

        public void Update(GameTime gameTime) => list.Update(gameTime);

        public void Draw(SpriteBatch spriteBatch, float alpha = 1f) => list.Draw(spriteBatch, alpha);

        public void Draw(SpriteBatch spriteBatch) => Draw(spriteBatch, 1f);
    }
}
