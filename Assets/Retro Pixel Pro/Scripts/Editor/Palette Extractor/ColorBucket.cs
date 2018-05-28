using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AlpacaSound.RetroPixelPro
{

	public class ColorBucket
	{
		float redVariance;
		float greenVariance;
		float blueVariance;

		Color32[] colors;

		public Color averageColor;


		public ColorBucket(Color32[] colors)
		{
			this.colors = colors;
			FindAverage();
			FindVariances();
		}

		void FindAverage()
		{
			float redSum = 0;
			float greenSum = 0;
			float blueSum = 0;

			for (int i = 0; i < colors.Length; ++i)
			{
				Color c = colors[i];

				redSum += c.r;
				greenSum += c.g;
				blueSum += c.b;
			}

			averageColor.r = Mathf.Clamp01(redSum / colors.Length);
			averageColor.g = Mathf.Clamp01(greenSum / colors.Length);
			averageColor.b = Mathf.Clamp01(blueSum / colors.Length);
		}

		void FindVariances()
		{
			float redDistance = 0;
			float greenDistance = 0;
			float blueDistance = 0;

			for (int i = 0; i < colors.Length; ++i)
			{
				Color c = colors[i];

				redDistance += Mathf.Pow(c.r - averageColor.r, 2);
				greenDistance += Mathf.Pow(c.g - averageColor.g, 2);
				blueDistance += Mathf.Pow(c.b - averageColor.b, 2);
			}

			redVariance = redDistance / colors.Length;
			greenVariance = greenDistance / colors.Length;
			blueVariance = blueDistance / colors.Length;
		}

		public float BiggestVariance
		{
			get
			{
				return Mathf.Max(redVariance, greenVariance, blueVariance);
			}
		}

		public void SortOnBiggestVarience()
		{
			if (redVariance > greenVariance && redVariance > blueVariance)
			{
				System.Array.Sort(colors, new RedComparer());
			}
			else if (greenVariance > blueVariance)
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
			SortOnBiggestVarience();

			int length = colors.Length / 2;

			Color32[] lowColors = new Color32[length];
			Color32[] highColors = new Color32[length];

			System.Array.Copy(colors, lowColors, length);
			System.Array.Copy(colors, length, highColors, 0, length);

			List<ColorBucket> result = new List<ColorBucket>
			{
				new ColorBucket(lowColors),
				new ColorBucket(highColors)
			};

			return result;
		}


		override public string ToString()
		{
			string s = "";

			s += "[ColorBucket: ";
			s += "size=" + colors.Length + ", ";
			s += "average=" + averageColor + ", ";
			s += "red variance=[" + redVariance + "] ";
			s += "green variance=[" + greenVariance + "] ";
			s += "blue variance=[" + blueVariance + "] ";

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