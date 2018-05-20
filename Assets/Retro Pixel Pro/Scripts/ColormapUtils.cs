using UnityEngine;
using System.Collections;


namespace AlpacaSound.RetroPixelPro
{

	public class ColormapUtils
	{

		public static int GetColormapSize3D(bool preview)
		{
			if (preview)
			{
				return 16;
			}
			else
			{
				return 64;
			}
		}


		public static int GetColormapSize2D(bool preview)
		{
			if (preview)
			{
				return 64;
			}
			else
			{
				return 512;
			}
		}


		static public Color32 IndexToColor(int colorsteps, int index)
		{
			int stepLength = 256 / colorsteps;

			int r = stepLength * (index % colorsteps);
			int g = stepLength * ((index / colorsteps) % colorsteps);
			int b = stepLength * (index / (colorsteps * colorsteps));

			return new Color32((byte)r, (byte)g, (byte)b, 255);
		}



		static public Vector3 GetColorstepPosition(Color32 color, int colorsteps)
		{
			int stepLength = 256 / colorsteps;
			float offset = stepLength / 2f;

			Vector3 v = new Vector3
			{
				x = ((color.r / stepLength) * stepLength + offset) / 256f,
				y = ((color.g / stepLength) * stepLength + offset) / 256f,
				z = ((color.b / stepLength) * stepLength + offset) / 256f
			};

			return v;
		}


		static public Vector3 ProjectPointOnLine(Vector3 linepoint1, Vector3 linepoint2, Vector3 point)
		{
			Vector3 lineDirection = (linepoint2 - linepoint1).normalized;
			Vector3 v = point - linepoint1;
			float d = Vector3.Dot(v, lineDirection);

			return linepoint1 + lineDirection * d;
		}

		static public bool PointIsInsideSegment(Vector3 segmentStart, Vector3 segmentEnd, Vector3 point)
		{
			float length = Vector3.Distance(segmentStart, segmentEnd);
			float distanceToStart = Vector3.Distance(point, segmentStart);
			float distanceToEnd = Vector3.Distance(point, segmentEnd);

			return (distanceToStart < length) && (distanceToEnd < length);
		}

	}
}
