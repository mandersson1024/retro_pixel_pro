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
    static int SEGMENT_COUNT = 19;

    List<VideoSegment> segments;

    SpriteRenderer spriteRenderer;
    Sprite sprite;
    RetroPixelPro retroPixelPro;

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

        LoadAllSegments();
        StartCoroutine(Play());
    }


    IEnumerator Play()
    {
        {
            ShowSegment(0);

            float totalTime = 6;
            //float startScale = 1f;
            //float endScale = 2f;

            while (SegmentRunningTime < totalTime)
            {
                //Scale = Mathf.Lerp(startScale, endScale, SegmentRunningTime / totalTime);
                yield return null;
            }
        }

        yield return null;
    }


    void Update()
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
                retroPixelPro.opacity = 0.4f;
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

            case 13:
                retroPixelPro.pixelSize = 4;
                retroPixelPro.dither = 0.29f;
                retroPixelPro.opacity = 1;
                break;

            default:
                break;

        }

        segmentStartTime = Time.time;
    }

}
