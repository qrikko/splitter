using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Events;

public class GradientSettingsManager : MonoBehaviour {
    [SerializeField] private GradientImage _gradient_image;
    [SerializeField] private TMP_Dropdown _picker;
    [SerializeField] private ColorManager _color_manager;
    public UnityEvent<float> AlphaSliderEnable;

    private GradientSettingsSO _settings;
    
    public GradientSettingsSO settings {
        get { return _settings; }
    }

    public void save_settings() {
        _gradient_image.save();
    }

    public string image_path {
        set {     
            _gradient_image.image_path = value;
        }
    }

    public float alpha {
        get { return _gradient_image.alpha; }
        set { 
            _gradient_image.alpha = value;
        }
    }

    public Color color {
        //get {return Color.white;}
        set {
            _gradient_image.color = value;
        }
    }

    public Color left_color {
        set {
            _gradient_image.left = value;
        }
    }
    public Color right_color {
        set {
            _gradient_image.right = value;
        }
    }
    public Color top_color {
        set {
            _gradient_image.top = value;
        }
    }
    public Color bottom_color {
        set {
            _gradient_image.bottom = value;
        }
    }
    public Color top_left_color {
        set {
            _gradient_image.top_left = value;
        }
    }
    public Color top_right_color {
        set {
            _gradient_image.top_right = value;
        }
    }
    public Color bottom_left_color {
        set {
            _gradient_image.bottom_left = value;
        }
    }
    public Color bottom_right_color {
        set {
            _gradient_image.bottom_right = value;
        }
    }

    public int gradient_type {
        set {
            _gradient_image.color_type = value;
        }
    }
    
    void OnEnable() {
        _settings = _gradient_image.settings;
        _picker.value = (int)_settings.color_type;
        _color_manager.current_id = _picker.value;

       // AlphaSliderEnable.Invoke(alpha);
    }

    void Awake () {
        _settings = _gradient_image.settings;
    }
}
