using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlpacaSound.RetroPixelPro
{
    [System.Serializable]
    public class ColormapPreset : ScriptableObject
    {
        public Color32[] palette;

        public void SetNumColors(int numColors)
        {
            palette = new Color32[numColors];
        }
    }


}

