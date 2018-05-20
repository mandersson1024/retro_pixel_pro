using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

using AlpacaSound.RetroPixelPro;

public class ColormapValueTest
{

	[Test]
	public void NewTestScriptSimplePasses()
	{
		Assert.AreEqual(new Vector3(0.5f, 0, 0), ColormapUtils.FindNearestPointOnLine(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0.5f, 1, 0)));
	}
}
