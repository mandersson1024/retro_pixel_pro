using UnityEngine;
using System.Collections;


namespace AlpacaSound.RetroPixelPro
{

	public class ColormapUtils
	{

		public static int GetPrecisionColorsteps(ColormapPrecision precision)
		{
			switch (precision)
			{
			case ColormapPrecision.Low: return 16;
			case ColormapPrecision.Medium: return 32;
			case ColormapPrecision.High: return 64;
			case ColormapPrecision.Overkill: return 128;
			case ColormapPrecision.StupidOverkill: return 256;
			default: return 16;
			}
		}


	}
}
