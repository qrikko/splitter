using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Resolution))]
public class ResolutionEditor : Editor {
    public override void OnInspectorGUI() {
        Resolution resolution = (Resolution)target;

        resolution._tween = EditorGUILayout.Toggle("Show Tweened", resolution._tween);
        resolution._type = (Resolution.WindowType) EditorGUILayout.EnumPopup(resolution._type);

        GUI.enabled = false;
        switch(resolution._type) {
            case Resolution.WindowType.Tall:
                resolution._width = 365;
                resolution._height = 700;
            break;
            case Resolution.WindowType.Wide:
            case Resolution.WindowType.Settings:
                resolution._width = 625;
                resolution._height = 450;
            break;
            case Resolution.WindowType.Free:
                GUI.enabled = true;
            break;
        }
        
        resolution._width = EditorGUILayout.IntField("Width", resolution._width);
        resolution._height = EditorGUILayout.IntField("Height", resolution._height);
        GUI.enabled = true;
    }    
}
