using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FunkyTransitions : MonoBehaviour
{

    public RenderTexture otherTexture;
    public Texture2D gradientTexture;
    public Material mat;

    [Range(0, 1)]
    public float offset;

    public void Awake()
    {
    }

    public void OnValidate()
    {
        mat.SetTexture("_OtherTex", otherTexture);
        mat.SetTexture("_GradientTex", gradientTexture);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetTextureOffset("_GradientTex", new Vector2(Mathf.Lerp(0.7f, -0.7f, offset), 0));
        Graphics.Blit(source, destination, mat);
    }
}
