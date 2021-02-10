using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class FontSettings : MonoBehaviour {
    [SerializeField] private FontSettingsSO _settings;

    private TMP_Text _text;

    public int font {
        set {
            _settings.index = value;
            update_font();    
        }
    }
    public string[] fonts {
        get {return _settings.fonts;}
    }

    public void update_font() {
        Font font = new Font(_settings.font);        
        _text.font = TMP_FontAsset.CreateFontAsset(font);
    }

    //void Update() {
    //    update_font();
    //}

    void Awake () {
        _text = GetComponent<TMP_Text>();        
    }
}
