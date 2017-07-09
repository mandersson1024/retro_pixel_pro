using UnityEngine;
using System.Collections;

namespace AlpacaSound.RetroPixelPro
{
	[ExecuteInEditMode]
    [System.Serializable]
    public class Colormap : ScriptableObject
    {
		public PalettePresets.PresetName preset;

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
			preset = PalettePresets.PresetName.Classic1;
            palette = new Color[256];
            numberOfColors = 16;
            usedColors = new bool[256];
            colormapPrecision = ColormapPrecision.Medium;
			initialized = false;

			PalettePresets.SetPalette(preset, this);
        }


		void OnEnable()
		{
			changedInternally = true;

			if (!initialized)
			{
				Debug.LogWarning("The colormap '" + this.name + "' has not been initialized. Please click on it in the editor and it will set itself up.");
			}
		}



    }

}

