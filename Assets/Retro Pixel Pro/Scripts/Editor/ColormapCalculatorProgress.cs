using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlpacaSound.RetroPixelPro
{

	public class ColormapCalculatorProgress
	{
		public Color32 color;
		public int steps;
		public int progress;

		public ColormapCalculatorProgress(int colorsteps)
		{
			steps = colorsteps;
			color = new Color32(0, 0, 0, 0);
			progress = 0;
		}

		public void NextPixel()
		{
			++color.r;

			if (color.r == steps)
			{
				color.r = 0;
				++color.g;

				if (color.g == steps)
				{
					color.g = 0;
					++color.b;
				}
			}

			++progress;
		}

		public Vector3 GetRGBCoordinate()
		{
			return new Vector3(color.r, color.g, color.b);
		}


	}

}

