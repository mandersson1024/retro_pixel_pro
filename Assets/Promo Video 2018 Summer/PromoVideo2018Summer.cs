using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AlpacaSound.RetroPixelPro;


public class VideoSegment
{
    public Sprite sprite;
    public Colormap colormap;

    public override string ToString()
    {
        return sprite + ", " + colormap;
    }
}


public class PromoVideo2018Summer : MonoBehaviour
{
    static int SEGMENT_COUNT = 17;

    List<VideoSegment> segments;

    SpriteRenderer spriteRenderer;
    Sprite sprite;
    RetroPixelPro retroPixelPro;
    FunkyTransitions funkyTransitions;
    GameObject logo;

    int currentlyShowingIndex = 0;
    float segmentStartTime;
    float SegmentRunningTime
    {
        get
        {
            return Time.time - segmentStartTime;
        }
    }

    float Scale
    {
        set
        {
            transform.localScale = new Vector3(value, value, value);
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        retroPixelPro = Camera.main.GetComponent<RetroPixelPro>();
        funkyTransitions = Camera.main.GetComponent<FunkyTransitions>();
        logo = GameObject.Find("Logo");

        LoadAllSegments();
        StartCoroutine(Play());
    }


    IEnumerator Play()
    {
        logo.SetActive(false);

        yield return new WaitForSeconds(1);

        ShowSegment(5);
        //StartCoroutine(Zoom(1, 1.3f, 8));
        //StartCoroutine(Translate(0, 5f, 8));
        yield return new WaitForSeconds(1);
        StartCoroutine(DoTransition(1f));
        yield return new WaitForSeconds(2);
        logo.SetActive(true);
        yield return new WaitForSeconds(4);
        StartCoroutine(FadeAlpha(1, 0, 10));
        yield return new WaitForSeconds(4);
        StartCoroutine(Zoom(logo.transform, 0.5f, 1, 2f));
        StartCoroutine(Translate(logo.transform, new Vector3(3.15f, 3.3f, 1), new Vector3(0, 0.8f, 1), 2f));

        /*
        for (int i = 0; i < 17; ++i)
        {
            ShowSegment(i);
            StartCoroutine(Zoom(1, 1.3f, 8));
            //StartCoroutine(Translate(0, 5f, 8));
            yield return new WaitForSeconds(2);
            StartCoroutine(DoTransition(0.4f));
            yield return new WaitForSeconds(6);
        }
        */

        yield return null;
    }

    IEnumerator DoTransition(float duration)
    {
        float startTime = Time.time;

        while (Time.time <= startTime + duration)
        {
            float elapsed = Time.time - startTime;
            funkyTransitions.offset = Sinerp(0, 1, elapsed / duration);
            yield return null;
        }

        funkyTransitions.offset = 1;

        yield return null;
    }

    IEnumerator Fade(float startValue, float endValue, float duration)
    {
        float startTime = Time.time;

        while (Time.time <= startTime + duration)
        {
            float elapsed = Time.time - startTime;
            retroPixelPro.opacity = Mathf.Lerp(startValue, endValue, elapsed / duration);
            yield return null;
        }

        retroPixelPro.opacity = Mathf.Lerp(startValue, endValue, 1);

        yield return null;
    }

    IEnumerator FadeAlpha(float startValue, float endValue, float duration)
    {
        float startTime = Time.time;

        while (Time.time <= startTime + duration)
        {
            float elapsed = Time.time - startTime;
            spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(startValue, endValue, elapsed / duration));
            yield return null;
        }

        retroPixelPro.opacity = Mathf.Lerp(startValue, endValue, 1);

        yield return null;
    }

    IEnumerator Zoom(Transform trans, float startScale, float endScale, float duration)
    {
        float startTime = Time.time;

        while (Time.time <= startTime + duration)
        {
            float elapsed = Time.time - startTime;
            float value = Mathf.Lerp(startScale, endScale, elapsed / duration);
            trans.localScale = new Vector3(value, value, 1);
            yield return null;
        }

        transform.localScale = new Vector3(endScale, endScale, 1);

        yield return null;
    }

    IEnumerator Translate(Transform trans, Vector3 startPos, Vector3 endPos, float duration)
    {
        float startTime = Time.time;

        while (Time.time <= startTime + duration)
        {
            float elapsed = Time.time - startTime;
            trans.position = Sinerp(startPos, endPos, elapsed / duration);
            yield return null;
        }

        trans.position = endPos;

        yield return null;
    }

