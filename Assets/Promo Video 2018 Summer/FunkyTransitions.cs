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

    private void ApplyOffset()
    {
        mat.SetTextureOffset("_GradientTex", new Vector2(Mathf.Lerp(0.66f, -0.66f, offset), 0));
    }

    public void OnValidate()
    {
        mat.SetTexture("_OtherTex", otherTexture);
        mat.SetTexture("_GradientTex", gradientTexture);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        ApplyOffset();
        Graphics.Blit(source, destination, mat);
    }
}
