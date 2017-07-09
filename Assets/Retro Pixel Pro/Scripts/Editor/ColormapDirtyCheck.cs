using UnityEngine;
using System.Collections;


namespace AlpacaSound.RetroPixelPro
{

	public class ColormapDirtyCheck
	{

		public PalettePresets.PresetName preset;
		public ColormapPrecision colormapPrecision;
		public int numberOfColors;

		// Set to true when fiddling with variables other than the listed.
		public bool forceDirty;

		Colormap colormap;

		public ColormapDirtyCheck(Colormap colormap)
		{
			this.colormap = colormap;
			Reset();
		}


		// Called after update is done.
		public void Reset()
		{
			forceDirty = false;
			preset = colormap.preset;
			colormapPrecision = colormap.colormapPrecision;
			numberOfColors = colormap.numberOfColors;
		}


		// Called when deciding whether do display Apply Changes button enabled or not.
		public bool IsDirty()
		{
			return
				forceDirty ||
				(preset != colormap.preset) ||
				(colormapPrecision != colormap.colormapPrecision) ||
				(numberOfColors != colormap.numberOfColors);
		}


	}

}

