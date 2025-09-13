using Microsoft.Xna.Framework;

namespace stardewValleyUWP.Objects
{
    public abstract class Screen
    {
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}