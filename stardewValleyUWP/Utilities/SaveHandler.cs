using System;
using System.Collections.Generic;
using System.IO;

namespace stardewValleyUWP.Utilities
{
    public static class SaveHandler
    {
        private static readonly string SaveFolder = Path.Combine(
            Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Saves");

        static SaveHandler()
        {
            if (!Directory.Exists(SaveFolder))
            {
                Directory.CreateDirectory(SaveFolder);
            }
        }

        private static string GetSavePath(string saveName)
        {
            return Path.Combine(SaveFolder, saveName + ".sav");
        }

        public static List<string> getSaveFileNames()
        {
            var saves = new List<string>();
            if (!Directory.Exists(SaveFolder))
            {
                return saves;
            }

            foreach (var file in Directory.GetFiles(SaveFolder, "*.sav"))
            {
                saves.Add(Path.GetFileNameWithoutExtension(file));
            }
            
            return saves;
        }

        public static void CreateNewSave(string saveName)
        {
            if (string.IsNullOrEmpty(saveName))
            {
                MessageBox.Show("Save name cannot be empty.");
            }
            string path = GetSavePath(saveName);
            File.WriteAllText(path, "");
        }

        public static string LoadSave(string saveName)
        {
            string path = GetSavePath(saveName);
            if (!File.Exists(path))
            {
                MessageBox.Show($"Save file {saveName} does not exist.");
            }
            return File.ReadAllText(path);
        }

        public static void SaveGame(string saveName, string data)
        {
            string path = GetSavePath(saveName);
            File.WriteAllText(path, data);
        }

        public static void DeleteSave(string saveName)
        {
            string savePath = GetSavePath(saveName);
            if (File.Exists(savePath))
            {
               File.Delete(savePath);
            }
        }
    }
}