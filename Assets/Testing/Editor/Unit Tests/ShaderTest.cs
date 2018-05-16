using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

using AlpacaSound.RetroPixelPro;

public class ShaderTest
{


	Texture3D CreateRGBCube(int size3D)
	{
		Color32[] pixels = new Color32[size3D * size3D * size3D];

		for (byte r = 0; r < size3D; ++r)
		{
			for (byte g = 0; g < size3D; ++g)
			{
				for (byte b = 0; b < size3D; ++b)
				{
					int index = r + (g * size3D) + (b * size3D * size3D);
					pixels[index] = new Color32(r, g, b, 255);
				}
			}
		}

		Texture3D tex = new Texture3D(size3D, size3D, size3D, TextureFormat.RGB24, false)
		{
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp
		};

		tex.SetPixels32(pixels);
		tex.Apply();

		return tex;
	}


	Texture3D CreateRGBCubeFromSequence(int size3D)
	{
		Texture3D tex = new Texture3D(size3D, size3D, size3D, TextureFormat.RGB24, false)
		{
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp
		};

		Color32[] pixels = new Color32[size3D * size3D * size3D];

		for (int i = 0; i < pixels.Length; ++i)
		{
			pixels[i] = ColormapUtils.IndexToColor(size3D, i);
		}

		tex.SetPixels32(pixels);
		tex.Apply();

		return tex;
	}


	Texture2D CreateColormap(int size3D, int size2D)
	{
		Texture3D rgbCube = CreateRGBCubeFromSequence(size3D);
		Color32[] pixels = rgbCube.GetPixels32();

		Texture2D tex = new Texture2D(size2D, size2D, TextureFormat.RGB24, false)
		{
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp
		};

		tex.SetPixels32(pixels);
		tex.Apply();

		return tex;
	}


	int ColorToIndex(int colorsteps, Color32 color)
	{
		return color.r + (color.g * colorsteps) + (color.b * colorsteps * colorsteps);
	}


	Vector2Int IndexToColormap2D(int size2D, int index)
	{
		int x = index % size2D;
		int y = index / size2D;

		return new Vector2Int(x, y);
	}


	Vector2Int Colormap3DToColormap2D(int size3D, int size2D, Color32 color)
	{
		int index = ColorToIndex(size3D, color);
		return IndexToColormap2D(size2D, index);
	}


	[Test]
	public void Test_ColorConversion()
	{
		Assert.AreEqual(new Color(0, 0, 0, 0), (Color)new Color32(0, 0, 0, 0));
		Assert.AreEqual(new Color(1 / 255f, 1 / 255f, 1 / 255f, 1 / 255f), (Color)new Color32(1, 1, 1, 1));
		Assert.AreEqual(new Color(127 / 255f, 127 / 255f, 127 / 255f, 127 / 255f), (Color)new Color32(127, 127, 127, 127));
		Assert.AreEqual(new Color(1, 1, 1, 1), (Color)new Color32(255, 255, 255, 255));
	}


	[Test]
	public void Test_PixelOrderingInRGBCube()
	{
		Texture3D texture;
		Color32[] pixels;

		texture = CreateRGBCube(2);
		pixels = texture.GetPixels32();
		Assert.AreEqual(new Color32(0, 0, 0, 255), pixels[0]);
		Assert.AreEqual(new Color32(1, 0, 0, 255), pixels[1]);
		Assert.AreEqual(new Color32(0, 1, 0, 255), pixels[2]);
		Assert.AreEqual(new Color32(1, 1, 0, 255), pixels[3]);
		Assert.AreEqual(new Color32(0, 0, 1, 255), pixels[4]);
		Assert.AreEqual(new Color32(1, 0, 1, 255), pixels[5]);
		Assert.AreEqual(new Color32(0, 1, 1, 255), pixels[6]);
		Assert.AreEqual(new Color32(1, 1, 1, 255), pixels[7]);
	}


