using Microsoft.Xna.Framework;
using System.Collections.Generic;
using stardewValleyUWP.Objects;
using System.Threading.Tasks;
using stardewValleyUWP.Utilities;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace stardewValleyUWP.Handlers
{
    public class ScreenManager
    {
        private Screen _currentScreen;
        private Screen _nextScreen;

        private bool TransitionInProgress = false;
        private float _transitionAlpha = 0f;
        private float _transitionSpeed = 1f;

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
        
        public void switchScreenWithFade(string key, float fadeSpeed = 1f)
        {
            if (_screens.TryGetValue((string)key, out var newScreen))
            {
                _nextScreen = newScreen;
                TransitionInProgress = true;
                _transitionAlpha = 0f;
                _transitionSpeed = fadeSpeed;
            }
        }

        public void Update(GameTime gameTime)
        {
            _currentScreen?.Update(gameTime);

            if (TransitionInProgress)
            {
                _transitionAlpha += _transitionSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_transitionAlpha >= 1f)
                {
                    _currentScreen.UnloadContent();
                    _currentScreen = _nextScreen;
                    _nextScreen = null;
                    _transitionAlpha = 1f;

                    _transitionSpeed = -_transitionSpeed;
                }

                if (_transitionAlpha <= 0f)
                {
                    _transitionAlpha = 0f;
                    TransitionInProgress = false;
                    _transitionSpeed = Math.Abs(_transitionSpeed);
                }
            }

        }

        public void Draw(GameTime gameTime)
        {
            _currentScreen?.Draw(gameTime);

            if (TransitionInProgress)
            {
                var sb = GameServices.SpriteBatch;
                sb.Begin();
                Texture2D black = TextureFactory.GetPlainTexture(sb.GraphicsDevice);
                sb.Draw(black,
                    new Rectangle(0, 0, sb.GraphicsDevice.Viewport.Width, sb.GraphicsDevice.Viewport.Height),
                    Color.Black * _transitionAlpha
                );
                sb.End();
            }
        }
    }
}
