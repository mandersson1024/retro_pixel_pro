﻿using System.Collections;
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
			color = new Color32(0, 0, 0, 0);
			progress = 0;
		}

		public void NextPixel()
		{
			++progress;
			color = ColormapUtils.IndexToColor(colorsteps, progress);
		}


		public Vector3 GetRGBCoordinate()
		{
			return ColormapUtils.GetColorstepPosition(color, colorsteps);
		}

		public bool IsDone()
		{
			return progress >= colorsteps * colorsteps * colorsteps;
		}


	}

}

