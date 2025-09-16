using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;

namespace stardewValleyUWP.Utilities
{
    public static class MessageBox
    {
        /// <summary>
        /// Show a message box
        /// </summary>
        /// <param name="content">The main body of the message.</param>
        /// <param name="title"></param>
        /// <param name="isFatal">If true, exit application early.</param>
        /// <returns></returns>
        public static async Task Show(string content, string title = "Warning!", bool isFatal = false)
        {
            var dialog = new MessageDialog(content, title);
            await dialog.ShowAsync();

            if (isFatal)
            {
                CoreApplication.Exit();
            }
        }
    }
}