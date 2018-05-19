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
		Vector3 rgb;

		rgb = ColormapUtils.GetColorstepPosition(new Color32(0, 0, 0, 0), 1);
		Assert.AreEqual(0.5f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.5f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.5f, rgb.z, Mathf.Epsilon);

		rgb = ColormapUtils.GetColorstepPosition(new Color32(10, 10, 10, 0), 1);
		Assert.AreEqual(0.5f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.5f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.5f, rgb.z, Mathf.Epsilon);

		rgb = ColormapUtils.GetColorstepPosition(new Color32(245, 245, 245, 0), 1);
		Assert.AreEqual(0.5f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.5f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.5f, rgb.z, Mathf.Epsilon);

		rgb = ColormapUtils.GetColorstepPosition(new Color32(255, 255, 255, 0), 1);
		Assert.AreEqual(0.5f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.5f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.5f, rgb.z, Mathf.Epsilon);
	}


	[Test]
	public void TestColorsteps2()
	{
		Vector3 rgb;

		rgb = ColormapUtils.GetColorstepPosition(new Color32(0, 0, 0, 0), 2);
		Assert.AreEqual(0.25f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.25f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.25f, rgb.z, Mathf.Epsilon);

		rgb = ColormapUtils.GetColorstepPosition(new Color32(10, 10, 10, 0), 2);
		Assert.AreEqual(0.25f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.25f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.25f, rgb.z, Mathf.Epsilon);

		rgb = ColormapUtils.GetColorstepPosition(new Color32(245, 245, 245, 0), 2);
		Assert.AreEqual(0.75f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.75f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.75f, rgb.z, Mathf.Epsilon);

		rgb = ColormapUtils.GetColorstepPosition(new Color32(255, 255, 255, 0), 2);
		Assert.AreEqual(0.75f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.75f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.75f, rgb.z, Mathf.Epsilon);
	}


	[Test]
	public void TestColorsteps256()
	{
		Vector3 rgb;

		rgb = ColormapUtils.GetColorstepPosition(new Color32(0, 0, 0, 0), 256);
		Assert.AreEqual(0.5f / 256f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(0.5f / 256f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(0.5f / 256f, rgb.z, Mathf.Epsilon);

		rgb = ColormapUtils.GetColorstepPosition(new Color32(255, 255, 255, 0), 256);
		Assert.AreEqual(1f - 0.5f / 256f, rgb.x, Mathf.Epsilon);
		Assert.AreEqual(1f - 0.5f / 256f, rgb.y, Mathf.Epsilon);
		Assert.AreEqual(1f - 0.5f / 256f, rgb.z, Mathf.Epsilon);
	}


}
