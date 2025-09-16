using Microsoft.Xna.Framework;
using System.Collections.Generic;
using stardewValleyUWP.Objects;
using System.Threading.Tasks;
using stardewValleyUWP.Utilities;
using System;

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

        public async Task SwitchScreen(string key)
        {
            if (_screens.TryGetValue(key, out var newScreen))
            {
                _currentScreen?.UnloadContent();
                _currentScreen = newScreen;
            }

            else
            {
                await MessageBox.Show($"FATAL: Attempted to switch to screen '{key}'.\nThis screen does not exist or could not be found.", "Fatal exception!", true);
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
