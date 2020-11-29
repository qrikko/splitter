using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageButton : MonoBehaviour {
    [SerializeField] private TimerSettingsSO _settings;
    private Image _image;

    [SerializeField] private FileBrowser _filepicker;
    [SerializeField] private Image _bg;

    public void on_click() {
        FileBrowser filepicker = Instantiate(_filepicker);
        filepicker.gameObject.SetActive(true);

        string[] filters = {".png", ".jpg", ".jpeg", ".bmp", ".tga", ".gif"};
        filepicker.show((string path) => {
            if (File.Exists(path)) {
                _settings.background_image_src = path;
                _bg.sprite = _settings.background_image;
                _image.sprite = _settings.background_image;
            }
            
        }, filters);
    }

    private void update_sprite() {
        _image.sprite = _settings.background_image;
    }

    void Start() {
        _image = GetComponent<Image>();
        _image.sprite = _settings.background_image;
    }
}
