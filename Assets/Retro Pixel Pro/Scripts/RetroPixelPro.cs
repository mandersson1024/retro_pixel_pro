using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AlpacaSound.RetroPixelPro
{
	[ExecuteInEditMode]
	[RequireComponent (typeof(Camera))]
	[AddComponentMenu("Image Effects/Custom/Retro Pixel Pro")]
	public class RetroPixelPro : MonoBehaviour
	{
		/// <summary>
		/// The horizontal resolution.
		/// Clamped in the range [1, 16384]
		/// </summary>
		public int horizontalResolution = 320;

		/// <summary>
		/// The vertical resolution.
		/// Clamped in the range [1, 16384]
		/// </summary>
		public int verticalResolution = 200;

		/// <summary>
		/// Alpha of the colorization.
		/// Clamped in the range [0, 1]
		/// </summary>
        [Range(0,1)]
		public float strength = 1;

        /// <summary>
        /// Contains palette and pre-computed color data.
        /// </summary>
        public Colormap colormap;

		Texture3D colormapTexture;
		Texture2D colormapPalette;
		Material m_material = null;

		public Material material
		{
			get
			{
				if (m_material == null)
				{
					string shaderName = "AlpacaSound/RetroPixelPro";
					Shader shader = Shader.Find(shaderName);

					if (shader == null)
					{
						Debug.LogWarning ("Shader '" + shaderName + "' not found. Was it deleted?");
						enabled = false;
					}

					m_material = new Material (shader);
					m_material.hideFlags = HideFlags.DontSave;
				}

				return m_material;
			} 
		}


		void Start ()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				Debug.LogWarning("This system does not support image effects.");
				enabled = false;
			}
		}

		Colormap oldColormap = null;

		void Update()
		{
			if (colormap != null && (colormap.changedInternally || oldColormap != colormap))
			{
				colormap.changedInternally = false;
				ApplyToMaterial();
			}

			oldColormap = colormap;
		}


		void Reset()
		{
			horizontalResolution = 320;
			verticalResolution = 200;
			strength = 1;
		}


		void OnEnable()
		{
			ApplyToMaterial();
		}


		void OnDisable ()
		{
			if (m_material != null)
			{
				Material.DestroyImmediate (m_material);
				m_material = null;
			}
		}


		public void OnRenderImage (RenderTexture src, RenderTexture dest)
		{
			horizontalResolution = Mathf.Clamp(horizontalResolution, 1, 16384);
			verticalResolution = Mathf.Clamp(verticalResolution, 1, 16384);
			strength = Mathf.Clamp01(strength);

			if (colormap != null)
			{
				//Debug.Log(colormap.map + ", " + colormap.palette);

				material.SetFloat("_Strength", strength);
				RenderTexture scaled = RenderTexture.GetTemporary (horizontalResolution, (int) verticalResolution);
				scaled.filterMode = FilterMode.Point;
				Graphics.Blit (src, scaled, material);
				//Graphics.Blit (src, scaled);
				Graphics.Blit (scaled, dest);
				RenderTexture.ReleaseTemporary (scaled);
			}
			else
			{
				Graphics.Blit(src, dest);
			}
		}


		public void ApplyToMaterial()
		{
			if (colormap != null)
			{
				ApplyPalette();
				ApplyMap();
			}
		}


		void ApplyPalette()
		{
			colormapPalette = new Texture2D(256, 1, TextureFormat.RGB24, false);
			colormapPalette.filterMode = FilterMode.Point;
			colormapPalette.wrapMode = TextureWrapMode.Clamp;

			for (int i = 0; i < colormap.numberOfColors; ++i)
			{
				colormapPalette.SetPixel(i, 0, colormap.palette[i]);
			}

			colormapPalette.Apply();

			material.SetTexture("_Palette", colormapPalette);
		}


		public void ApplyMap()
		{
			int colorsteps = ColormapUtils.GetPrecisionColorsteps(colormap.colormapPrecision);
			colormapTexture = new Texture3D(colorsteps, colorsteps, colorsteps, TextureFormat.Alpha8, false);
			colormapTexture.filterMode = FilterMode.Point;
			colormapTexture.wrapMode = TextureWrapMode.Clamp;
			colormapTexture.SetPixels32(colormap.buffer);
			colormapTexture.Apply();

			material.SetTexture("_ColorMap", colormapTexture);
		}


	}
}



