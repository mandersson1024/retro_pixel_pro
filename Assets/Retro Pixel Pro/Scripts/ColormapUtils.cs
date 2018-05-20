using UnityEngine;
using System.Collections;


namespace AlpacaSound.RetroPixelPro
{

	public class ColormapUtils
	{

		public static int GetColormapSize3D(bool preview)
		{
			if (preview)
			{
				return 16;
			}
			else
			{
				return 64;
			}
		}


		public static int GetColormapSize2D(bool preview)
		{
			if (preview)
			{
				return 64;
			}
			else
			{
				return 512;
			}
		}


		static public Color32 IndexToColor(int colorsteps, int index)
		{
			int stepLength = 256 / colorsteps;

			int r = stepLength * (index % colorsteps);
			int g = stepLength * ((index / colorsteps) % colorsteps);
			int b = stepLength * (index / (colorsteps * colorsteps));

			return new Color32((byte)r, (byte)g, (byte)b, 255);
		}



		static public Vector3 GetColorstepPosition(Color32 color, int colorsteps)
		{
			int stepLength = 256 / colorsteps;
			float offset = stepLength / 2f;

			Vector3 v = new Vector3
			{
				x = ((color.r / stepLength) * stepLength + offset) / 256f,
				y = ((color.g / stepLength) * stepLength + offset) / 256f,
				z = ((color.b / stepLength) * stepLength + offset) / 256f
			};

			return v;
		}


		static public Vector3 FindNearestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 otherPoint)
		{
			return Vector3.zero;
		}


	}
}
