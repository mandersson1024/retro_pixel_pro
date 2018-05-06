using UnityEngine;
using System.Collections.Generic;

namespace AlpacaSound.RetroPixelPro
{
    [ExecuteInEditMode]
    [System.Serializable]
    public class Colormap : ScriptableObject
    {
        [Range(2, 256)]
        public int numberOfColors;

        [HideInInspector]
        public Color32[] palette;

        [HideInInspector]
        public Color32[] texturePixels;

        [HideInInspector]
        public bool initialized;

        [System.NonSerialized]
        public bool changedInternally;


        public Colormap()
        {
            palette = new Color32[256];
            numberOfColors = 16;
            initialized = false;
        }


        void OnEnable()
        {
            changedInternally = true;

            if (!initialized)
            {
                Debug.LogWarning("The colormap has not yet been initialized. Please click on it in the editor and it will set itself up.");
            }
        }


        public void SetColors(List<Color32> colors)
        {
            for (int i = 0; i < 256; ++i)
            {
                if (i < colors.Count)
                {
                    palette[i] = colors[i];
                }
            }
        }


        public void ApplyPreset(ColormapPreset preset)
        {
            if (preset != null)
            {
                numberOfColors = preset.palette.Length;
                System.Array.Copy(preset.palette, palette, preset.palette.Length);
            }
        }

        public Color32[] GetUsedPalette()
        {
            Color32[] usedPalette = new Color32[numberOfColors];
            System.Array.Copy(palette, usedPalette, numberOfColors);
            return usedPalette;
        }

    }

}

