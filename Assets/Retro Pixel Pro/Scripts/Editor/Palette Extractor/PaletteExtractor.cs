using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AlpacaSound.RetroPixelPro
{

    public class PaletteExtractor
    {
        public List<ColorBucket> buckets = new List<ColorBucket>();


        public PaletteExtractor(Texture2D texture)
        {
            ColorBucket bucket = new ColorBucket(texture.GetPixels32());
            buckets.Add(bucket);
        }


        public List<Color32> GetColors(int numColors)
        {
            while (buckets.Count < numColors)
            {
                buckets.Sort((ColorBucket x, ColorBucket y) => (int)Mathf.Sign(y.BiggestVariance - x.BiggestVariance));
                ColorBucket bucket = buckets[0];
                buckets.RemoveAt(0);
                List<ColorBucket> splitResult = bucket.MedianCut();
                buckets.AddRange(splitResult);

                if (splitResult.Count < 2)
                {
                    break;
                }
            }

            List<Color32> colors = new List<Color32>();

            foreach (ColorBucket bucket in buckets)
            {
                colors.Add(bucket.averageColor);
            }

            // Sort by approx luminance (grayscale value)
            colors.Sort((Color32 x, Color32 y) => (int)Mathf.Sign(((Color)x).grayscale - ((Color)y).grayscale));

            return colors;
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

            PaletteExtractor extractor = new PaletteExtractor(smalltex);
            List<Color32> extractedPalette = extractor.GetColors(numberOfColors);

            return extractedPalette;
        }

    }

}

