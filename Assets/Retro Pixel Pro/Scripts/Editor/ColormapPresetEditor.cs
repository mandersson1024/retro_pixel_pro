using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AlpacaSound.RetroPixelPro
{

    [CustomEditor(typeof(ColormapPreset))]
    public class ColormapPresetEditor : Editor
    {
        ColormapPreset _target;

        const string MENU_ITEM_NAME = "Create New Colormap";

        void OnEnable()
        {
            _target = target as ColormapPreset;
        }


        public override void OnInspectorGUI()
        {
            DrawColors();
        }


        void DrawColors()
        {
            for (int i = 0; i < _target.palette.Length; i += 5)
            {
                EditorGUILayout.BeginHorizontal();

                for (int j = 0; j < 5; ++j)
                {
                    if (i + j < _target.palette.Length)
                    {
                        Color color = _target.palette[i + j];
                        EditorGUILayout.ColorField(GUIContent.none, color, false, false, false, null, GUILayout.Width(40), GUILayout.Height(25));
                    }
                    else
                    {
                        GUILayout.Space(46);
                    }

                    //EditorGUILayout.Space();
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
        }
    }

}


