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

    public void Start()
    {
        mat.SetTexture("_OtherTexture", otherTexture);
        mat.SetTexture("_GradientTexture", gradientTexture);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetFloat("_Amount", amount);
        Graphics.Blit(source, destination, mat);
    }
}