    void CheckDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ++currentlyShowingIndex;
            currentlyShowingIndex %= SEGMENT_COUNT;
            ShowSegment(currentlyShowingIndex);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentlyShowingIndex = SEGMENT_COUNT + currentlyShowingIndex - 1;
            currentlyShowingIndex %= SEGMENT_COUNT;
            ShowSegment(currentlyShowingIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            retroPixelPro.enabled = !retroPixelPro.enabled;
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            ShowSegment(currentlyShowingIndex);
        }
    }


    void Update()
    {
        CheckDebugInput();
    }


    void LoadAllSegments()
    {
        segments = new List<VideoSegment>();

        for (int i = 0; i < SEGMENT_COUNT; ++i)
        {
            segments.Add(LoadSegment(i));
        }
    }


    VideoSegment LoadSegment(int i)
    {
        VideoSegment segment = new VideoSegment
        {
            sprite = Resources.Load<Sprite>("Segments/" + i + "/Texture"),
            colormap = Resources.Load<Colormap>("Segments/" + i + "/Colormap")
        };

        return segment;
    }


    void ShowSegment(int i)
    {
        Debug.Log("ShowSegment: " + i);

        spriteRenderer.sprite = segments[i].sprite;
        retroPixelPro.colormap = segments[i].colormap;
        funkyTransitions.offset = 0;

        retroPixelPro.pixelSize = 1;
        retroPixelPro.dither = 0;
        retroPixelPro.opacity = 0;
        Scale = 1;

        switch (i)
        {
            case 0:
                retroPixelPro.pixelSize = 4;
                retroPixelPro.dither = 0.3f;
                retroPixelPro.opacity = 0.38f;
                break;

            case 1:
                retroPixelPro.pixelSize = 4;
                retroPixelPro.dither = 0.285f;
                retroPixelPro.opacity = 0.5f;
                break;

            case 2:
                retroPixelPro.pixelSize = 5;
                retroPixelPro.dither = 0.369f;
                retroPixelPro.opacity = 0.771f;
                break;

            case 3:
                retroPixelPro.pixelSize = 3;
                retroPixelPro.dither = 0.0f;
                retroPixelPro.opacity = 1;
                break;

            case 4:
                retroPixelPro.pixelSize = 4;
                retroPixelPro.dither = 0.345f;
                retroPixelPro.opacity = 0.55f;
                break;

            case 5:
                retroPixelPro.pixelSize = 5;
                retroPixelPro.dither = 0.18f;
                retroPixelPro.opacity = 1f;
                break;

            case 6:
                retroPixelPro.pixelSize = 3;
                retroPixelPro.dither = 0;
                retroPixelPro.opacity = 1;
                break;

            case 7:
                retroPixelPro.pixelSize = 4;
                retroPixelPro.dither = 0;
                retroPixelPro.opacity = 0.8f;
                break;

            case 8:
                retroPixelPro.pixelSize = 6;
                retroPixelPro.dither = 0;
                retroPixelPro.opacity = 1f;
                break;

            case 9:
                retroPixelPro.pixelSize = 4;
                retroPixelPro.dither = 0;
                retroPixelPro.opacity = 1f;
                break;

            case 10:
                retroPixelPro.pixelSize = 4;
                retroPixelPro.dither = 0.165f;
                retroPixelPro.opacity = 1f;
                break;

            case 11:
                retroPixelPro.pixelSize = 4;
                retroPixelPro.dither = 0.118f;
                retroPixelPro.opacity = 1f;
                break;

            case 12:
                retroPixelPro.pixelSize = 4;
                retroPixelPro.dither = 0;
                retroPixelPro.opacity = 1f;
                break;

            case 13:
                retroPixelPro.pixelSize = 4;
                retroPixelPro.dither = 0.29f;
                retroPixelPro.opacity = 1;
                break;

            case 14:
                retroPixelPro.pixelSize = 1;
                retroPixelPro.dither = 0;
                retroPixelPro.opacity = 1;
                break;

            case 15:
                retroPixelPro.pixelSize = 1;
                retroPixelPro.dither = 0;
                retroPixelPro.opacity = 1;
                break;

            case 16:
                retroPixelPro.pixelSize = 3;
                retroPixelPro.dither = 0.842f;
                retroPixelPro.opacity = 1;
                break;

            default:
                break;

        }

        segmentStartTime = Time.time;
    }

    //Ease out
    public static float Sinerp(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
    }

    public static Vector2 Sinerp(Vector2 start, Vector2 end, float value)
    {
        return new Vector2(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)));
    }

    public static Vector3 Sinerp(Vector3 start, Vector3 end, float value)
    {
        return new Vector3(Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)), Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)), Mathf.Lerp(start.z, end.z, Mathf.Sin(value * Mathf.PI * 0.5f)));
    }
}
