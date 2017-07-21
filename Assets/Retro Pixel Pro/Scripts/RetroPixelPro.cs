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
        public Vector2 resolution = new Vector2(Screen.width, Screen.height);

        /// <summary>
        /// Size of the pixels in the pixelated image.
        /// Only used if resolution mode is ResolutionMode.ConstantPixelSize.
        /// </summary>
        public int pixelSize = 1;

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


        void Start()
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
            resolutionMode = ResolutionMode.ConstantPixelSize;
            //resolution.x = Screen.width;
            //resolution.y = Screen.height;
            pixelSize = 3;
            opacity = 1;
            colormap = null;
        }


        void OnEnable()
        {
            ApplyToMaterial();
        }


        void OnDisable()
        {
            if (m_material != null)
            {
                Material.DestroyImmediate(m_material);
                m_material = null;
            }
        }


        public void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            pixelSize = (int)Mathf.Clamp(pixelSize, 1, float.MaxValue);

            if (resolutionMode == ResolutionMode.ConstantPixelSize)
            {
                resolution.x = Screen.width / pixelSize;
                resolution.y = Screen.height / pixelSize;
            }

            resolution.x = (int)Mathf.Clamp(resolution.x, 1, 16384);
            resolution.y = (int)Mathf.Clamp(resolution.y, 1, 16384);

            opacity = Mathf.Clamp01(opacity);

            RenderTexture scaled = RenderTexture.GetTemporary((int)resolution.x, (int)resolution.y);
            scaled.filterMode = FilterMode.Point;

            if (colormap == null)
            {
                Graphics.Blit(src, scaled);
            }
            else
            {
                material.SetFloat("_Strength", opacity);
                Graphics.Blit(src, scaled, material);
            }

            Graphics.Blit(scaled, dest);
            RenderTexture.ReleaseTemporary(scaled);
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
            //Debug.Log("RetroPixelPro.ApplyPalette, palette=" + colormap.palette + ", length=" + colormap.palette.Length);

            colormapPalette = new Texture2D(256, 1, TextureFormat.RGB24, false);
            colormapPalette.filterMode = FilterMode.Point;
            colormapPalette.wrapMode = TextureWrapMode.Clamp;

            for (int i = 0; i < colormap.numberOfColors; ++i)
            {
                //Debug.Log("SetPixel(" + i + ")=" + colormap.palette[i]);
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
            colormapTexture.SetPixels32(colormap.texture3Dpixels);
            colormapTexture.Apply();

            material.SetTexture("_ColorMap", colormapTexture);
        }


    }

    public enum ResolutionMode
    {
        ConstantResolution,
        ConstantPixelSize,
    }
}



