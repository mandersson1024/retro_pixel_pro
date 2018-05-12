using UnityEngine;
using System.Collections;
using UnityEditor;


namespace AlpacaSound.RetroPixelPro
{

	public class ColormapCalculator
	{
		public float progress;
		public Color32[] pixelBuffer;

		ColormapPrecision precision;
		Color32[] palette;
		bool[] usedColors;
		int numColors;
		System.Action doneCallback;
		int colorsteps;
		int totalPixels;
		Color32 colorProgress;



		public ColormapCalculator(ColormapPrecision precision, Color32[] palette, bool[] usedColors, int numColors, System.Action doneCallback)
		{
			this.precision = precision;
			this.palette = palette;
			this.usedColors = usedColors;
			this.doneCallback = doneCallback;
			this.numColors = numColors;
			progress = 0;
			colorProgress = new Color32(0, 0, 0, 0);
			SetupPixelBuffer();
		}


		void SetupPixelBuffer()
		{
			colorsteps = ColormapUtils.GetPrecisionColorsteps(precision);
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

		private void gotoNextPixel()
		{
			++colorProgress.r;

			if (colorProgress.r == colorsteps)
			{
				colorProgress.r = 0;
				++colorProgress.g;

				if (colorProgress.g == colorsteps)
				{
					colorProgress.g = 0;
					++colorProgress.b;
				}
			}
		}

		private int getPixelProgress()
		{
			return colorProgress.r + colorProgress.g * colorsteps + colorProgress.b * colorsteps * colorsteps;
		}


		void CalculateNextPixel()
		{
			int pixelProgress = getPixelProgress();

			if (pixelProgress < totalPixels)
			{
				byte paletteIndex = GetClosestPaletteIndex();
				pixelBuffer[pixelProgress] = new Color32(0, 0, 0, paletteIndex);
				gotoNextPixel();
				progress = (float)pixelProgress / (float)totalPixels;
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
			Vector3 rgb = new Vector3(colorProgress.r, colorProgress.g, colorProgress.b);
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


