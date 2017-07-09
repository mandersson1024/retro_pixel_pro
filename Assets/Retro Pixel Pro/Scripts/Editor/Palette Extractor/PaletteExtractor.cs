using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AlpacaSound.RetroPixelPro
{

    public class PaletteExtractor
    {
        public PaletteTree paletteTree;


        public PaletteExtractor(Texture2D texture)
        {
            ColorBucket bucket = new ColorBucket(texture.GetPixels32());
            paletteTree = new PaletteTree(bucket, 0, 8);
        }


        public List<Color32> GetColors(int numColors)
        {
            int mainLevel = (int)Math.Ceiling(Math.Log(numColors, 2));
            int backupLevel = mainLevel - 1;

            List<Color32> mainColors = paletteTree.GetColorsFromLevel(mainLevel);
            List<Color32> backupColors = paletteTree.GetColorsFromLevel(backupLevel);

            //Debug.Log("num main colors: " + mainColors.Count);
            //Debug.Log("num backup colors: " + backupColors.Count);

            List<Color32> result = new List<Color32>();

            for (int i = 0; i < numColors; ++i)
            {
                if (UseMainColor(mainLevel, numColors, i))
                {
                    Debug.Log("using main color for: " + i);
                    result.Add(mainColors[i]);
                }
                else
                {
                    int backupIndex = backupColors.Count - (numColors - i);
                    Debug.Log("needed backup color for: " + i + ", backup index: " + backupIndex);
                    result.Add(backupColors[backupIndex]);
                }
            }

            return result;
        }


        bool UseMainColor(int mainLevel, int numColors, int colorIndex)
        {
            int numColorsInMainLevel = (int)Math.Round(Math.Pow(2, mainLevel));
            int backupsNeeded = numColorsInMainLevel - numColors;

            return colorIndex < (numColors - backupsNeeded);
        }
    
    }
        
}

