using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlpacaSound.RetroPixelPro
{

	public struct ColormapValue
	{
		public int paletteIndex1;
		public int paletteIndex2;
		public float blend;

		public Color32 ToRGB()
		{
			byte r = (byte)paletteIndex1;
			byte g = (byte)paletteIndex2;
			byte b = (byte)(blend * 255);

			return new Color32(r, g, b, 255);
		}
	}

}
