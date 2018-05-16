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
			int r = index % colorsteps;
			int g = (index / colorsteps) % colorsteps;
			int b = index / (colorsteps * colorsteps);

			return new Color32((byte)r, (byte)g, (byte)b, 255);
		}

	}
}
