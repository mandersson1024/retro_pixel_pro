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
    public float amount;

    public void Awake()
    {
        mat.SetFloat("_Amount", 0);
    }

    public void OnValidate()
    {
        mat.SetTexture("_OtherTex", otherTexture);
        mat.SetTexture("_GradientTex", gradientTexture);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetFloat("_Amount", amount);
        Graphics.Blit(source, destination, mat);
    }
}
