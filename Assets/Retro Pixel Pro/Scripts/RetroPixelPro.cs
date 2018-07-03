using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace AlpacaSound.RetroPixelPro
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Custom/Retro Pixel Pro")]
    public class RetroPixelPro : MonoBehaviour
    {

        /// <summary>
        /// The resolution of the pixelated image can be set in two ways.
        /// Either by setting absolute values (ConstantResolution)
        /// or by setting the pixel size (ConstantPixelSize).
        /// </summary>
        public ResolutionMode resolutionMode = ResolutionMode.ConstantPixelSize;

        /// <summary>
        /// The resolution of the pixelated image.
        /// X = width, Y = height.
        /// Only used if resolution mode is ResolutionMode.ConstantResolution.
        /// </summary>
        public Vector2Int resolution = new Vector2Int(Screen.width, Screen.height);

        /// <summary>
        /// Size of the pixels in the pixelated image.
        /// Only used if resolution mode is ResolutionMode.ConstantPixelSize.
        /// </summary>
        public int pixelSize = 1;

        /// <summary>
        /// Coloring dither amount.
        /// Clamped in the range [0, 1]
        /// </summary>
        public float dither = 1;

        /// <summary>
        /// Alpha of the colorization.
        /// Clamped in the range [0, 1]
        /// </summary>
        public float opacity = 1;

        /// <summary>
        /// Contains palette and pre-computed color data.
        /// </summary>
        public Colormap colormap;

        Texture3D colormapTexture;

        Texture2D colormapPalette;
        Material m_material = null;

        /*
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
						Debug.LogWarning("Shader '" + shaderName + "' not found. Was it deleted?");
						enabled = false;
					}

					m_material = new Material(shader);
					m_material.hideFlags = HideFlags.DontSave;
				}

				return m_material;
			}
		}
		*/

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
                        Debug.LogWarning("Shader '" + shaderName + "' not found. Was it deleted?");
                        enabled = false;
                    }

                    m_material = new Material(shader);
                    m_material.hideFlags = HideFlags.DontSave;

                    Texture2D texture = Resources.Load("RetroPixelProResources/Textures/blue_noise") as Texture2D;

                    if (texture == null)
                    {
                        Debug.LogWarning("RetroPixelProResources/Textures/blue_noise.png not found. Was it moved or deleted?");
                    }

                    m_material.SetTexture("_BlueNoise", texture);

                }

                return m_material;
            }
        }


        void Start()
        {
            if (!SystemInfo.supportsImageEffects)
            {
                Debug.LogWarning("This system does not support image effects.");
                enabled = false;
            }
        }

        Colormap oldColormap = null;

        void Reset()
        {
            resolutionMode = ResolutionMode.ConstantPixelSize;
            //resolution.x = Screen.width;
            //resolution.y = Screen.height;
            pixelSize = 3;
            opacity = 1;
            dither = 1;
            colormap = null;
        }


        void OnEnable()
        {
            ApplyColormapToMaterial();
        }


        void OnDisable()
        {
            if (m_material != null)
            {
                DestroyImmediate(m_material);
                m_material = null;
            }
        }


        public void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (colormap != null && (colormap.changedInternally || oldColormap != colormap))
            {
                colormap.changedInternally = false;
                ApplyColormapToMaterial();
            }

            oldColormap = colormap;

            ApplyMaterialVariables();

            RenderTexture scaled = RenderTexture.GetTemporary(resolution.x, resolution.y);
            scaled.filterMode = FilterMode.Point;

            if (colormap == null)
            {
                Graphics.Blit(src, scaled);
            }
            else
            {
                Graphics.Blit(src, scaled, material);
            }

            Graphics.Blit(scaled, dest);
            RenderTexture.ReleaseTemporary(scaled);
        }


        public void ApplyMaterialVariables()
        {
            pixelSize = (int)Mathf.Clamp(pixelSize, 1, float.MaxValue);

            if (resolutionMode == ResolutionMode.ConstantPixelSize)
            {
                resolution.x = Screen.width / pixelSize;
                resolution.y = Screen.height / pixelSize;
            }

            resolution.x = Mathf.Clamp(resolution.x, 1, 16384);
            resolution.y = Mathf.Clamp(resolution.y, 1, 16384);

            opacity = Mathf.Clamp01(opacity);
            dither = Mathf.Clamp01(dither);

            material.SetFloat("_Opacity", opacity);
            material.SetFloat("_Dither", dither);
        }

        public void ApplyColormapToMaterial()
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
            int colorsteps = ColormapUtils.GetColormapSize3D(colormap.preview);
            colormapTexture = new Texture3D(colorsteps, colorsteps, colorsteps, TextureFormat.RGB24, false)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            colormapTexture.SetPixels32(colormap.pixels);
            colormapTexture.Apply();
            material.SetTexture("_Colormap", colormapTexture);
        }


    }

    public enum ResolutionMode
    {
        ConstantResolution,
        ConstantPixelSize,
    }
}



