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
        SerializedProperty dither;
        SerializedProperty colormap;


        void OnEnable()
        {
            serObj = new SerializedObject(target);

            resolutionMode = serObj.FindProperty("resolutionMode");
            resolution = serObj.FindProperty("resolution");
            pixelSize = serObj.FindProperty("pixelSize");
            opacity = serObj.FindProperty("opacity");
            dither = serObj.FindProperty("dither");
            colormap = serObj.FindProperty("colormap");
        }

        public override void OnInspectorGUI()
        {
            serObj.Update();

            resolutionMode.enumValueIndex = (int)(ResolutionMode)EditorGUILayout.EnumPopup("Resolution Mode", (ResolutionMode)resolutionMode.enumValueIndex);

            if (resolutionMode.enumValueIndex == (int)ResolutionMode.ConstantResolution)
            {
                resolution.vector2IntValue = EditorGUILayout.Vector2IntField("Resolution", resolution.vector2IntValue);
            }

            if (resolutionMode.enumValueIndex == (int)ResolutionMode.ConstantPixelSize)
            {
                pixelSize.intValue = EditorGUILayout.IntField("Pixel Size", pixelSize.intValue);
            }

            dither.floatValue = EditorGUILayout.Slider("Dither", dither.floatValue, 0, 1);
            opacity.floatValue = EditorGUILayout.Slider("Opacity", opacity.floatValue, 0, 1);
            colormap.objectReferenceValue = EditorGUILayout.ObjectField("Colormap", colormap.objectReferenceValue, typeof(Colormap), false);

            serObj.ApplyModifiedProperties();
        }
    }


}

