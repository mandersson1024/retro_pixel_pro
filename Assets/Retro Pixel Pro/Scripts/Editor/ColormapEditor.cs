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


        const string MENU_ITEM_NAME = "Create New Colormap";

        [MenuItem("Retro Pixel Pro/" + MENU_ITEM_NAME)]
        static void CreateNewColormap()
        {
            string path = EditorUtility.SaveFilePanel(MENU_ITEM_NAME, "Assets/", "New Colormap.asset", "asset");

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            path = FileUtil.GetProjectRelativePath(path);

            Colormap colormap = CreateInstance<Colormap>();

            ColormapPreset preset = FileUtils.LoadColormapPreset("Classic6.asset");
            //Debug.Log("preset: " + preset);
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
                UpdateColormap();
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
                UpdateColormap();
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

            DrawExtractPalette();

            EditorGUILayout.Space();

            DrawApplyChanges();

            EditorGUILayout.Space();

            DrawColors();
        }


        void DrawExtractPalette()
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
                    UpdateColormap();
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
                            Color newColor = EditorGUILayout.ColorField(GUIContent.none, color, false, false, false, null, GUILayout.Width(40), GUILayout.Height(25));
                            _target.palette[i + j] = newColor;

                            if (oldColor != newColor)
                            {
                                dirty.forceDirty = true;
                            }
                        }
                        else
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUILayout.ColorField(GUIContent.none, DisabledColor(color), false, false, false, null, GUILayout.Width(40), GUILayout.Height(25));
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


        Color DisabledColor(Color color)
        {
            return Color.Lerp(Color.white, color, 0.5f);
        }


        public void UpdateColormap()
        {
            isUpdatingColormap = true;
            calculator = new ColormapCalculator(_target.colormapPrecision, _target.palette, _target.usedColors, _target.numberOfColors, DoneUpdatingColormap);
        }


        public void CancelColormapUpdate()
        {
            isUpdatingColormap = false;
        }


        void DoneUpdatingColormap()
        {
            isUpdatingColormap = false;
            _target.texture3Dpixels = calculator.pixelBuffer;
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


        void colormapSynchronousUpdate()
        {
            while (isUpdatingColormap)
            {
                calculator.CalculateChunk();
            }
        }


    }

}


