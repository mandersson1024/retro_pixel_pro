using UnityEngine;
using System.Collections;

public class MoviePlayer : MonoBehaviour
{

	public void Play () 
	{
#if !UNITY_WEBGL
        Renderer r = GetComponent<Renderer>();
		MovieTexture movie = (MovieTexture)r.material.mainTexture;
		movie.Play();
#endif
	}

	public void Stop () 
	{
#if !UNITY_WEBGL
		Renderer r = GetComponent<Renderer>();
		MovieTexture movie = (MovieTexture)r.material.mainTexture;
		movie.Stop();
#endif
    }

}
