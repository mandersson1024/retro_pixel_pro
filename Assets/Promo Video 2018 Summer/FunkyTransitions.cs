using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FunkyTransitions : MonoBehaviour
{

    public RenderTexture texture;
    public Material mat;

    public void Start()
    {
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }
}
