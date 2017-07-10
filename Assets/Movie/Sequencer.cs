using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlpacaSound.RetroPixelPro;



public class Sequencer : MonoBehaviour 
{
	public GameObject camera_yellowBrown;
	public GameObject camera_blue;
	public GameObject camera_monoBlue;
	public GameObject camera_blueRed;
	public GameObject camera_green;
	public GameObject camera_laughter;

	public GameObject field;
	public GameObject flower;
	public GameObject wave;
	public GameObject laughter;

	RetroPixelPro blueRedPixel;
	//RetroPixelPro yellowPixel;
	RetroPixelPro laughterPixel;

	void Start ()
	{
		camera_yellowBrown = GameObject.Find("Camera - Yellow Brown");
		camera_blueRed = GameObject.Find("Camera - Blue Red");
		camera_green = GameObject.Find("Camera - MonoGreen");
		camera_monoBlue = GameObject.Find("Camera - MonoBlue");
		camera_blue = GameObject.Find("Camera - Blue");
		camera_laughter = GameObject.Find("Camera - Laughter");

		field = GameObject.Find("Quad - Field");
		flower = GameObject.Find("Quad - Flower");
		wave = GameObject.Find("Quad - Movie");
		laughter = GameObject.Find("Quad - Laughter");

		SetCameraEnabled(camera_yellowBrown, false);
		SetCameraEnabled(camera_blue, false);
		SetCameraEnabled(camera_blueRed, false);
		SetCameraEnabled(camera_green, false);
		SetCameraEnabled(camera_laughter, false);

		SetRendererEnabled(field, false);
		SetRendererEnabled(flower, false);
		SetRendererEnabled(wave, false);
		SetRendererEnabled(laughter, false);

		blueRedPixel = camera_blueRed.GetComponent<RetroPixelPro>();
		//yellowPixel = camera_yellowBrown.GetComponent<RetroPixelPro>();
		laughterPixel = camera_laughter.GetComponent<RetroPixelPro>();

		StartCoroutine(Phase1());
	}


	void SetCameraEnabled(GameObject obj, bool b)
	{
		obj.GetComponent<Camera>().enabled = b;
	}
	
	
	void SetRendererEnabled(GameObject obj, bool b)
	{
		obj.GetComponent<MeshRenderer>().enabled = b;
	}
	
	
	IEnumerator Phase1()
	{
		SetRendererEnabled(flower, true);
		SetCameraEnabled(camera_blueRed, true);

		blueRedPixel.resolution.x = 192;
		blueRedPixel.resolution.y = 108;

		float startTime = Time.time;

		float initialDuration = 8;
		while(Time.time < startTime + initialDuration)
		{
			float duration  = Time.time - startTime;
			flower.transform.localScale = new Vector3(2048, 1580, 1) * 0.01f * duration / 2;
			flower.transform.Rotate(Vector3.forward, -3 * Time.deltaTime);

			yield return null;
		}

		float pixelFadeDuration = 3;

		while(Time.time < startTime + initialDuration + pixelFadeDuration)
		{
			float duration  = Time.time - startTime;
			float currentDuration = duration - initialDuration;
			flower.transform.localScale = new Vector3(2048, 1580, 1) * 0.01f * duration / 2;
			flower.transform.Rotate(Vector3.forward, -3 * Time.deltaTime);
			
			blueRedPixel.resolution.x = (int) Mathf.Lerp(192, 1920, currentDuration / pixelFadeDuration);
			blueRedPixel.resolution.y = (int) Mathf.Lerp(108, 1080, currentDuration / pixelFadeDuration);
			
			yield return null;
		}

		blueRedPixel.resolution.x = 1920;
		blueRedPixel.resolution.y = 1080;

		while(Time.time < startTime + 14)
		{
			float duration  = Time.time - startTime;
			flower.transform.localScale = new Vector3(2048, 1580, 1) * 0.01f * duration / 2;
			flower.transform.Rotate(Vector3.forward, -3 * Time.deltaTime);
			yield return null;
		}
		
		SetRendererEnabled(flower, false);
		SetCameraEnabled(camera_blueRed, false);

		StartCoroutine(Phase2());
	}
	
	
	IEnumerator Phase2()
	{
		SetRendererEnabled(wave, true);
		SetCameraEnabled(camera_monoBlue, true);
		wave.GetComponent<MoviePlayer>().Play();

		float startTime = Time.time;
		while(Time.time < startTime + 8)
		{
			yield return null;
		}
		
		SetCameraEnabled(camera_monoBlue, false);
		SetCameraEnabled(camera_blue, true);
		
		while(Time.time < startTime + 13)
		{
			yield return null;
		}
		
		SetCameraEnabled(camera_blue, false);
		SetCameraEnabled(camera_green, true);
		
		while(Time.time < startTime + 20)
		{
			yield return null;
		}
		
		SetRendererEnabled(wave, false);
		SetCameraEnabled(camera_green, false);
		wave.GetComponent<MoviePlayer>().Stop();

		StartCoroutine(Phase3());
	}
	
	
	IEnumerator Phase3()
	{
		SetRendererEnabled(field, true);
		SetCameraEnabled(camera_yellowBrown, true);
		
		Vector3 startPosition = new Vector3(-2, 10, 10);
		Vector3 endPosition = new Vector3(5, 12, 10);
		
		float duration = 10;
		
		float startTime = Time.time;
		while(Time.time < startTime + duration)
		{
			float currentTime = Time.time - startTime;
			field.transform.localPosition = Vector3.Lerp(startPosition, endPosition, currentTime/duration);
			yield return null;
		}
		
		
		SetRendererEnabled(field, false);
		SetCameraEnabled(camera_yellowBrown, false);
		
		StartCoroutine(Phase4());
	}
	
	
	IEnumerator Phase4()
	{
		SetRendererEnabled(laughter, true);
		SetCameraEnabled(camera_laughter, true);
		
		laughterPixel.resolution.x = 160;
		laughterPixel.resolution.y = 100;
		laughterPixel.opacity = 0;
		
		float startTime = Time.time;

		float duration1 = 3;
		float duration2 = 4;
		float duration3 = 7;

		while(Time.time < startTime + duration1)
		{
			yield return null;
		}

		laughterPixel.opacity = 1;

		while(Time.time < startTime + duration1 + duration2)
		{
			yield return null;
		}
		
		laughterPixel.resolution.x = 1920;
		laughterPixel.resolution.y = 1080;
		laughterPixel.opacity = 0.56f;

		while(Time.time < startTime + duration1 + duration2 + duration3)
		{
			yield return null;
		}
		
		SetRendererEnabled(laughter, false);
		SetCameraEnabled(camera_laughter, false);
	}
	
	
}
