using UnityEngine;
using UnityEditor;
using System.IO;


namespace AlpacaSound.RetroPixelPro
{
    public class FileUtils
    {
        public const string PRESETS_DIRECTORY_PATH = "Assets/Retro Pixel Pro/Colormap Presets/";

        public static void AddFilesInDirectory(DirectoryInfo dirInfo, GenericMenu menu, string basePath, GenericMenu.MenuFunction2 callback)
        {
            foreach (DirectoryInfo subDirInfo in dirInfo.GetDirectories())
            {
                string subPath = basePath + subDirInfo.Name + "/";
                AddFilesInDirectory(subDirInfo, menu, subPath, callback);
            }

            foreach (FileInfo fileInfo in dirInfo.GetFiles("*.asset"))
            {
                string filename = basePath + fileInfo.Name;
                string displayname = filename.Split('.')[0];
                menu.AddItem(new GUIContent(displayname), false, callback, filename);
            }
        }

        public static ColormapPreset LoadColormapPreset(string presetName)
        {
            string filepath = PRESETS_DIRECTORY_PATH + presetName;
            ColormapPreset preset = (ColormapPreset)AssetDatabase.LoadAssetAtPath(filepath, typeof(ColormapPreset));
            return preset;
        }
    }
}


