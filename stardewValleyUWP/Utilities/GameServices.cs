using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace stardewValleyUWP.Utilities
{
    public static class GameServices
    {
        public static ContentManager Content {  get; set; } = null;
        public static SpriteBatch SpriteBatch { get; set; } = null;
    }
}