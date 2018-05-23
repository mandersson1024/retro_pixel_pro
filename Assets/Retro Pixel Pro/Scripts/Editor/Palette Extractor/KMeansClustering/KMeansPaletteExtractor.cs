using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KMeansPaletteExtractor
{

	public KMeansPaletteExtractor(Texture2D texture)
	{
		//MedianCutColorBucket bucket = new MedianCutColorBucket(texture.GetPixels32());
		//paletteTree = new MedianCutPaletteTree(bucket, 0, 8);
	}


	public List<Color32> GetColors(int numColors)
	{
		List<Color32> result = new List<Color32>()
		{
			Color.black,
			Color.white,
		};

		return result;
	}


	public static List<Color32> ExtractPalette(string validImagePath, int numberOfColors)
	{
		byte[] fileData = System.IO.File.ReadAllBytes(validImagePath);
		Texture2D tex = new Texture2D(2, 2);
		tex.LoadImage(fileData);

		int scaledHeight = 128;
		int scaledWidth = (128 * tex.width) / tex.height;

		RenderTexture scaled = RenderTexture.GetTemporary(scaledWidth, scaledHeight);
		Graphics.Blit(tex, scaled);
		Texture2D smalltex = new Texture2D(scaledWidth, scaledHeight);
		smalltex.ReadPixels(new Rect(0, 0, scaledWidth, scaledHeight), 0, 0);
		RenderTexture.ReleaseTemporary(scaled);

		KMeansPaletteExtractor extractor = new KMeansPaletteExtractor(smalltex);
		List<Color32> extractedPalette = extractor.GetColors(numberOfColors);

		return extractedPalette;
	}
}
