using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DitherTest
{

    float Threshold(float _Dither, float blend)
    {
        if (_Dither > 0)
        {
            /*
            float slope = 1f / _Dither;
            float y0 = 0.5f - (_Dither / 2f);
            float line = slope * blend - (slope * y0);
            */

            /*
            float slope = 1f / _Dither;
            float y0 = 0.5f - (_Dither / 2f);
            float line = slope * (blend - y0);
            */

            float line = (1f / _Dither) * (blend - 0.5f + (_Dither / 2f));

            return Mathf.Clamp01(line);
        }

        return 0;
    }


    [Test]
    public void Dither_1_0()
    {
        Assert.AreEqual(0.0f, Threshold(1.0f, 0.0f), Mathf.Epsilon);
        Assert.AreEqual(0.1f, Threshold(1.0f, 0.1f), Mathf.Epsilon);
        Assert.AreEqual(0.2f, Threshold(1.0f, 0.2f), Mathf.Epsilon);
        Assert.AreEqual(0.3f, Threshold(1.0f, 0.3f), Mathf.Epsilon);
        Assert.AreEqual(0.4f, Threshold(1.0f, 0.4f), Mathf.Epsilon);
        Assert.AreEqual(0.5f, Threshold(1.0f, 0.5f), Mathf.Epsilon);
    }


    [Test]
    public void Dither_0_5()
    {
        Assert.AreEqual(0.0f, Threshold(0.5f, 0.0f), Mathf.Epsilon);
        Assert.AreEqual(0.0f, Threshold(0.5f, 0.1f), Mathf.Epsilon);
        Assert.AreEqual(0.0f, Threshold(0.5f, 0.2f), Mathf.Epsilon);
        Assert.AreEqual(0.0f, Threshold(0.5f, 0.25f), Mathf.Epsilon);
        Assert.AreEqual(0.25f, Threshold(0.5f, 0.375f), Mathf.Epsilon);
        Assert.AreEqual(0.5f, Threshold(0.5f, 0.5f), Mathf.Epsilon);
    }


    [Test]
    public void Dither_0_25()
    {
        Assert.AreEqual(0.0f, Threshold(0.25f, 0.0f), Mathf.Epsilon);
        Assert.AreEqual(0.0f, Threshold(0.25f, 0.1f), Mathf.Epsilon);
        Assert.AreEqual(0.0f, Threshold(0.25f, 0.2f), Mathf.Epsilon);
        Assert.AreEqual(0.0f, Threshold(0.25f, 0.25f), Mathf.Epsilon);
        Assert.AreEqual(0.0f, Threshold(0.25f, 0.375f), Mathf.Epsilon);
        Assert.AreEqual(0.5f, Threshold(0.25f, 0.5f), Mathf.Epsilon);
    }


}
