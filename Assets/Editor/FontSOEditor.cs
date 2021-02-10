using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FontSettingsSO))]
public class FontSOEditor : Editor {    
    public override void OnInspectorGUI () {
        EditorGUILayout.LabelField( "Font Family: ");
        
        FontSettingsSO f = (FontSettingsSO)target;
        f.index = EditorGUILayout.Popup(f.index, f.fonts);
    }
}
