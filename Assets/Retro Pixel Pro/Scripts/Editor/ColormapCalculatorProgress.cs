using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlpacaSound.RetroPixelPro
{

	public class ColormapCalculatorProgress
	{
		public Color32 color;
		public int colorsteps;
		public int progress;

		public ColormapCalculatorProgress(int colorsteps)
		{
			this.colorsteps = colorsteps;
			progress = 0;
			color = ColormapUtils.IndexToColor(colorsteps, 0);
		}

		public void NextPixel()
		{
			++progress;
			color = ColormapUtils.IndexToColor(colorsteps, progress);
		}


		public bool IsDone()
		{
			return progress >= colorsteps * colorsteps * colorsteps;
		}


	}

}

