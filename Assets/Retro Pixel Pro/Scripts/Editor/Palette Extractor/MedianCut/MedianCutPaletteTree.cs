using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AlpacaSound.RetroPixelPro
{

	public class MedianCutPaletteTree
	{
		MedianCutColorBucket bucket;
		int level;
		public List<MedianCutPaletteTree> children;


		public MedianCutPaletteTree(MedianCutColorBucket bucket, int level, int maxLevels)
		{
			this.bucket = bucket;
			this.level = level;

			if (level < maxLevels)
			{
				children = new List<MedianCutPaletteTree>();
				List<MedianCutColorBucket> childBuckets = bucket.MedianCut();

				foreach (MedianCutColorBucket child in childBuckets)
				{
					children.Add(new MedianCutPaletteTree(child, level + 1, maxLevels));
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
				foreach (MedianCutPaletteTree child in children)
				{
					result.AddRange(child.GetColorsFromLevel(level));
				}
			}

			return result;
		}


	}

}

