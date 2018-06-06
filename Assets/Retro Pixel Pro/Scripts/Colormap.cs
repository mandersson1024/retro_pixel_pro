using UnityEngine;
using System.Collections.Generic;

namespace AlpacaSound.RetroPixelPro
{
    [ExecuteInEditMode]
    [System.Serializable]
    public class Colormap : ScriptableObject
    {
        public bool preview;

        [Range(2, 64)]
        public int numberOfColors;

        [HideInInspector]
        public Color32[] palette;

        [HideInInspector]
        public bool[] usedColors;

        [HideInInspector]
        public Color32[] pixels;

        [HideInInspector]
        public bool initialized;

        [System.NonSerialized]
        public bool changedInternally;


        public Colormap()
        {
            palette = new Color32[64];
            numberOfColors = 16;
            usedColors = new bool[64];
            preview = true;
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
            for (int i = 0; i < 64; ++i)
            {
                if (i < colors.Count)
                {
                    palette[i] = colors[i];
                    usedColors[i] = true;
                }
            }
        }


        public void ApplyPreset(ColormapPreset preset)
        {
            if (preset != null)
            {
                numberOfColors = preset.palette.Length;

                for (int i = 0; i < 64; ++i)
                {
                    usedColors[i] = i < numberOfColors;
                }

                System.Array.Copy(preset.palette, palette, preset.palette.Length);
            }
        }

    }

}

