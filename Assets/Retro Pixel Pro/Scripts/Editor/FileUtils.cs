using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


namespace AlpacaSound.RetroPixelPro
{
    public class FileUtils
    {
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

    }
}


