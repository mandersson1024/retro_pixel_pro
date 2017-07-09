using UnityEngine;
using System.Collections;


namespace AlpacaSound.RetroPixelPro
{


	public class PalettePresets
	{

		[System.Serializable]
		public enum PresetName
		{
			Classic1,
			Classic2,
			Classic3,
			Classic4,
			Classic5,
			Classic6,
			Grayscale2Bit,
			Grayscale4Bit,
			Grayscale8Bit,
			MonochromeGreen,
			MonochromeBrown,
			BlackAndWhite,
			//Custom,
		}


		static public void SetPalette(PresetName preset, Colormap target)
		{
			switch (preset)
			{
			case PresetName.Classic1:          _Amstrad(target);                break;
			case PresetName.Classic2:          _AppleII(target);                break;
			case PresetName.Classic3:          _C64(target);                    break;
			case PresetName.Classic4:          _VIC20(target);                  break;
			case PresetName.Classic5:          _ZXSpectrum(target);             break;
			case PresetName.Classic6:          _CGA(target);                    break;
			case PresetName.Grayscale2Bit:     _Grayscale(target, 4);           break;
			case PresetName.Grayscale4Bit:     _Grayscale(target, 16);          break;
			case PresetName.Grayscale8Bit:     _Grayscale(target, 256);         break;
			case PresetName.MonochromeGreen:   _MonochromeGreen(target, true);  break;
			case PresetName.MonochromeBrown:   _MonochromeBrown(target, true);  break;
			case PresetName.BlackAndWhite:     _BlackAndWhite(target);          break;
			//case PresetName.Custom:            _Custom(target);                 break;
			default:                           _Custom(target);                 break;
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


		static Color rgb(byte r, byte g, byte b)
		{
			return new Color32(r, g, b, 255);
		}


        static void _Custom (Colormap target)
		{
			// do nothing
		}


        static void _AppleII (Colormap target)
		{
			SetColorCount(target, 16);
			target.palette[0]  = rgb(0,0,0);
			target.palette[1]  = rgb(107,39,65);
			target.palette[2]  = rgb(64,50,119);
			target.palette[3]  = rgb(212,39,240);
			target.palette[4]  = rgb(26,88,64);
			target.palette[5]  = rgb(126,126,126);
			target.palette[6]  = rgb(45,146,239);
			target.palette[7]  = rgb(189,175,247);
			target.palette[8]  = rgb(65,77,11);
			target.palette[9]  = rgb(213,102,20);
			target.palette[10] = rgb(126,126,126);
			target.palette[11] = rgb(234,164,189);
			target.palette[12] = rgb(43,202,0);
			target.palette[13] = rgb(189,203,135);
			target.palette[14] = rgb(147,216,189);
			target.palette[15] = rgb(255,255,255);
		}


        static void _VIC20 (Colormap target)
		{
			SetColorCount(target, 16);
			target.palette[0] = rgb(0,0,0);
			target.palette[1] = rgb(255,255,255);
			target.palette[2] = rgb(162,15,29);
			target.palette[3] = rgb(75,233,248);
			target.palette[4] = rgb(157,34,244);
			target.palette[5] = rgb(67,220,52);
			target.palette[6] = rgb(23,28,247);
			target.palette[7] = rgb(209,204,41);
			target.palette[8] = rgb(186,63,19);
			target.palette[9] = rgb(225,159,100);
			target.palette[10] = rgb(220,124,130);
			target.palette[11] = rgb(143,247,251);
			target.palette[12] = rgb(214,138,253);
			target.palette[13] = rgb(130,225,132);
			target.palette[14] = rgb(112,124,251);
			target.palette[15] = rgb(223,215,120);
		}


        static void _ZXSpectrum (Colormap target)
		{
			SetColorCount(target, 15);
			target.palette[0] = rgb(0,0,0);
			target.palette[1] = rgb(0,0,189);
			target.palette[2] = rgb(190,0,0);
			target.palette[3] = rgb(187,0,190);
			target.palette[4] = rgb(26,196,1);
			target.palette[5] = rgb(0,193,191);
			target.palette[6] = rgb(191,193,4);
			target.palette[7] = rgb(190,190,190);
			target.palette[8] = rgb(0,0,255);
			target.palette[9] = rgb(251,0,5);
			target.palette[10] = rgb(252,0,255);
			target.palette[11] = rgb(14,255,0);
			target.palette[12] = rgb(33,255,255);
			target.palette[13] = rgb(255,255,9);
			target.palette[14] = rgb(255,255,255);
		}


        static void _Amstrad (Colormap target)
		{
			SetColorCount(target, 27);

			target.palette[0] = rgb(0,0,0);
			target.palette[1] = rgb(0,0,126);
			target.palette[2] = rgb(0,0,255);
			target.palette[3] = rgb(124,0,0);
			target.palette[4] = rgb(124,0,126);
			target.palette[5] = rgb(125,0,255);
			target.palette[6] = rgb(255,0,0);
			target.palette[7] = rgb(255,0,127);
			target.palette[8] = rgb(254,0,255);

			target.palette[9] = rgb(6,132,0);
			target.palette[10] = rgb(16,128,126);
			target.palette[11] = rgb(13,122,255);
			target.palette[12] = rgb(127,129,0);
			target.palette[13] = rgb(126,126,126);
			target.palette[14] = rgb(125,116,255);
			target.palette[15] = rgb(253,125,6);
			target.palette[16] = rgb(253,123,127);
			target.palette[17] = rgb(254,112,255);

			target.palette[18] = rgb(34,255,3);
			target.palette[19] = rgb(11,255,122);
			target.palette[20] = rgb(33,255,255);
			target.palette[21] = rgb(131,255,6);
			target.palette[22] = rgb(132,255,127);
			target.palette[23] = rgb(131,255,255);
			target.palette[24] = rgb(255,255,9);
			target.palette[25] = rgb(255,255,120);
			target.palette[26] = rgb(255,255,255);
		}


        static void _CGA (Colormap target)
		{
			SetColorCount(target, 16);
			target.palette[0] = rgb(0,0,0);
			target.palette[1] = rgb(0,0,168);
			target.palette[2] = rgb(22,173,2);
			target.palette[3] = rgb(21,171,168);
			target.palette[4] = rgb(166,0,1);
			target.palette[5] = rgb(166,0,169);
			target.palette[6] = rgb(169,85,0);
			target.palette[7] = rgb(168,168,168);
			target.palette[8] = rgb(85,85,85);
			target.palette[9] = rgb(84,74,254);
			target.palette[10] = rgb(93,255,85);
			target.palette[11] = rgb(85,255,255);
			target.palette[12] = rgb(253,79,85);
			target.palette[13] = rgb(252,67,254);
			target.palette[14] = rgb(255,255,73);
			target.palette[15] = rgb(255,255,255);
		}


        static void _C64(Colormap target)
		{
			SetColorCount(target, 16);
			target.palette[0]  = Color.black;
			target.palette[1]  = Color.white;
			target.palette[2]  = rgb(136, 0, 0);
			target.palette[3]  = rgb(170, 255, 238);
			target.palette[4]  = rgb(204, 68, 204);
			target.palette[5]  = rgb(0, 204, 85);
			target.palette[6]  = rgb(0, 0, 170);
			target.palette[7]  = rgb(238, 238, 119);
			target.palette[8]  = rgb(221, 136, 85);
			target.palette[9]  = rgb(102, 68, 0);
			target.palette[10] = rgb(255, 119, 119);
			target.palette[11] = rgb(51, 51, 51);
			target.palette[12] = rgb(119, 119, 119);
			target.palette[13] = rgb(170, 255, 102);
			target.palette[14] = rgb(0, 136, 255);
			target.palette[15] = rgb(187, 187, 187);
		}


        static void _MonochromeGreen (Colormap target, bool black)
		{
			SetColorCount(target, 2);
			target.palette[0]  = rgb(16,36,13);
			target.palette[1]  = rgb(66,216,41);
		}
		

        static void _MonochromeBrown (Colormap target, bool black)
		{
			SetColorCount(target, 2);
			target.palette[0]  = rgb(47,32,16);
			target.palette[1]  = rgb(231,146,64);
		}


        static void _BlackAndWhite (Colormap target)
		{
			SetColorCount(target, 2);
			target.palette[0]  = rgb(33,33,33);
			target.palette[1]  = rgb(242,242,242);
		}		


        static void _Grayscale(Colormap target, int numColors)
		{
			SetColorCount(target, numColors);
			for (int i = 0; i < numColors; ++i)
			{
				float v = (float) i / (numColors-1);
				target.palette[i] = new Color(v, v, v);
			}
		}
		
	}

}