	[Test]
	public void Test_PixelOrderingInRGBCubeFromSequence()
	{
		Texture3D texture;
		Color32[] pixels;

		texture = CreateRGBCubeFromSequence(2);
		pixels = texture.GetPixels32();

		Assert.AreEqual(new Color32(0, 0, 0, 255), pixels[0]);
		Assert.AreEqual(new Color32(1, 0, 0, 255), pixels[1]);
		Assert.AreEqual(new Color32(0, 1, 0, 255), pixels[2]);
		Assert.AreEqual(new Color32(1, 1, 0, 255), pixels[3]);
		Assert.AreEqual(new Color32(0, 0, 1, 255), pixels[4]);
		Assert.AreEqual(new Color32(1, 0, 1, 255), pixels[5]);
		Assert.AreEqual(new Color32(0, 1, 1, 255), pixels[6]);
		Assert.AreEqual(new Color32(1, 1, 1, 255), pixels[7]);
	}


	[Test]
	public void Test_IndexToColor()
	{
		Assert.AreEqual(new Color32(0, 0, 0, 255), ColormapUtils.IndexToColor(2, 0));
		Assert.AreEqual(new Color32(1, 0, 0, 255), ColormapUtils.IndexToColor(2, 1));
		Assert.AreEqual(new Color32(0, 1, 0, 255), ColormapUtils.IndexToColor(2, 2));
		Assert.AreEqual(new Color32(1, 1, 0, 255), ColormapUtils.IndexToColor(2, 3));
		Assert.AreEqual(new Color32(0, 0, 1, 255), ColormapUtils.IndexToColor(2, 4));
		Assert.AreEqual(new Color32(1, 0, 1, 255), ColormapUtils.IndexToColor(2, 5));
		Assert.AreEqual(new Color32(0, 1, 1, 255), ColormapUtils.IndexToColor(2, 6));
		Assert.AreEqual(new Color32(1, 1, 1, 255), ColormapUtils.IndexToColor(2, 7));

		Assert.AreEqual(new Color32(0, 0, 0, 255), ColormapUtils.IndexToColor(4, 0));
		Assert.AreEqual(new Color32(1, 0, 0, 255), ColormapUtils.IndexToColor(4, 1));
		Assert.AreEqual(new Color32(2, 0, 0, 255), ColormapUtils.IndexToColor(4, 2));
		Assert.AreEqual(new Color32(3, 0, 0, 255), ColormapUtils.IndexToColor(4, 3));
		Assert.AreEqual(new Color32(0, 1, 0, 255), ColormapUtils.IndexToColor(4, 4));
	}


	[Test]
	public void Test_ColormapLayout()
	{
		Texture2D colormap;

		colormap = CreateColormap(1, 1);
		Assert.AreEqual(new Color32(0, 0, 0, 255), (Color32)colormap.GetPixel(0, 0));

		colormap = CreateColormap(4, 8);
		Assert.AreEqual(new Color32(0, 0, 0, 255), (Color32)colormap.GetPixel(0, 0));
		Assert.AreEqual(new Color32(1, 0, 0, 255), (Color32)colormap.GetPixel(1, 0));
		Assert.AreEqual(new Color32(2, 0, 0, 255), (Color32)colormap.GetPixel(2, 0));
		Assert.AreEqual(new Color32(3, 0, 0, 255), (Color32)colormap.GetPixel(3, 0));
		Assert.AreEqual(new Color32(0, 1, 0, 255), (Color32)colormap.GetPixel(4, 0));
		Assert.AreEqual(new Color32(1, 1, 0, 255), (Color32)colormap.GetPixel(5, 0));
		Assert.AreEqual(new Color32(2, 1, 0, 255), (Color32)colormap.GetPixel(6, 0));
		Assert.AreEqual(new Color32(3, 1, 0, 255), (Color32)colormap.GetPixel(7, 0));
		Assert.AreEqual(new Color32(0, 2, 0, 255), (Color32)colormap.GetPixel(0, 1));
		Assert.AreEqual(new Color32(3, 3, 3, 255), (Color32)colormap.GetPixel(7, 7));
	}


