using UnityEngine;
using System.Collections;

public class MoviePlayer : MonoBehaviour
{

	public void Play () 
	{
		Renderer r = GetComponent<Renderer>();
		MovieTexture movie = (MovieTexture)r.material.mainTexture;
		movie.Play();
	}

	public void Stop () 
	{
		Renderer r = GetComponent<Renderer>();
		MovieTexture movie = (MovieTexture)r.material.mainTexture;
		movie.Stop();
	}

}
