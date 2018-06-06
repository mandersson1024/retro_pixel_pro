using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AlpacaSound.RetroPixelPro
{

    [CustomEditor(typeof(Colormap))]
    public class ColormapEditor : Editor
    {

        Colormap _target;
        ColormapCalculator calculator;
        bool isUpdatingColormap;
        ColormapDirtyCheck dirty;
        bool autoApplyChanges;
        string paletteImagePath;
        GenericMenu presetMenu;

        //float debugUpdateStartTime;


        const string MENU_ITEM_NAME = "Create New Colormap";

        [MenuItem("Retro Pixel Pro/" + MENU_ITEM_NAME)]
        static void CreateNewColormap()
        {
            string path = EditorUtility.SaveFilePanelInProject(MENU_ITEM_NAME, "New Colormap", "asset", MENU_ITEM_NAME);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            Colormap colormap = CreateInstance<Colormap>();
            ColormapPreset preset = FileUtils.LoadColormapPreset("Classics/Breadbox 1982.asset");
            colormap.ApplyPreset(preset);

            AssetDatabase.CreateAsset(colormap, path);
            AssetDatabase.SaveAssets();
        }


        void OnEnable()
        {
            _target = target as Colormap;
            dirty = new ColormapDirtyCheck(_target);
            autoApplyChanges = true;

            //usedColors = serializedObject.FindProperty("usedColors");

            EditorApplication.update += OnEditorUpdate;

            if (!_target.initialized)
            {
                Debug.Log("Initializing colormap '" + _target.name + "'");

                _target.initialized = true;
                StartUpdatingColormap();
            }

            presetMenu = new GenericMenu();
            DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/Retro Pixel Pro/Colormap Presets");
            FileUtils.AddFilesInDirectory(dirInfo, presetMenu, "", PresetMenuCallback);
        }


        void PresetMenuCallback(object obj)
        {
            string presetName = obj as string;
            ColormapPreset preset = FileUtils.LoadColormapPreset(presetName);
            //Debug.Log("preset: " + preset);
            _target.ApplyPreset(preset);
            dirty.forceDirty = true;
        }


        void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }


        public override void OnInspectorGUI()
        {
            if (!isUpdatingColormap && autoApplyChanges && dirty.IsDirty())
            {
                StartUpdatingColormap();
            }

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(Application.isPlaying);

            EditorGUI.BeginDisabledGroup(isUpdatingColormap);

            DrawDefaultInspector();

            EditorGUI.EndDisabledGroup();

            DrawStaticProperties();

            EditorGUI.EndDisabledGroup();

            serializedObject.ApplyModifiedProperties();
        }


        void DrawStaticProperties()
        {
            EditorGUILayout.Space();

            if (GUILayout.Button("Select Preset", GUILayout.Width(200), GUILayout.Height(28)))
            {
                presetMenu.ShowAsContext();
            }

            if (GUILayout.Button("Save As Preset", GUILayout.Width(200), GUILayout.Height(28)))
            {
                SaveAsPreset();
            }

            DrawExtractPaletteMedianCut();

            EditorGUILayout.Space();

            DrawApplyChanges();

            EditorGUILayout.Space();

            DrawColors();
        }


        void SaveAsPreset()
        {
            string folderPath = Application.dataPath + "/Retro Pixel Pro/Colormap Presets/Custom";
            string path = EditorUtility.SaveFilePanel("Save Colormap Preset", folderPath, "New Colormap Preset", "asset");

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            int numUsedColor = 0;
            for (int i = 0; i < _target.usedColors.Length; ++i)
            {
                if (_target.usedColors[i])
                {
                    ++numUsedColor;
                }
            }

            ColormapPreset preset = CreateInstance<ColormapPreset>();
            preset.SetNumColors(numUsedColor);


            int j = 0;
            for (int i = 0; i < _target.numberOfColors; ++i)
            {
                if (_target.usedColors[i])
                {
                    preset.palette[j] = _target.palette[i];
                    ++j;
                }
            }

            AssetDatabase.CreateAsset(preset, path);
            AssetDatabase.SaveAssets();
        }


        void DrawExtractPaletteMedianCut()
        {
            if (GUILayout.Button("Extract Palette From Image", GUILayout.Width(200), GUILayout.Height(28)))
            {
                if (paletteImagePath == null)
                {
                    paletteImagePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                }

                string imagePath = EditorUtility.OpenFilePanelWithFilters("Select Image File", paletteImagePath, new string[]
                    {
                        "JPG Image", "jpg",
                        "PNG Image", "png",
                    });

                if (imagePath.Length > 0)
                {
                    paletteImagePath = imagePath;
                    List<Color32> extractedPalette = PaletteExtractor.ExtractPalette(paletteImagePath, _target.numberOfColors);
                    _target.SetColors(extractedPalette);
                    dirty.forceDirty = true;
                }
            }
        }


        void DrawApplyChanges()
        {
            autoApplyChanges = EditorGUILayout.ToggleLeft(" Apply Changes Automatically", autoApplyChanges);

            EditorGUI.BeginDisabledGroup(!dirty.IsDirty() || autoApplyChanges);
            if (!isUpdatingColormap)
            {
                if (GUILayout.Button("Apply Changes", GUILayout.Width(200), GUILayout.Height(28)))
                {
                    StartUpdatingColormap();
                }
            }
            EditorGUI.EndDisabledGroup();

            if (isUpdatingColormap)
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Cancel", GUILayout.Width(130), GUILayout.Height(28)))
                {
                    autoApplyChanges = false;
                    CancelColormapUpdate();
                }

                Rect progressRect = GUILayoutUtility.GetRect(0, 32, GUILayout.ExpandWidth(true));
                EditorGUI.ProgressBar(progressRect, calculator.progress, "Updating Colormap");
                EditorUtility.SetDirty(target);

                EditorGUILayout.EndHorizontal();
            }
        }


        void DrawColors()
        {
            EditorGUILayout.Space();

            for (int i = 0; i < _target.numberOfColors; i += 4)
            {
                EditorGUILayout.BeginHorizontal();

                for (int j = 0; j < 4; ++j)
                {
                    if (i + j < _target.numberOfColors)
                    {
                        bool oldUsed = _target.usedColors[i + j];
                        bool newUsed = EditorGUILayout.Toggle(oldUsed, GUILayout.Width(15));
                        _target.usedColors[i + j] = newUsed;

                        if (oldUsed != newUsed)
                        {
                            dirty.forceDirty = true;
                        }

                        Color color = _target.palette[i + j];

                        if (oldUsed)
                        {
                            Color oldColor = _target.palette[i + j];
                            Color newColor = EditorGUILayout.ColorField(GUIContent.none, color, false, false, false, GUILayout.Width(40), GUILayout.Height(25));

                            if (!dirty.forceDirty && oldColor != newColor)
                            {
                                _target.palette[i + j] = newColor;
                                dirty.forceDirty = true;
                            }
                        }
                        else
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            //EditorGUILayout.ColorField(GUIContent.none, DisabledColor(color), false, false, false, GUILayout.Width(40), GUILayout.Height(25));
                            EditorGUILayout.ColorField(GUIContent.none, color, false, false, false, GUILayout.Width(40), GUILayout.Height(25));
                            EditorGUI.EndDisabledGroup();
                        }
                    }
                    else
                    {
                        GUILayout.Space(67);
                    }

                    EditorGUILayout.Space();
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }


        /*
		Color DisabledColor(Color color)
		{
			return Color.Lerp(Color.white, color, 0.5f);
		}
		*/


        public void StartUpdatingColormap()
        {
            isUpdatingColormap = true;
            calculator = new ColormapCalculator(_target.preview, _target.palette, _target.usedColors, _target.numberOfColors, DoneUpdatingColormap);
            //debugUpdateStartTime = Time.time;
        }


        public void CancelColormapUpdate()
        {
            isUpdatingColormap = false;
        }

        void DoneUpdatingColormap()
        {
            //Debug.Log("DoneUpdatingColormap, time: " + (Time.time - debugUpdateStartTime));

            isUpdatingColormap = false;
            _target.pixels = calculator.pixelBuffer;
            //_target.ApplyToMaterial();
            AssetDatabase.SaveAssets();

            dirty.Reset();
            _target.changedInternally = true;
        }


        public void OnEditorUpdate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (isUpdatingColormap)
            {
                if (calculator != null)
                {
                    calculator.CalculateChunk();
                }
            }
        }


        /*
		void ColormapSynchronousUpdate()
		{
			while (isUpdatingColormap)
			{
				calculator.CalculateChunk();
			}
		}
		*/


    }

}


