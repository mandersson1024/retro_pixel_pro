using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace AlpacaSound.RetroPixelPro
{
    public class AutomatedTests : MonoBehaviour
    {

        Colormap colormap1;
        Colormap colormap2;

        RetroPixelPro retroPixel;

        
        Stepper stepper;
        Text logText;

        void Start()
        {
            colormap1 = FileUtils.LoadColormap("Default.asset");
            colormap2 = FileUtils.LoadColormap("Monochrome/BlackAndWhite.asset");

            stepper = new Stepper(Log);
            logText = GameObject.Find("LogText").GetComponent<Text>();

            Log("Click mouse to step through the tests");

            stepper.AddStep(AddComponentToCamera);
            stepper.AddStep(SetColormap(colormap1));
            stepper.AddStep(SetResolutionMode(ResolutionMode.ConstantResolution));
            stepper.AddStep(SetResolution(160, 100));
            stepper.AddStep(SetResolution(Screen.width, Screen.height));
            stepper.AddStep(SetResolutionMode(ResolutionMode.ConstantPixelSize));
            stepper.AddStep(SetPixelSize(10));
            stepper.AddStep(SetPixelSize(1));
            stepper.AddStep(SetOpacity(0));
            stepper.AddStep(SetOpacity(0.5f));
            stepper.AddStep(SetOpacity(1));
            stepper.AddStep(SetComponentEnabled(false));
            stepper.AddStep(SetComponentEnabled(true));
            stepper.AddStep(SetColormap(null));
            stepper.AddStep(SetColormap(colormap1));
            stepper.AddStep(SetColormap(colormap2));
            stepper.AddStep(LoadAndSetColormapResource);
            stepper.AddStep(InstantiateCameraPrefab);
        }


        void Log(string text)
        {
            logText.text = text;
        }

        void AddComponentToCamera()
        {
            Log("Adding Retro Pixel Pro component to Main Camera");
            retroPixel = GameObject.Find("Main Camera").AddComponent<RetroPixelPro>();
        }


        System.Action SetResolutionMode(ResolutionMode mode)
        {
            return () =>
            {
                Log("retroPixel resolutionMode = " + mode);
                retroPixel.resolutionMode = mode;
            };
        }

        System.Action SetResolution(int width, int height)
        {
            return () =>
            {
                Log("retroPixel resolution = (" + width + "," + height + ")");
                retroPixel.resolution.Set(width, height);
            };
        }

        System.Action SetPixelSize(int size)
        {
            return () =>
            {
                Log("retroPixel.pixelSize = " + size);
                retroPixel.pixelSize = size;
            };
        }

        System.Action SetOpacity(float opacity)
        {
            return () =>
            {
                Log("retroPixel.opacity = " + opacity);
                retroPixel.opacity = opacity;
            };
        }

        System.Action SetColormap(Colormap colormap)
        {
            return () =>
            {
                Log("retroPixel.colormap = " + (colormap == null ? "null" : colormap.name));
                retroPixel.colormap = colormap;
            };
        }

        System.Action SetComponentEnabled(bool enabled)
        {
            return () =>
            {
                Log("retroPixel.enabled = " + enabled);
                retroPixel.enabled = enabled;
            };
        }

        void LoadAndSetColormapResource()
        {
            Log("Loading and setting colormap from resources");
            Colormap loaded = Resources.Load("AutomatedTestColormap") as Colormap;
            retroPixel.colormap = loaded;
        }

        void InstantiateCameraPrefab()
        {
            Log("Instantiating Camera prefab.");
            Instantiate(Resources.Load("AutomatedTestCamera") as GameObject);
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                stepper.Step();
            }
        }

    }


    class Stepper
    {
        List<System.Action> actions = new List<System.Action>();
        System.Action<string> logger;

        public Stepper(System.Action<string> logger)
        {
            this.logger = logger;
        }

        public void AddStep(System.Action action)
        {
            actions.Add(action);
        }

        public void Step()
        {
            if (actions.Count == 0)
            {
                logger.Invoke("No more steps");
            }
            else
            {
                System.Action action = actions[0];
                action.Invoke();
                actions.RemoveAt(0);
            }
        }
    }
}

