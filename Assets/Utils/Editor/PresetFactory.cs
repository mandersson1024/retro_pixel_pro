using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace AlpacaSound.RetroPixelPro
{

	public class PresetFactory : Editor
	{

        [MenuItem("Retro Pixel Pro Utils/Generate Fixed Presets")]
		static void GenerateFixedPresets()
		{
			foreach (PalettePresets.PresetName preset in PalettePresets.PresetName.GetValues(typeof(PalettePresets.PresetName)))
			{
				GenerateFixedPreset(preset);
			}

			AssetDatabase.SaveAssets();
		}

        static void GenerateFixedPreset(PalettePresets.PresetName presetName)
        {
            ColormapPreset preset = CreateInstance<ColormapPreset>();
            preset.palette = PalettePresets.GetPresetPalette(presetName);

            string path = FileUtils.PRESETS_DIRECTORY_PATH;

            if (preset.palette.Length == 2)
            {
                path += "Monochrome/";
            }
            else
            {
                path += "Classic Computers/";
            }

            path += presetName + ".asset";
            AssetDatabase.CreateAsset(preset, path);

            Debug.Log("Created preset: " + presetName);
        }

        [MenuItem("Retro Pixel Pro Utils/Generate Gradient Presets")]
        static void GenerateGradientPresets()
        {
            GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0xffffff), "BlackToWhite", 16);

            GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0xff0000), "BlackToRed", 16);
            GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0x00ff00), "BlackToGreen", 16);
            GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0x0000ff), "BlackToBlue", 16);
            GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0xffff00), "BlackToYellow", 16);
            GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0xff00ff), "BlackToMagenta", 16);
            GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0x00ffff), "BlackToCyan", 16);

            GenerateGradientPreset(ColorModel.RGB, Hex(0xff0000), Hex(0xffffff), "RedToWhtie", 16);
            GenerateGradientPreset(ColorModel.RGB, Hex(0x00ff00), Hex(0xffffff), "GreenToWhtie", 16);
            GenerateGradientPreset(ColorModel.RGB, Hex(0x0000ff), Hex(0xffffff), "BlueToWhtie", 16);
            GenerateGradientPreset(ColorModel.RGB, Hex(0xffff00), Hex(0xffffff), "YellowToWhtie", 16);
            GenerateGradientPreset(ColorModel.RGB, Hex(0xff00ff), Hex(0xffffff), "MagentaToWhtie", 16);
            GenerateGradientPreset(ColorModel.RGB, Hex(0x00ffff), Hex(0xffffff), "CyanToWhtie", 16);

            GenerateGradientPreset(ColorModel.RGB, Hex(0xff0000), Hex(0xffff00), "RedToYellow", 16);

            GenerateGradientPreset(ColorModel.HSV, new Color(0, 1, 1), new Color(0.75f, 1, 1), "Rainbow", 16);

            AssetDatabase.SaveAssets();
        }


        static Color Hex(int hex)
        {
            int r = (hex & 0xff0000) / 0xffff;
            int g = (hex & 0x00ff00) / 0xff;
            int b = hex & 0x0000ff;

            return new Color(r / 256f, g / 256f, b / 256f);
        }


        static void GenerateGradientPreset(ColorModel colorModel, Color start, Color end, string name, int numColors)
        {
            ColormapPreset preset = CreateInstance<ColormapPreset>();
            preset.SetNumColors(numColors);
            //preset.numberOfColors = numColors;

            //List<Color32> colors = new List<Color32>();

            for (int i = 0; i < numColors; ++i)
            {
                float t = (float) i / ((float) numColors - 1.0f);
                Color color = Color.Lerp(start, end, t);

                if (colorModel == ColorModel.HSV)
                {
                    color = Color.HSVToRGB(color.r, color.g, color.b);
                }

                preset.palette[i] = color;
            }

            //preset.SetColors(colors);

            name += " " + numColors;
            string path = FileUtils.PRESETS_DIRECTORY_PATH + "Gradients/" + name + ".asset";

            AssetDatabase.CreateAsset(preset, path);

            Debug.Log("Created preset: " + name);
        }

    }

    enum ColorModel
    {
        RGB,
        HSV,
    }
}

