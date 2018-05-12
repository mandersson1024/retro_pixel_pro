using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

using AlpacaSound.RetroPixelPro;

public class ColormapCalculatorProgressTest
{

	[Test]
	public void TestColorsteps1()
	{
		ColormapCalculatorProgress progress = new ColormapCalculatorProgress(1);
		progress.color = new Color32(0, 0, 0, 0);
		Vector3 rgb = progress.GetRGBCoordinate();
		Assert.AreEqual(0.5f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.5f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.5f, rgb.z, Mathf.Epsilon);
	}


	[Test]
	public void TestColorsteps2()
	{
		Vector3 rgb;

		ColormapCalculatorProgress progress = new ColormapCalculatorProgress(2);
		progress.color = new Color32(0, 0, 0, 0);
		rgb = progress.GetRGBCoordinate();
		Assert.AreEqual(0.25f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.25f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.25f, rgb.z, Mathf.Epsilon);

		progress.color = new Color32(1, 0, 0, 0);
		rgb = progress.GetRGBCoordinate();
		Assert.AreEqual(0.75f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.25f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.25f, rgb.z, Mathf.Epsilon);

		progress.color = new Color32(0, 1, 0, 0);
		rgb = progress.GetRGBCoordinate();
		Assert.AreEqual(0.25f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.75f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.25f, rgb.z, Mathf.Epsilon);

		progress.color = new Color32(0, 0, 1, 0);
		rgb = progress.GetRGBCoordinate();
		Assert.AreEqual(0.25f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.25f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.75f, rgb.z, Mathf.Epsilon);
	}


}
