using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontSettingsManager : MonoBehaviour {
    [SerializeField] private FontSettings _font_settings;
    [SerializeField] private TMP_Dropdown _font_select;
    
    public int font {
        set { _font_settings.font = value; }
    }
    void OnEnable() {
        _font_select.options.Clear();
        _font_select.AddOptions(new List<string>(_font_settings.fonts));
    }
}
