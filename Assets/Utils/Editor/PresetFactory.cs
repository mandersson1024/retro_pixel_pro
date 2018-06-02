using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace AlpacaSound.RetroPixelPro
{

    public class PresetFactory : Editor
    {

        [MenuItem("Retro Pixel Pro Utils/Generate Presets")]
        static void GenerateAllPresets()
        {
            //Directory.Delete(FileUtils.PRESETS_DIRECTORY_PATH, true);
            //Directory.CreateDirectory(FileUtils.PRESETS_DIRECTORY_PATH);

            GenerateFixedPresets();
            GenerateGradientPresets();
        }


        //[MenuItem("Retro Pixel Pro Utils/Generate Fixed Presets")]
        static void GenerateFixedPresets()
        {
            RecreateDirectories();

            foreach (PalettePresets.PresetName preset in System.Enum.GetValues(typeof(PalettePresets.PresetName)))
            {
                GenerateFixedPreset(preset);
            }

            AssetDatabase.SaveAssets();
        }


        static void RecreateDirectories()
        {
            Directory.Delete(FileUtils.PRESETS_DIRECTORY_PATH, true);
            Directory.CreateDirectory(FileUtils.PRESETS_DIRECTORY_PATH);
            //Directory.CreateDirectory(FileUtils.PRESETS_DIRECTORY_PATH + "Classic Computers");
            //Directory.CreateDirectory(FileUtils.PRESETS_DIRECTORY_PATH + "Monochrome");
            //Directory.CreateDirectory(FileUtils.PRESETS_DIRECTORY_PATH + "Gradients");
        }


        static void GenerateFixedPreset(PalettePresets.PresetName presetName)
        {
            ColormapPreset preset = CreateInstance<ColormapPreset>();
            preset.palette = PalettePresets.GetPresetPalette(presetName);

            string path = FileUtils.PRESETS_DIRECTORY_PATH;

            path += presetName + ".asset";
            AssetDatabase.CreateAsset(preset, path);

            Debug.Log("Created preset: " + presetName);
        }

        //[MenuItem("Retro Pixel Pro Utils/Generate Gradient Presets")]
        static void GenerateGradientPresets()
        {
            foreach (int numColors in new int[] { 3, 5, 7, 15, 31, 63 })
            {
                GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0x888888), Hex(0xffffff), "Gray", numColors);
                GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0xff0000), Hex(0xffffff), "Red", numColors);
                GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0x00ff00), Hex(0xffffff), "Green", numColors);
                GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0x0000ff), Hex(0xffffff), "Blue", numColors);
                GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0xffff00), Hex(0xffffff), "Yellow", numColors);
                GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0xff00ff), Hex(0xffffff), "Magenta", numColors);
                GenerateGradientPreset(ColorModel.RGB, Hex(0x000000), Hex(0x00ffff), Hex(0xffffff), "Cyan", numColors);

                //GenerateGradientPreset(ColorModel.RGB, Hex(0xff0000), Hex(0xffffff), "RedToWhtie", numColors);
                //GenerateGradientPreset(ColorModel.RGB, Hex(0x00ff00), Hex(0xffffff), "GreenToWhtie", numColors);
                //GenerateGradientPreset(ColorModel.RGB, Hex(0x0000ff), Hex(0xffffff), "BlueToWhtie", numColors);
                //GenerateGradientPreset(ColorModel.RGB, Hex(0xffff00), Hex(0xffffff), "YellowToWhtie", numColors);
                //GenerateGradientPreset(ColorModel.RGB, Hex(0xff00ff), Hex(0xffffff), "MagentaToWhtie", numColors);
                //GenerateGradientPreset(ColorModel.RGB, Hex(0x00ffff), Hex(0xffffff), "CyanToWhtie", numColors);

                //GenerateGradientPreset(ColorModel.HSV, new Color(0, 1, 1), new Color(0.75f, 1, 1), "GradientRainbow", numColors);
            }

            AssetDatabase.SaveAssets();
        }


        static Color32 Hex(uint hex)
        {
            uint r = (hex & 0xff0000) >> 16;
            uint g = (hex & 0x00ff00) >> 8;
            uint b = hex & 0x0000ff;

            return new Color32((byte)r, (byte)g, (byte)b, 255);
        }


        /*
        static void GenerateGradientPreset(ColorModel colorModel, Color start, Color end, string name, int numColors)
        {
            ColormapPreset preset = CreateInstance<ColormapPreset>();
            preset.SetNumColors(numColors);
            //preset.numberOfColors = numColors;

            //List<Color32> colors = new List<Color32>();

            for (int i = 0; i < numColors; ++i)
            {
                float t = (float)i / ((float)numColors - 1.0f);
                Color color = Color.Lerp(start, end, t);

                if (colorModel == ColorModel.HSV)
                {
                    color = Color.HSVToRGB(color.r, color.g, color.b);
                }

                preset.palette[i] = color;
            }

            //preset.SetColors(colors);

            name = name + " " + numColors;
            string path = FileUtils.PRESETS_DIRECTORY_PATH + name + ".asset";

            AssetDatabase.CreateAsset(preset, path);

            Debug.Log("Created preset: " + name);
        }
        */

        static void GenerateGradientPreset(ColorModel colorModel, Color start, Color mid, Color end, string name, int numColors)
        {
            ColormapPreset preset = CreateInstance<ColormapPreset>();
            preset.SetNumColors(numColors);
            //preset.numberOfColors = numColors;

            //List<Color32> colors = new List<Color32>();

            int halfNumColors = Mathf.CeilToInt(numColors / 2f);

            for (int i = 0; i < halfNumColors; ++i)
            {
                float t = (float)i / ((float)halfNumColors - 1f);
                Color color = Color.Lerp(start, mid, t);

                if (colorModel == ColorModel.HSV)
                {
                    color = Color.HSVToRGB(color.r, color.g, color.b);
                }

                preset.palette[i] = color;
            }

            int secondHalfNumColors = numColors - halfNumColors;

            for (int i = 0; i < secondHalfNumColors; ++i)
            {
                float t = (float)(i + 1f) / ((float)secondHalfNumColors);
                Color color = Color.Lerp(mid, end, t);

                if (colorModel == ColorModel.HSV)
                {
                    color = Color.HSVToRGB(color.r, color.g, color.b);
                }

                preset.palette[i + halfNumColors] = color;
            }

            //preset.SetColors(colors);

            //name = ((numColors == 15) ? "4Bit" : "8Bit") + " " + name;

            name = "[" + (numColors < 10 ? "0" : "") + numColors + " colors] " + name;
            string path = FileUtils.PRESETS_DIRECTORY_PATH + name + ".asset";

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

