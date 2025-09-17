using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using stardewValleyUWP.Objects;
using stardewValleyUWP.Utilities;
using System;
using System.Collections.Generic;
using Windows.System.Profile;
using Windows.UI.Core; // Add this for character input
using Windows.UI.ViewManagement; // Add this using statement for InputPane
using Windows.UI.Xaml;

namespace stardewValleyUWP.Objects
{
    public class SaveListUI : IUIElement
    {
        public Rectangle Bounds { get; set; }
        private ScrollableList list;
        private SpriteFont font;
        private string newSaveName = "";
        private bool isCreatingNewSave = false;

        // Mouse and Keyboard states for non-mobile platforms
        private KeyboardState previousKeyboardState;
        private MouseState previousMouseState;

        // InputPane for Windows Phone
        private InputPane inputPane;
        private bool isWindowsPhone;

        public SaveListUI(SpriteFont font, Rectangle bounds)
        {
            this.font = font;
            this.Bounds = bounds;
            list = new ScrollableList(font)
            {
                Bounds = bounds,
            };

            isWindowsPhone = AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";

            if (isWindowsPhone)
            {
                // Set up the input pane for the on-screen keyboard
                inputPane = InputPane.GetForCurrentView();
                // We'll hook up the keyboard events when a save name is being created
            }

            PopulateList();
        }

        private int getTextWidth(string text)
        {
            int width = (int)font.MeasureString(text).X;
            return width;
        }

        private void OnCharacterReceived(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            if (isCreatingNewSave)
            {
                char typedChar = (char)args.KeyCode;
                if (char.IsLetterOrDigit(typedChar) || typedChar == ' ')
                {
                    newSaveName += typedChar;
                }
                else if (typedChar == '\b' && newSaveName.Length > 0) // Backspace
                {
                    newSaveName = newSaveName.Substring(0, newSaveName.Length - 1);
                }
                else if (typedChar == '\r') // Enter key
                {
                    if (!string.IsNullOrWhiteSpace(newSaveName))
                    {
                        SaveHandler.CreateNewSave(newSaveName);
                        isCreatingNewSave = false;
                        PopulateList();
                        inputPane.TryHide();
                    }
                }
            }
        }

        public void PopulateList()
        {
            var saves = SaveHandler.getSaveFileNames();
            var listItems = new List<ListItem>();

            if (saves.Count > 0)
            {
                foreach (var save in saves)
                {
                    var deleteButton = new SubButton
                    {
                        Text = "Delete",
                        OnClick = () => DeleteSave(save),
                        Size = new Vector2(getTextWidth("Delete"), 32)
                    };

                    listItems.Add(new ListItem
                    {
                        Text = save,
                        SubButtons = { deleteButton },
                        OnClick = () => LoadSave(save)
                    });
                }
            }
            else
            {
                listItems.Add(new ListItem
                {
                    Text = "Create New Save",
                    OnClick = () =>
                    {
                        isCreatingNewSave = true;
                        newSaveName = "";
                        if (isWindowsPhone)
                        {
                            inputPane.TryShow();
                            Window.Current.CoreWindow.CharacterReceived += OnCharacterReceived;
                        }
                    }
                });
            }

            list.SetItems(listItems);
        }

        private void LoadSave(string saveName)
        {
            string data = SaveHandler.LoadSave(saveName);
        }

        private void DeleteSave(string saveName)
        {
            SaveHandler.DeleteSave(saveName);
            PopulateList();
        }

        public void Update(GameTime gameTime)
        {
            if (isCreatingNewSave)
            {
                if (!isWindowsPhone)
                {
                    HandleKeyboardInput();
                }
                // On Windows Phone, input is handled by the OnCharacterReceived event.
            }
            else
            {
                list.Update(gameTime);
                if (isWindowsPhone)
                {
                    // Clean up the event handler when not in input mode
                    Window.Current.CoreWindow.CharacterReceived -= OnCharacterReceived;
                }
            }
        }

        private void HandleKeyboardInput()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            Keys[] pressedKeys = currentKeyboardState.GetPressedKeys();

            foreach (Keys key in pressedKeys)
            {
                if (previousKeyboardState.IsKeyUp(key))
                {
                    if (key == Keys.Enter)
                    {
                        if (!string.IsNullOrWhiteSpace(newSaveName))
                        {
                            SaveHandler.CreateNewSave(newSaveName);
                            isCreatingNewSave = false;
                            PopulateList();
                        }
                    }
                    else if (key == Keys.Back)
                    {
                        if (newSaveName.Length > 0)
                        {
                            newSaveName = newSaveName.Substring(0, newSaveName.Length - 1);
                        }
                    }
                    else if (IsAlphanumeric(key))
                    {
                        newSaveName += key.ToString();
                    }
                }
            }

            previousKeyboardState = currentKeyboardState;
        }

        private bool IsAlphanumeric(Keys key)
        {
            return (key >= Keys.A && key <= Keys.Z) || (key >= Keys.D0 && key <= Keys.D9) || key == Keys.Space;
        }

        public void Draw(SpriteBatch spriteBatch, float alpha = 1f)
        {
            if (isCreatingNewSave)
            {
                spriteBatch.Draw(TextureFactory.GetPlainTexture(spriteBatch.GraphicsDevice), Bounds, Color.Black * 0.7f);

                string prompt = "Enter Save Name:";
                Vector2 promptSize = font.MeasureString(prompt);
                Vector2 promptPos = new Vector2(Bounds.X + (Bounds.Width - promptSize.X) / 2, Bounds.Y + 50);
                spriteBatch.DrawString(font, prompt, promptPos, Color.White);

                Vector2 inputPos = new Vector2(promptPos.X, promptPos.Y + promptSize.Y + 10);
                spriteBatch.DrawString(font, newSaveName, inputPos, Color.White);
            }
            else
            {
                list.Draw(spriteBatch, alpha);
            }
        }

        public void Draw(SpriteBatch spriteBatch) => Draw(spriteBatch, 1f);
    }
}