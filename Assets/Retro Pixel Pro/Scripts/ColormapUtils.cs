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
				case ColormapPrecision.Preview: return 16;
				case ColormapPrecision.Normal: return 64;
				default: return 64;
			}
		}


	}
}
