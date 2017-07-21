using UnityEngine;
using UnityEditor;


namespace AlpacaSound.RetroPixelPro
{
    [CustomEditor(typeof(RetroPixelPro))]
    public class RetroPixelProEditor : Editor
    {
        SerializedObject serObj;

        SerializedProperty resolutionMode;
        SerializedProperty resolution;
        SerializedProperty pixelSize;
        SerializedProperty opacity;
        SerializedProperty colormap;


        void OnEnable()
        {
            serObj = new SerializedObject(target);

            resolutionMode = serObj.FindProperty("resolutionMode");
            resolution = serObj.FindProperty("resolution");
            pixelSize = serObj.FindProperty("pixelSize");
            opacity = serObj.FindProperty("opacity");
            colormap = serObj.FindProperty("colormap");
        }

        public override void OnInspectorGUI()
        {
            serObj.Update();

            resolutionMode.enumValueIndex = (int)(ResolutionMode)EditorGUILayout.EnumPopup("Resolution Mode", (ResolutionMode)resolutionMode.enumValueIndex);

            if (resolutionMode.enumValueIndex == (int)ResolutionMode.ConstantResolution)
            {
                resolution.vector2Value = EditorGUILayout.Vector2Field("Resolution", resolution.vector2Value);
            }

            if (resolutionMode.enumValueIndex == (int)ResolutionMode.ConstantPixelSize)
            {
                pixelSize.intValue = EditorGUILayout.IntField("Pixel Size", pixelSize.intValue);
            }

            opacity.floatValue = EditorGUILayout.Slider("Opacity", opacity.floatValue, 0, 1);
            colormap.objectReferenceValue = EditorGUILayout.ObjectField("Colormap", colormap.objectReferenceValue, typeof(Colormap), false);

            serObj.ApplyModifiedProperties();
        }
    }


}

