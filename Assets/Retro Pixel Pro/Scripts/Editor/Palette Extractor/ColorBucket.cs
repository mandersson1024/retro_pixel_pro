using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AlpacaSound.RetroPixelPro
{

    public class ColorBucket
    {
        float redMin;
        float redMax;
        float redRange;

        float greenMin;
        float greenMax;
        float greenRange;

        float blueMin;
        float blueMax;
        float blueRange;

        public Color averageColor;

        Color32[] colors;


        public ColorBucket(Color32[] colors)
        {
            this.colors = colors;
            FindRanges();
            SortOnBiggestRange();
        }


        public void FindRanges()
        {
            double redSum = 0;
            double greenSum = 0;
            double blueSum = 0;


            redMin = 1;
            redMax = 0;

            greenMin = 1;
            greenMax = 0;

            blueMin = 1;
            blueMax = 0;

            for (int i = 0; i < colors.Length; ++i)
            {
                Color c = colors[i];

                redMin = Mathf.Min(redMin, c.r);
                redMax = Mathf.Max(redMax, c.r);

                greenMin = Mathf.Min(greenMin, c.g);
                greenMax = Mathf.Max(greenMax, c.g);

                blueMin = Mathf.Min(blueMin, c.b);
                blueMax = Mathf.Max(blueMax, c.b);

                redSum += c.r;
                greenSum += c.g;
                blueSum += c.b;
            }

            redRange = redMax - redMin;
            greenRange = greenMax - greenMin;
            blueRange = blueMax - blueMin;

            averageColor.r = (float)(redSum / colors.Length);
            averageColor.g = (float)(greenSum / colors.Length);
            averageColor.b = (float)(blueSum / colors.Length);
        }


        public void SortOnBiggestRange()
        {
            if (redRange > greenRange && redRange > blueRange)
            {
                System.Array.Sort(colors, new RedComparer());
            }
            else if (greenRange > blueRange)
            {
                System.Array.Sort(colors, new GreenComparer());
            }
            else
            {
                System.Array.Sort(colors, new BlueComparer());
            }
        }


        public List<ColorBucket> MedianCut()
        {
            int length = colors.Length / 2;

            Color32[] lowColors = new Color32[length];
            Color32[] highColors = new Color32[length];

            System.Array.Copy(colors, lowColors, length);
            System.Array.Copy(colors, length, highColors, 0, length);

            List<ColorBucket> result = new List<ColorBucket>();

            result.Add(new ColorBucket(lowColors));
            result.Add(new ColorBucket(highColors));

            return result;
        }


        override public string ToString()
        {
            string s = "";

            s += "[ColorBucket: ";
            s += "size=" + colors.Length + ", ";
            s += "average=" + averageColor + ", ";
            s += "reds=[" + redMin + "," + redMax + "] ";
            s += "greens=[" + greenMin + "," + greenMax + "] ";
            s += "blues=[" + blueMin + "," + blueMax + "]";

            return s;
        }


    }


    abstract public class ColorComparer : IComparer
    {
        public int Compare(object o1, object o2)
        {
            if (o1 is Color32 && o2 is Color32)
            {
                Color32 c1 = (Color32)o1;
                Color32 c2 = (Color32)o2;

                return CompareColors(c1, c2);
            }
            else
            {
                return 0;
            }
        }

        abstract public int CompareColors(Color32 c1, Color32 c2);
    }


    public class RedComparer : ColorComparer
    {
        override public int CompareColors(Color32 c1, Color32 c2)
        {
            return System.Math.Sign(c1.g - c2.g);
        }
    }


    public class GreenComparer : ColorComparer
    {
        override public int CompareColors(Color32 c1, Color32 c2)
        {
            return System.Math.Sign(c1.g - c2.g);
        }
    }


    public class BlueComparer : ColorComparer
    {
        override public int CompareColors(Color32 c1, Color32 c2)
        {
            return System.Math.Sign(c1.b - c2.b);
        }
    }

}