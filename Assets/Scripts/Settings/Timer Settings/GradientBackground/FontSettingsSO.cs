using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Settings", menuName="Settings/Font Settings", order=3)]
public class FontSettingsSO : ScriptableObject {
    
    private string[] _fonts;
    public string[] fonts { get {return _fonts; }}

    private int _index;
    public int index { 
        get {return _index;} 
        set {
            _index = value;

        }
    }


    public string font {
        get { return Font.GetPathsToOSFonts()[_index]; }
    }

    void OnEnable() {
        _fonts = Font.GetOSInstalledFontNames();
    }
    
}
