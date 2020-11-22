using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageButton : MonoBehaviour {
    [SerializeField] private TimerSettingsSO _settings;
    private Image _image;

    [SerializeField] private FileBrowser _filepicker;

    public void on_click() {
        FileBrowser filepicker = Instantiate(_filepicker);
        filepicker.gameObject.SetActive(true);

        string[] filters = {".png", ".jpg", ".jpeg", ".bmp", ".tga", ".gif"};
        filepicker.show((string path) => {
            if (File.Exists(path)) {
                _settings.background_image_src = path;
            }
            
        }, filters);
    }

    private void update_sprite() {
        _image.sprite = _settings.background_image;
    }

    void OnEnable() {
        TimerSettingsSO.on_bg_image_change += update_sprite;
    }
    void OnDisable() {
        TimerSettingsSO.on_bg_image_change -= update_sprite;
    }

    void Start() {
        _image = GetComponent<Image>();
        _image.sprite = _settings.background_image;
    }
}
