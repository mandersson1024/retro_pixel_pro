using UnityEngine;
using System.Collections.Generic;

namespace AlpacaSound.RetroPixelPro
{
	[ExecuteInEditMode]
    [System.Serializable]
    public class Colormap : ScriptableObject
    {
        public ColormapPrecision colormapPrecision;

        [Range(1, 256)]
        public int numberOfColors;

        [HideInInspector]
        public Color[] palette;

        [HideInInspector]
        public bool[] usedColors;

        [HideInInspector]
        public Color32[] buffer;

		[HideInInspector]
		public bool initialized;

		[System.NonSerialized]
		public bool changedInternally;


        public Colormap()
        {
            palette = new Color[256];
            numberOfColors = 16;
            usedColors = new bool[256];
            colormapPrecision = ColormapPrecision.Medium;
			initialized = false;
        }


		void OnEnable()
		{
			changedInternally = true;

			if (!initialized)
			{
				Debug.LogWarning("The colormap '" + this.name + "' has not been initialized. Please click on it in the editor and it will set itself up.");
			}
		}


        public void SetColors(List<Color32> colors)
        {
            for (int i = 0; i < 256; ++i)
            {
                if (i < colors.Count)
                {
                    palette[i] = colors[i];
                    usedColors[i] = true;
                }
            }
        }


        public void UseAsPreset(Colormap preset)
        {
            numberOfColors = preset.numberOfColors;
            usedColors = preset.usedColors;
            System.Array.Copy(preset.palette, palette, 256);
        }

    }

}

