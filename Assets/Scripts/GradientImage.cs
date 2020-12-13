using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using Newtonsoft.Json;

[RequireComponent(typeof(Image))]
public class GradientImage : MonoBehaviour {
    [SerializeField] private GradientSettingsSO _settings;
    private Material _material;
    private Image _image;

    public float alpha {
        get { return _settings.alpha; }
        set {
            _settings.alpha = value; 
            Color tmp = _image.color;
            tmp.a = value;
            _image.color = tmp;
        }
    }

    public GradientSettingsSO settings {
        get { return _settings; }
    }

    public string image_path {
        set {
            _settings.image_src = value;
            _image.sprite = _settings.background_image;
        }
    }
    public Color color {
        set {            
            if (_material == null ) { return ;}
            _material.SetColor("_TopLeftColor", value);
            _material.SetColor("_BottomLeftColor", value);
            _material.SetColor("_TopRightColor", value);
            _material.SetColor("_BottomRightColor", value);
            
            _settings.color = value;            
        }        
    }
    public Color left {
        set {
            if (!_material) {return;}
            _material.SetColor("_TopLeftColor", value);
            _material.SetColor("_BottomLeftColor", value);

            _settings.horizontal_gradient.left = value;
        }
    }
    public Color right {
        set {      
            if (!_material) {return;}      
            _material.SetColor("_TopRightColor", value);
            _material.SetColor("_BottomRightColor", value);
            
            _settings.horizontal_gradient.right = value;
        }
    }
    public Color top {
        set {            
            _material.SetColor("_TopLeftColor", value);
            _material.SetColor("_TopRightColor", value);

            _settings.vertical_gradient.top = value;
        }
    }
    public Color bottom {
        set {
            _material.SetColor("_BottomLeftColor", value);
            _material.SetColor("_BottomRightColor", value);

            _settings.vertical_gradient.bottom = value;
        }
    }
    public Color top_left {
        set {
            _material.SetColor("_TopLeftColor", value);
            _settings.gradient.tl = value;
        }
    }
    public Color top_right {
        set {
            _material.SetColor("_TopRightColor", value);
            _settings.gradient.tr = value;
        }
    }
    public Color bottom_left {
        set {
            _material.SetColor("_BottomLeftColor", value);
            _settings.gradient.bl = value;            
        }
    }
    public Color bottom_right {
        set {
            _material.SetColor("_BottomRightColor", value);
            _settings.gradient.br = value;            
        }
    }

    private void update_color_type() {
        switch(_settings.color_type) {
            case GradientSettingsSO.ColorType.Solid:
                color = _settings.color;
            break;
            case GradientSettingsSO.ColorType.VerticalGradient:
                top = _settings.vertical_gradient.top;                
                bottom = _settings.vertical_gradient.bottom;
            break;
            case GradientSettingsSO.ColorType.HorizontalGradient:
                left = _settings.horizontal_gradient.left;
                right = _settings.horizontal_gradient.right;
            break;
            case GradientSettingsSO.ColorType.CornerGradiant:
                top_left = _settings.gradient.tl;
                top_right = _settings.gradient.tr;
                bottom_left = _settings.gradient.bl;
                bottom_right = _settings.gradient.br;
            break;
        }
    }
    public int color_type {
        set {
            _settings.color_type = (GradientSettingsSO.ColorType)value;
            update_color_type();
        }
    }

    void OnEnable() {
        //_image.color = _settings.color;
        _image.sprite = _settings.background_image;
        update_color_type();
        alpha = _settings.alpha;
    }

    private void reset_material() {
/*        _material.SetColor("_TopLeftColor", Color.White);
        _material.SetColor("_BottomLeftColor", Color.White);
        _material.SetColor("_TopRightColor", Color.White);
        _material.SetColor("_BottomRightColor", Color.White);*/
    }

    protected void Awake() {        
        _image = GetComponent<Image>();
        _material = _image.material;

        initialize();
    }

    private void initialize() {
        string id = PlayerPrefs.GetString("active_game");
        string settings_folder = Path.GetDirectoryName(PlayerPrefs.GetString(id)) + "/settings/";
        string settings_file = settings_folder + gameObject.name + ".json";
        string json = File.ReadAllText(settings_file);

        JsonUtility.FromJsonOverwrite(json, _settings);
    }

    public void save() {
        // also need the splits info, so we can put it into the propper folder..
        string id = PlayerPrefs.GetString("active_game");
        string splitpath = Path.GetDirectoryName(PlayerPrefs.GetString(id));
        string settings_folder = splitpath + "/settings/";
        string settings_file = settings_folder + gameObject.name + ".json";
        string json = JsonUtility.ToJson(_settings);
        
        if (!Directory.Exists(settings_folder)) {
            Directory.CreateDirectory(Path.GetDirectoryName(settings_folder));
        }

        FileStream fs = new FileStream(settings_file, FileMode.Create);

        StreamWriter sw = new StreamWriter(fs);
        sw.Write(json);
        sw.Close();
        fs.Close();
        Debug.Log("settings saved: " + settings_file);
    }    
}
