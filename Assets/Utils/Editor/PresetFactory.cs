﻿using UnityEngine;
using System.Collections;
using UnityEditor;

namespace AlpacaSound.RetroPixelPro
{

	public class PresetFactory : Editor
	{

		[MenuItem("Retro Pixel Pro Utilities/Generate Presets")]
		static void GeneratePresets()
		{
			foreach (PalettePresets.PresetName preset in PalettePresets.PresetName.GetValues(typeof(PalettePresets.PresetName)))
			{
				GeneratePreset(preset);
			}

			AssetDatabase.SaveAssets();
		}

		static void GeneratePreset(PalettePresets.PresetName preset)
		{
			Colormap colormap = CreateInstance<Colormap>();
			colormap.preset = preset; // behövs inte om ApplyPreset sätter detta
			PalettePresets.SetPalette(preset, colormap);

			string name = preset.ToString();
			string path = "Assets/Retro Pixel Pro/Colormaps/Presets/" + name + ".asset"; // är sökvägen rätt?

			AssetDatabase.CreateAsset(colormap, path);

			Debug.Log("Created preset: " + name);
		}



	}

}