	[Test]
	public void Test_IndexToColorToIndex()
	{
		Assert.AreEqual(0, ColorToIndex(4, ColormapUtils.IndexToColor(4, 0)));
		Assert.AreEqual(1, ColorToIndex(4, ColormapUtils.IndexToColor(4, 1)));
		Assert.AreEqual(2, ColorToIndex(4, ColormapUtils.IndexToColor(4, 2)));
		Assert.AreEqual(3, ColorToIndex(4, ColormapUtils.IndexToColor(4, 3)));
		Assert.AreEqual(4, ColorToIndex(4, ColormapUtils.IndexToColor(4, 4)));
		Assert.AreEqual(5, ColorToIndex(4, ColormapUtils.IndexToColor(4, 5)));
		Assert.AreEqual(6, ColorToIndex(4, ColormapUtils.IndexToColor(4, 6)));
		Assert.AreEqual(7, ColorToIndex(4, ColormapUtils.IndexToColor(4, 7)));
		Assert.AreEqual(8, ColorToIndex(4, ColormapUtils.IndexToColor(4, 8)));
		Assert.AreEqual(9, ColorToIndex(4, ColormapUtils.IndexToColor(4, 9)));
		Assert.AreEqual(10, ColorToIndex(4, ColormapUtils.IndexToColor(4, 10)));
		Assert.AreEqual(11, ColorToIndex(4, ColormapUtils.IndexToColor(4, 11)));
		Assert.AreEqual(12, ColorToIndex(4, ColormapUtils.IndexToColor(4, 12)));
	}


	[Test]
	public void Test_IndexToColormap2D()
	{
		Assert.AreEqual(new Vector2Int(0, 0), IndexToColormap2D(8, 0));
		Assert.AreEqual(new Vector2Int(1, 0), IndexToColormap2D(8, 1));
		Assert.AreEqual(new Vector2Int(2, 0), IndexToColormap2D(8, 2));
		Assert.AreEqual(new Vector2Int(3, 0), IndexToColormap2D(8, 3));
		Assert.AreEqual(new Vector2Int(4, 0), IndexToColormap2D(8, 4));
		Assert.AreEqual(new Vector2Int(5, 0), IndexToColormap2D(8, 5));
		Assert.AreEqual(new Vector2Int(6, 0), IndexToColormap2D(8, 6));
		Assert.AreEqual(new Vector2Int(7, 0), IndexToColormap2D(8, 7));
		Assert.AreEqual(new Vector2Int(0, 1), IndexToColormap2D(8, 8));
		Assert.AreEqual(new Vector2Int(7, 7), IndexToColormap2D(8, 63));
	}

	[Test]
	public void Test_Colormap3DToColormap2D()
	{
		Assert.AreEqual(new Vector2Int(0, 0), Colormap3DToColormap2D(4, 8, new Color32(0, 0, 0, 255)));
		Assert.AreEqual(new Vector2Int(1, 0), Colormap3DToColormap2D(4, 8, new Color32(1, 0, 0, 255)));
		Assert.AreEqual(new Vector2Int(2, 0), Colormap3DToColormap2D(4, 8, new Color32(2, 0, 0, 255)));
		Assert.AreEqual(new Vector2Int(3, 0), Colormap3DToColormap2D(4, 8, new Color32(3, 0, 0, 255)));
		Assert.AreEqual(new Vector2Int(4, 0), Colormap3DToColormap2D(4, 8, new Color32(0, 1, 0, 255)));
		Assert.AreEqual(new Vector2Int(5, 0), Colormap3DToColormap2D(4, 8, new Color32(1, 1, 0, 255)));
		Assert.AreEqual(new Vector2Int(6, 0), Colormap3DToColormap2D(4, 8, new Color32(2, 1, 0, 255)));
		Assert.AreEqual(new Vector2Int(7, 0), Colormap3DToColormap2D(4, 8, new Color32(3, 1, 0, 255)));
		Assert.AreEqual(new Vector2Int(0, 1), Colormap3DToColormap2D(4, 8, new Color32(0, 2, 0, 255)));
		Assert.AreEqual(new Vector2Int(7, 7), Colormap3DToColormap2D(4, 8, new Color32(3, 3, 3, 255)));
	}


}
