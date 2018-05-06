using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using AlpacaSound.RetroPixelPro;

public class ColormapCalculatorTest
{
    static Color32 BLACK = new Color32(0, 0, 0, 255);
    static Color32 WHITE = new Color32(255, 255, 255, 255);

    static Color32 GRAY = new Color32(127, 127, 127, 255);
    static Color32 GRAY_DARK = new Color32(63, 63, 63, 255);
    static Color32 GRAY_LIGHT = new Color32(191, 191, 191, 255);

    static Color32 RED = new Color32(255, 0, 0, 255);
    static Color32 GREEN = new Color32(0, 255, 0, 255);
    static Color32 BLUE = new Color32(0, 0, 255, 255);

    static Color32 YELLOW = new Color32(255, 255, 0, 255);
    static Color32 MAGENTA = new Color32(255, 0, 255, 255);
    static Color32 CYAN = new Color32(0, 255, 255, 255);


    [Test]
    public void Test_GetClosestPaletteIndex()
    {
        Assert.AreEqual(-1, ColormapCalculator.GetClosestPaletteIndex(WHITE, new Color32[] { }));
        Assert.AreEqual(0, ColormapCalculator.GetClosestPaletteIndex(WHITE, new Color32[] { WHITE, BLACK, RED, GREEN, BLUE }));
        Assert.AreEqual(0, ColormapCalculator.GetClosestPaletteIndex(GRAY_LIGHT, new Color32[] { WHITE, BLACK, RED, GREEN, BLUE }));
        Assert.AreEqual(1, ColormapCalculator.GetClosestPaletteIndex(GRAY_DARK, new Color32[] { WHITE, BLACK, RED, GREEN, BLUE }));
        Assert.AreEqual(2, ColormapCalculator.GetClosestPaletteIndex(new Color(0.8f, 0.1f, 0.1f), new Color32[] { WHITE, BLACK, RED, GREEN, BLUE }));
    }

    [Test]
    public void Test_ColormapValueToColor32()
    {
        Assert.AreEqual(new Color32(0, 0, 0, 255), (new ColormapValue(0, 0, 0).ToColor32()));
        Assert.AreEqual(new Color32(1, 1, 0, 255), (new ColormapValue(1, 1, 0).ToColor32()));
        Assert.AreEqual(new Color32(0, 0, 255, 255), (new ColormapValue(0, 0, 1).ToColor32()));
        Assert.AreEqual(new Color32(0, 0, 127, 255), (new ColormapValue(0, 0, 0.5f).ToColor32()));
    }

    [Test]
    public void Test_CalculateColormapValue()
    {
        ColormapValue result = ColormapCalculator.CalculateColormapValue(BLACK, new Color32[] { BLACK, WHITE });
        Assert.AreEqual(0, result.primaryPaletteIndex);
        Assert.AreEqual(0, result.secondaryPaletteIndex);
        Assert.AreEqual(0, result.blend);
    }

}
