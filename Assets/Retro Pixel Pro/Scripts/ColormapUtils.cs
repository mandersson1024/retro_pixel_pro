using UnityEngine;
using System.Collections;


namespace AlpacaSound.RetroPixelPro
{

	public class ColormapUtils
	{

		public static int GetNumColorsteps(bool preview)
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


	}
}
