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
		ColormapCalculatorProgress calculatorProgress;


		public ColormapCalculator(bool preview, Color32[] palette, bool[] usedColors, int numColors, System.Action doneCallback)
		{
			this.palette = palette;
			this.usedColors = usedColors;
			this.doneCallback = doneCallback;
			this.numColors = numColors;
			progress = 0;
			colorsteps = ColormapUtils.GetColormapSize3D(preview);
			calculatorProgress = new ColormapCalculatorProgress(colorsteps);
			totalPixels = colorsteps * colorsteps * colorsteps;
			pixelBuffer = new Color32[totalPixels];
		}


		public void CalculateChunk()
		{
			double frameStartTime = EditorApplication.timeSinceStartup;

			while (EditorApplication.timeSinceStartup < frameStartTime + (1.0 / 30.0))
			{
				if (calculatorProgress.IsDone())
				{
					doneCallback.Invoke();
					break;
				}
				else
				{
					CalculateNextPixel();
				}
			}
		}


		void CalculateNextPixel()
		{
			byte paletteIndex = GetClosestPaletteIndex();
			pixelBuffer[calculatorProgress.progress] = new Color32(0, 0, 0, paletteIndex);
			calculatorProgress.NextPixel();
			progress = (float)calculatorProgress.progress / (float)totalPixels;
		}


		byte GetClosestPaletteIndex()
		{
			float closestDistance = float.MaxValue;
			int closestIndex = 0;
			Vector3 rgb = calculatorProgress.GetRGBCoordinate();
			rgb = 256 * rgb;

			for (int i = 0; i < numColors; ++i)
			{
				if (usedColors[i])
				{
					Vector3 paletteRGB = new Vector3(palette[i].r + 0.5f, palette[i].g + 0.5f, palette[i].b + 0.5f);
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


