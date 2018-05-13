using UnityEngine;
using System.Collections;


namespace AlpacaSound.RetroPixelPro
{

	public class ColormapDirtyCheck
	{

		public bool preview;
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
			preview = colormap.preview;
			numberOfColors = colormap.numberOfColors;
		}


		// Called when deciding whether do display Apply Changes button enabled or not.
		public bool IsDirty()
		{
			return
				forceDirty ||
				(preview != colormap.preview) ||
				(numberOfColors != colormap.numberOfColors);
		}


	}

}

