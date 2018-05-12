using UnityEngine;
using System.Collections;

namespace AlpacaSound.RetroPixelPro
{

	[System.Serializable]
	public enum ColormapPrecision
	{
		Preview, // 16^3 == 64^2  == 4096,   GPU texture size: 12k
		Normal,  // 64^3 == 512^2 == 262144, GPU texture size: 786k
	}

}

