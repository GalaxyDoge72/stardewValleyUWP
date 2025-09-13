using Microsoft.Xna.Framework;
using System.Collections.Generic;
using stardewValleyUWP.Objects;

namespace stardewValleyUWP.Handlers
{
    public class ScreenManager
    {
        private Screen _currentScreen;
        private readonly Dictionary<string, Screen> _screens = new Dictionary<string, Screen>();

        public void AddScreen(string key, Screen screen)
        {
            _screens[key] = screen;
            screen.LoadContent();
        }

        public void SwitchScreen(string key)
        {
            if (_screens.TryGetValue(key, out var newScreen))
            {
                _currentScreen?.UnloadContent();
                _currentScreen = newScreen;
            }
        }

        public void Update(GameTime gameTime)
        {
            _currentScreen?.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _currentScreen?.Draw(gameTime);
        }
    }
}
