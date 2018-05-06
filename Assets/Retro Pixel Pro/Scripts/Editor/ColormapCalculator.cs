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
        System.Action doneCallback;
        int colorsteps;
        int totalPixels;
        int pixelProgress;



        public ColormapCalculator(Color32[] palette, System.Action doneCallback)
        {
            this.palette = palette;
            this.doneCallback = doneCallback;
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


        static Color32 GetSourceColorFromPixelProgress(int progress, int colorsteps)
        {
            int r = (progress % colorsteps) * (256 / colorsteps);
            progress /= colorsteps;

            int g = (progress % colorsteps) * (256 / colorsteps);
            progress /= colorsteps;

            int b = (progress % colorsteps) * (256 / colorsteps);

            return new Color32((byte)r, (byte)g, (byte)b, 255);
        }

        void CalculateNextPixel()
        {
            if (pixelProgress < totalPixels)
            {
                Color32 sourceColor = GetSourceColorFromPixelProgress(pixelProgress, 64);
                CalculatePixel(sourceColor);

                ++pixelProgress;
                progress = (float)pixelProgress / (float)totalPixels;
            }
            else
            {
                doneCallback.Invoke();
            }
        }

        void CalculatePixel(Color32 color)
        {
            ColormapValue value = CalculateColormapValue(color, palette);
            pixelBuffer[pixelProgress] = value.ToColor32();
        }


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

            float blend = 1;//closestDistance / nextClosestDistance;

            ColormapValue result = new ColormapValue(closest, nextClosest, blend);

            return result;
        }
    }

}


