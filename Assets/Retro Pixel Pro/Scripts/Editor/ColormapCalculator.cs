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
                //CalculatePixel(new Color(r, g, b));

                ++pixelProgress;
                progress = (float)pixelProgress / (float)totalPixels;
            }
            else
            {
                doneCallback.Invoke();
            }
        }


        struct ColormapValueDeprecated
        {
            public byte mainPaletteIndex;
            public byte secondaryPaletteIndex;
            public byte blend;
        }

        void CalculatePixel(int r, int g, int b)
        //void CalculatePixel(Color32 color)
        {
            //byte paletteIndex = GetClosestPaletteIndex(r, g, b);
            ColormapValueDeprecated value = GetTwoClosestPaletteIndices(r, g, b);
            pixelBuffer[pixelProgress] = new Color32(value.mainPaletteIndex, value.secondaryPaletteIndex, (byte)(value.blend / 2), 1);

            //ColormapValue value = CalculateColormapValue(color, palette);
            //pixelBuffer[pixelProgress] = value.ToColor32();
        }

        //*
        ColormapValueDeprecated GetTwoClosestPaletteIndices(int r, int g, int b)
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

            return new ColormapValueDeprecated()
            {
                mainPaletteIndex = (byte)closestIndex,
                secondaryPaletteIndex = (byte)nextClosestIndex,
                blend = Mathf.Approximately(nextClosestDistance, 0) ? (byte)127 : ((byte)(255 * (closestDistance / nextClosestDistance)))
            };
        }
        //*/

        static float ColorDistance(Color32 c1, Color32 c2)
        {
            Vector3 rgb1 = new Vector3(c1.r, c1.b, c1.b);
            Vector3 rgb2 = new Vector3(c2.r, c2.b, c2.b);

            return Vector3.Distance(rgb1, rgb2);
        }


        public static int GetClosestPaletteIndex(Color32 color, Color32[] palette)
        {
            float closestDistance = float.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < palette.Length; ++i)
            {
                float distance = ColorDistance(color, palette[i]);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        public static int GetNextClosestPaletteIndex(Color32 color, Color32[] palette, int skipIndex)
        {
            float closestDistance = float.MaxValue;
            int closestIndex = -1;

            for (int i = 0; i < palette.Length; ++i)
            {
                if (i == skipIndex)
                {
                    continue;
                }

                float distance = ColorDistance(color, palette[i]);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        public static ColormapValue CalculateColormapValue(Color32 color, Color32[] palette)
        {
            int closest = GetClosestPaletteIndex(color, palette);
            int nextClosest = closest; //GetNextClosestPaletteIndex(color, palette, closest);

            //float closestDistance = ColorDistance(color, palette[closest]);
            //float nextClosestDistance = ColorDistance(color, palette[nextClosest]);

            float blend = 0;//closestDistance / nextClosestDistance;

            ColormapValue result = new ColormapValue(closest, nextClosest, blend);

            return result;
        }
    }

}


