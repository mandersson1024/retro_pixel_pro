using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

using AlpacaSound.RetroPixelPro;

public class ColormapCalculatorProgressTest
{

	[Test]
	public void TestIndexToColor2()
	{
		Assert.AreEqual(new Color32(0, 0, 0, 0), ColormapUtils.IndexToColor(2, 0));
		Assert.AreEqual(new Color32(255, 0, 0, 0), ColormapUtils.IndexToColor(2, 1));
		Assert.AreEqual(new Color32(0, 255, 0, 0), ColormapUtils.IndexToColor(2, 2));
		Assert.AreEqual(new Color32(255, 255, 0, 0), ColormapUtils.IndexToColor(2, 3));
		Assert.AreEqual(new Color32(0, 0, 255, 0), ColormapUtils.IndexToColor(2, 4));
		Assert.AreEqual(new Color32(255, 0, 255, 0), ColormapUtils.IndexToColor(2, 5));
		Assert.AreEqual(new Color32(0, 255, 255, 0), ColormapUtils.IndexToColor(2, 6));
		Assert.AreEqual(new Color32(255, 255, 255, 0), ColormapUtils.IndexToColor(2, 7));
	}

	[Test]
	public void TestIndexToColor4()
	{
		Assert.AreEqual(new Color32(0, 0, 0, 0), ColormapUtils.IndexToColor(4, 0));
		Assert.AreEqual(new Color32(85, 0, 0, 0), ColormapUtils.IndexToColor(4, 1));
		Assert.AreEqual(new Color32(170, 0, 0, 0), ColormapUtils.IndexToColor(4, 2));
		Assert.AreEqual(new Color32(255, 0, 0, 0), ColormapUtils.IndexToColor(4, 3));
		Assert.AreEqual(new Color32(255, 255, 255, 0), ColormapUtils.IndexToColor(4, 63));
	}

}
