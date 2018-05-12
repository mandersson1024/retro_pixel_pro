using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AlpacaSound.RetroPixelPro
{

    public class PaletteTree
    {
        ColorBucket bucket;
        int level;
        public List<PaletteTree> children;


        public PaletteTree(ColorBucket bucket, int level, int maxLevels)
        {
            this.bucket = bucket;
            this.level = level;

            if (level < maxLevels)
            {
                children = new List<PaletteTree>();
                List<ColorBucket> childBuckets = bucket.MedianCut();

                foreach (ColorBucket child in childBuckets)
                {
                    children.Add(new PaletteTree(child, level + 1, maxLevels));
                }
            }
        }


        public List<Color32> GetColorsFromLevel(int level)
        {
            List<Color32> result = new List<Color32>();

            if (level < 0)
            {
                return result;
            }
            else if (this.level == level)
            {
                result.Add(bucket.averageColor);
            }
            else
            {
                foreach (PaletteTree child in children)
                {
                    result.AddRange(child.GetColorsFromLevel(level));
                }
            }

            return result;
        }


    }

}

