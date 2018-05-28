using UnityEngine;
using System.Collections;
using UnityEditor;

using UnityEngine.Assertions;

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
		Vector3[] paletteRGBCoordinates;
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

			paletteRGBCoordinates = new Vector3[palette.Length];
			for (int i = 0; i < palette.Length; ++i)
			{
				if (usedColors[i])
				{
					Color32 paletteColor = palette[i];
					paletteRGBCoordinates[i] = ColormapUtils.ColorToVector(paletteColor);
				}
			}
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
			ColormapValue value = CalculateColormapValue();
			pixelBuffer[calculatorProgress.progress] = value.ToRGB();
			calculatorProgress.NextPixel();
			progress = (float)calculatorProgress.progress / (float)totalPixels;
		}


		byte GetClosestPaletteIndex()
		{
			Vector3 sourceRGB = ColormapUtils.ColorToVector(calculatorProgress.color);
			float closestDistance = float.MaxValue;
			int closestIndex = 0;

			for (int i = 0; i < numColors; ++i)
			{
				if (usedColors[i])
				{
					Vector3 paletteRGB = paletteRGBCoordinates[i];
					float distance = Vector3.Distance(sourceRGB, paletteRGB);

					if (distance < closestDistance)
					{
						closestDistance = distance;
						closestIndex = i;
					}
				}
			}

			return (byte)closestIndex;
		}


		ColormapValue CalculateColormapValue()
		{
			Vector3 sourceRGB = ColormapUtils.ColorToVector(calculatorProgress.color);
			byte mainPaletteIndex = GetClosestPaletteIndex();

			if (palette[mainPaletteIndex].Equals(calculatorProgress.color))
			{
				return new ColormapValue
				{
					paletteIndex1 = mainPaletteIndex,
					paletteIndex2 = mainPaletteIndex,
					blend = 0
				};
			}

			Vector3 mainRGB = paletteRGBCoordinates[mainPaletteIndex];

			float closestDistance = float.MaxValue;
			int closestIndex = -1;
			float closestSegmentLength = float.MaxValue;
			float blend = 0;

			for (int i = 0; i < numColors; ++i)
			{
				if (usedColors[i] && i != mainPaletteIndex)
				{
					Vector3 paletteRGB = paletteRGBCoordinates[i];
					Vector3 projected = ColormapUtils.ProjectPointOnLine(mainRGB, paletteRGB, sourceRGB);

					if (ColormapUtils.PointIsInsideSegment(mainRGB, paletteRGB, projected))
					{
						float distance = Vector3.Distance(sourceRGB, projected);
						float segmentLength = Vector3.Distance(mainRGB, paletteRGB);

						if (segmentLength < closestSegmentLength)
						{
							closestDistance = distance;
							closestSegmentLength = segmentLength;
							closestIndex = i;
							blend = Mathf.Approximately(segmentLength, 0) ? 0 : Vector3.Distance(projected, mainRGB) / segmentLength;
						}
						else if (Mathf.Approximately(distance, closestDistance))
						{
							if (distance < closestDistance)
							{
								closestDistance = distance;
								closestSegmentLength = segmentLength;
								closestIndex = i;
								blend = Mathf.Approximately(segmentLength, 0) ? 0 : Vector3.Distance(projected, mainRGB) / segmentLength;
							}
						}
					}
				}
			}

			if (closestIndex == -1)
			{
				closestIndex = mainPaletteIndex;
				blend = 0;
			}

			blend = Mathf.Clamp(blend, 0, 0.5f);

			ColormapValue value = new ColormapValue
			{
				paletteIndex1 = mainPaletteIndex,
				paletteIndex2 = closestIndex,
				blend = blend,
			};

			/*
			if (calculatorProgress.color.r == calculatorProgress.color.g && calculatorProgress.color.b == calculatorProgress.color.g)
			{
				Debug.Log("color=" + calculatorProgress.color + ", index1=" + value.paletteIndex1 + ", index2=" + value.paletteIndex2 + ", blend=" + value.blend);
			}
			*/

			return value;
		}

	}

}


