using UnityEngine;
using System.Collections;
using UnityEditor;


namespace AlpacaSound.RetroPixelPro
{

	public class ColormapCalculator
	{
		public float progress;
		public Color32[] pixelBuffer;

		Color32[] palette;
		bool[] usedColors;
		int numColors;
		System.Action doneCallback;
		int colorsteps;
		int totalPixels;
		ColormapCalculatorProgress ccprogress;


		public ColormapCalculator(ColormapPrecision precision, Color32[] palette, bool[] usedColors, int numColors, System.Action doneCallback)
		{
			this.palette = palette;
			this.usedColors = usedColors;
			this.doneCallback = doneCallback;
			this.numColors = numColors;
			progress = 0;
			colorsteps = ColormapUtils.GetPrecisionColorsteps(precision);
			ccprogress = new ColormapCalculatorProgress(colorsteps);
			totalPixels = colorsteps * colorsteps * colorsteps;
			pixelBuffer = new Color32[totalPixels];
		}


		public void CalculateChunk()
		{
			double frameStartTime = EditorApplication.timeSinceStartup;

			while (EditorApplication.timeSinceStartup < frameStartTime + (1.0 / 30.0))
			{
				CalculateNextPixel();
			}
		}


		void CalculateNextPixel()
		{
			if (ccprogress.progress < totalPixels)
			{
				byte paletteIndex = GetClosestPaletteIndex();
				pixelBuffer[ccprogress.progress] = new Color32(0, 0, 0, paletteIndex);
				ccprogress.NextPixel();
				progress = (float)ccprogress.progress / (float)totalPixels;
			}
			else
			{
				doneCallback.Invoke();
			}
		}


		byte GetClosestPaletteIndex()
		{
			float closestDistance = float.MaxValue;
			int closestIndex = 0;
			Vector3 rgb = ccprogress.GetRGBCoordinate();
			rgb = 256 * rgb / (colorsteps - 1);

			for (int i = 0; i < numColors; ++i)
			{
				if (usedColors[i])
				{
					Vector3 paletteRGB = new Vector3(palette[i].r, palette[i].g, palette[i].b);
					float distance = Vector3.Distance(rgb, paletteRGB);
					if (distance < closestDistance)
					{
						closestDistance = distance;
						closestIndex = i;
					}
				}
			}

			return (byte)closestIndex;
		}

	}

}


