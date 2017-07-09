using UnityEngine;
using System.Collections;


namespace AlpacaSound.RetroPixelPro
{
	public class AutomatedTests : MonoBehaviour
	{

		public Colormap colormap1;
		public Colormap colormap2;

		int index = 0;
		RetroPixelPro retroPixel;

		void Start()
		{
			Debug.Log("Click mouse to step through the tests.");
		}

		void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Step(index++);
			}
		}

		void Step(int a)
		{
			Debug.Log("step " + a);

			if (a == 0)
			{
				Debug.Log(a + ". Adding Retro Pixel Pro component");
				retroPixel = GameObject.Find("Main Camera").AddComponent<RetroPixelPro>();
			}
			else if (a == 1)
			{
				Debug.Log(a + ". Setting colormap1 (" + colormap1 + ")");
				retroPixel.colormap = colormap1;
			}
			else if (a == 2)
			{
				Debug.Log(a + ". Setting colormap to null");
				retroPixel.colormap = null;
			}
			else if (a == 3)
			{
				Debug.Log(a + ". Setting colormap1 (" + colormap1 + ")");
				retroPixel.colormap = colormap1;
			}
			else if (a == 4)
			{
				Debug.Log(a + ". Setting colormap2 (" + colormap2 + ")");
				retroPixel.colormap = colormap2;
			}
			else if (a == 5)
			{
				Debug.Log(a + ". Loading colormap resource");
				Colormap loaded = Resources.Load("AutomatedTestColormap") as Colormap;

				Debug.Log(a + ". Setting colormap resource (" + loaded + ")");
				retroPixel.colormap = loaded;
			}
			else if (a == 6)
			{
				Debug.Log(a + ". Instantiating Camera prefab.");
				Instantiate (Resources.Load ("AutomatedTestCamera") as GameObject);
			}
			else
			{
				Debug.Log("No more steps");
			}
		}
	}
}

