using UnityEngine;
using System.Collections;
using UnityEditor;


namespace AlpacaSound.RetroPixelPro
{

    public class ColormapCalculator
    {
        public float progress;
        public Color32[] pixelBuffer;

        Color32[] palette;
        bool[] usedColors;
        int numColors;
        System.Action doneCallback;
        int colorsteps;
        int totalPixels;
        int pixelProgress;



        public ColormapCalculator(Color32[] palette, bool[] usedColors, int numColors, System.Action doneCallback)
        {
            this.palette = palette;
            this.usedColors = usedColors;
            this.doneCallback = doneCallback;
            this.numColors = numColors;
            progress = 0;
            pixelProgress = 0;
            SetupPixelBuffer();
        }


        void SetupPixelBuffer()
        {
            colorsteps = 64;
            totalPixels = colorsteps * colorsteps * colorsteps;
            pixelBuffer = new Color32[totalPixels];
        }


        public void CalculateChunk()
        {
            double frameStartTime = EditorApplication.timeSinceStartup;

            while (EditorApplication.timeSinceStartup < frameStartTime + (1.0 / 30.0))
            {
                CalculateNextPixel();
            }
        }


        void CalculateNextPixel()
        {
            if (pixelProgress < totalPixels)
            {
                int temp = pixelProgress;

                int r = temp % colorsteps;
                temp /= colorsteps;

                int g = temp % colorsteps;
                temp /= colorsteps;

                int b = temp % colorsteps;

                CalculatePixel(r, g, b);

                ++pixelProgress;
                progress = (float)pixelProgress / (float)totalPixels;
            }
            else
            {
                doneCallback.Invoke();
            }
        }


        struct ColormapValue
        {
            public byte mainPaletteIndex;
            public byte secondaryPaletteIndex;
            public byte blend;
        }

        void CalculatePixel(int r, int g, int b)
        {
            //byte paletteIndex = GetClosestPaletteIndex(r, g, b);
            ColormapValue value = GetTwoClosestPaletteIndices(r, g, b);

            //Debug.Log("Blend is: " + value.blend);


            pixelBuffer[pixelProgress] = new Color32(value.mainPaletteIndex, value.secondaryPaletteIndex, (byte)(value.blend / 2), 1);
        }

        ColormapValue GetTwoClosestPaletteIndices(int r, int g, int b)
        {
            float closestDistance = float.MaxValue;
            float nextClosestDistance = float.MaxValue;

            int closestIndex = 0;
            int nextClosestIndex = 0;

            Vector3 rgb = new Vector3(r, g, b);
            rgb = 256 * rgb / (colorsteps - 1);

            for (int i = 0; i < numColors; ++i)
            {
                if (usedColors[i])
                {
                    Vector3 paletteRGB = new Vector3(palette[i].r, palette[i].g, palette[i].b);
                    float distance = Vector3.Distance(rgb, paletteRGB);
                    if (distance < closestDistance)
                    {
                        nextClosestDistance = closestDistance;
                        closestDistance = distance;

                        nextClosestIndex = closestIndex;
                        closestIndex = i;
                    }
                    else if (distance < nextClosestDistance)
                    {
                        nextClosestDistance = distance;
                        nextClosestIndex = i;
                    }
                }
            }

            return new ColormapValue()
            {
                mainPaletteIndex = (byte)closestIndex,
                secondaryPaletteIndex = (byte)nextClosestIndex,
                blend = Mathf.Approximately(nextClosestDistance, 0) ? (byte)127 : ((byte)(255 * (closestDistance / nextClosestDistance)))
            };
        }

        byte GetClosestPaletteIndex(int r, int g, int b)
        {
            float closestDistance = float.MaxValue;
            int closestIndex = 0;
            Vector3 rgb = new Vector3(r, g, b);
            rgb = 256 * rgb / (colorsteps - 1);

            for (int i = 0; i < numColors; ++i)
            {
                if (usedColors[i])
                {
                    Vector3 paletteRGB = new Vector3(palette[i].r, palette[i].g, palette[i].b);
                    float distance = Vector3.Distance(rgb, paletteRGB);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestIndex = i;
                    }
                }
            }

            return (byte)closestIndex;
        }

    }

}


