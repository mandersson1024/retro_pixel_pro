﻿using UnityEngine;
using System.Collections;


namespace AlpacaSound.RetroPixelPro
{

    public class PalettePresets
    {

        public enum PresetName
        {
            Amstrad,
            AppleII,
            VIC20,
            C64,
            ZXSpectrum,
            CGA,
            MonochromeGreen,
            MonochromeBrown,
            MonochromeBlue,
            MonochromeBlackWhite,
        }


        static public string GetPresetNameString(PresetName preset)
        {
            switch (preset)
            {
                case PresetName.Amstrad: return "Colour Computer 1984";
                case PresetName.AppleII: return "The Big 1977";
                case PresetName.VIC20: return "Breadbox 1982";
                case PresetName.C64: return "Breadbox 1980";
                case PresetName.ZXSpectrum: return "Sir Clive";
                case PresetName.CGA: return "Adapter 1981";
                case PresetName.MonochromeGreen: return "Monochrome Green";
                case PresetName.MonochromeBrown: return "Monochrome Brown";
                case PresetName.MonochromeBlue: return "Monochrome Blue";
                case PresetName.MonochromeBlackWhite: return "Monochrome Black White";
                default: return null;
            }
        }


        static public Color32[] GetPresetPalette(PresetName presetName)
        {
            switch (presetName)
            {
                case PresetName.Amstrad: return _Amstrad();
                case PresetName.AppleII: return _AppleII();
                case PresetName.VIC20: return _C64();
                case PresetName.C64: return _VIC20();
                case PresetName.ZXSpectrum: return _ZXSpectrum();
                case PresetName.CGA: return _CGA();
                case PresetName.MonochromeGreen: return _MonochromeGreen();
                case PresetName.MonochromeBrown: return _MonochromeBrown();
                case PresetName.MonochromeBlue: return _MonochromeBlue();
                case PresetName.MonochromeBlackWhite: return _BlackAndWhite();
                default: return null;
            }
        }


        static void SetColorCount(Colormap target, int count)
        {
            target.numberOfColors = count;

            for (int i = 0; i < 256; ++i)
            {
                target.usedColors[i] = i < count;
            }
        }


        static Color32 rgb(byte r, byte g, byte b)
        {
            return new Color32(r, g, b, 255);
        }


        static Color32[] _AppleII()
        {
            return new Color32[]
            {
                rgb(0,0,0),
                rgb(107, 39, 65),
                rgb(64, 50, 119),
                rgb(212, 39, 240),
                rgb(26, 88, 64),
                rgb(126, 126, 126),
                rgb(45, 146, 239),
                rgb(189, 175, 247),
                rgb(65, 77, 11),
                rgb(213, 102, 20),
                rgb(126, 126, 126),
                rgb(234, 164, 189),
                rgb(43, 202, 0),
                rgb(189, 203, 135),
                rgb(147, 216, 189),
                rgb(255, 255, 255),
            };
        }


        static Color32[] _VIC20()
        {
            return new Color32[]
            {
                rgb(0,0,0),
                rgb(255, 255, 255),
                rgb(162, 15, 29),
                rgb(75, 233, 248),
                rgb(157, 34, 244),
                rgb(67, 220, 52),
                rgb(23, 28, 247),
                rgb(209, 204, 41),
                rgb(186, 63, 19),
                rgb(225, 159, 100),
                rgb(220, 124, 130),
                rgb(143, 247, 251),
                rgb(214, 138, 253),
                rgb(130, 225, 132),
                rgb(112, 124, 251),
                rgb(223, 215, 120),
            };
        }


        static Color32[] _ZXSpectrum()
        {
            return new Color32[]
            {
                rgb(0,0,0),
                rgb(0,0,189),
                rgb(190,0,0),
                rgb(187,0,190),
                rgb(26,196,1),
                rgb(0,193,191),
                rgb(191,193,4),
                rgb(190,190,190),
                rgb(0,0,255),
                rgb(251,0,5),
                rgb(252,0,255),
                rgb(14,255,0),
                rgb(33,255,255),
                rgb(255,255,9),
                rgb(255,255,255),
            };
        }


        static Color32[] _Amstrad()
        {
            return new Color32[]
            {
                rgb(0,0,0),
                rgb(0,0,126),
                rgb(0,0,255),
                rgb(124,0,0),
                rgb(124,0,126),
                rgb(125,0,255),
                rgb(255,0,0),
                rgb(255,0,127),
                rgb(254,0,255),

                rgb(6,132,0),
                rgb(16,128,126),
                rgb(13,122,255),
                rgb(127,129,0),
                rgb(126,126,126),
                rgb(125,116,255),
                rgb(253,125,6),
                rgb(253,123,127),
                rgb(254,112,255),

                rgb(34,255,3),
                rgb(11,255,122),
                rgb(33,255,255),
                rgb(131,255,6),
                rgb(132,255,127),
                rgb(131,255,255),
                rgb(255,255,9),
                rgb(255,255,120),
                rgb(255,255,255),
            };
        }


        static Color32[] _CGA()
        {
            return new Color32[]
            {
                rgb(0,0,0),
                rgb(0,0,168),
                rgb(22,173,2),
                rgb(21,171,168),
                rgb(166,0,1),
                rgb(166,0,169),
                rgb(169,85,0),
                rgb(168,168,168),
                rgb(85,85,85),
                rgb(84,74,254),
                rgb(93,255,85),
                rgb(85,255,255),
                rgb(253,79,85),
                rgb(252,67,254),
                rgb(255,255,73),
                rgb(255,255,255),
            };
        }


        static Color32[] _C64()
        {
            return new Color32[]
            {
                Color.black,
                Color.white,
                rgb(136, 0, 0),
                rgb(170, 255, 238),
                rgb(204, 68, 204),
                rgb(0, 204, 85),
                rgb(0, 0, 170),
                rgb(238, 238, 119),
                rgb(221, 136, 85),
                rgb(102, 68, 0),
                rgb(255, 119, 119),
                rgb(51, 51, 51),
                rgb(119, 119, 119),
                rgb(170, 255, 102),
                rgb(0, 136, 255),
                rgb(187, 187, 187),
            };
        }


        static Color32[] _MonochromeGreen()
        {
            return new Color32[]
            {
                rgb(16,36,13),
                rgb(66,216,41),
            };
        }


        static Color32[] _MonochromeBrown()
        {
            return new Color32[]
            {
                rgb(47, 32, 16),
                rgb(231, 146, 64),
            };
        }


        static Color32[] _MonochromeBlue()
        {
            return new Color32[]
            {
                rgb(0, 0, 170),
                rgb(0, 136, 255),
            };
        }


        static Color32[] _BlackAndWhite()
        {
            return new Color32[]
            {
                rgb(33,33,33),
                rgb(242,242,242),
            };
        }

    }

}
