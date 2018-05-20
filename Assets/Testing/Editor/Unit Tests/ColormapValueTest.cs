using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

using AlpacaSound.RetroPixelPro;

public class ColormapValueTest
{

	bool VectorEquals(Vector3 v1, Vector3 v2)
	{

		return Mathf.Approximately(v1.x, v2.x) && Mathf.Approximately(v1.y, v2.y) && Mathf.Approximately(v1.y, v2.y);
	}


	[Test]
	public void RedAxisTest()
	{
		Vector3 lineStart = new Vector3(0, 0, 0);
		Vector3 lineEnd = new Vector3(1, 0, 0);

		Assert.AreEqual(new Vector3(0, 0, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(0, 1, 0)));
		Assert.AreEqual(new Vector3(0.5f, 0, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(0.5f, 1, 0)));
		Assert.AreEqual(new Vector3(1, 0, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(1, 1, 0)));

		Assert.AreEqual(new Vector3(0, 0, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(0, 0, 1)));
		Assert.AreEqual(new Vector3(0.5f, 0, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(0.5f, 0, 1)));
		Assert.AreEqual(new Vector3(1, 0, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(1, 0, 1)));

		Assert.AreEqual(new Vector3(0, 0, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(0, 1, 1)));
		Assert.AreEqual(new Vector3(0.5f, 0, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(0.5f, 1, 1)));
		Assert.AreEqual(new Vector3(1, 0, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(1, 1, 1)));
	}

	[Test]
	public void RedGreenDiaginalTest()
	{
		Vector3 lineStart = new Vector3(0, 0, 0);
		Vector3 lineEnd = new Vector3(1, 1, 0);

		Assert.IsTrue(VectorEquals(new Vector3(0.5f, 0.5f, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(0, 1, 0))));
		Assert.IsTrue(VectorEquals(new Vector3(0.25f, 0.25f, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(0, 0.5f, 0))));
		Assert.IsTrue(VectorEquals(new Vector3(0.75f, 0.75f, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(0.5f, 1, 0))));
	}

	[Test]
	public void BlackWhiteDiagonalTest()
	{
		Vector3 lineStart = new Vector3(0, 0, 0);
		Vector3 lineEnd = new Vector3(1, 1, 1);

		Assert.IsTrue(VectorEquals(new Vector3(0, 0, 0), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(0, 0, 0))));
		Assert.IsTrue(VectorEquals(new Vector3(0.4f, 0.4f, 0.4f), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(0.4f, 0.4f, 0.4f))));
		Assert.IsTrue(VectorEquals(new Vector3(1, 1, 1), ColormapUtils.ProjectPointOnLine(lineStart, lineEnd, new Vector3(1, 1, 1))));
	}
}
