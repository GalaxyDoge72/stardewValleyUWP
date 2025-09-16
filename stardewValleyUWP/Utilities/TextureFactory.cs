using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace stardewValleyUWP.Utilities
{
    public static class TextureFactory
    {
        private static Texture2D _whiteTexture;

        public static Texture2D GetPlainTexture(GraphicsDevice device)
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(device, 1, 1);
                _whiteTexture.SetData(new[] { Color.White });
            }
            return _whiteTexture;
        }
    }
}
