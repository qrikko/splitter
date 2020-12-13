using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageButton : MonoBehaviour {
    private Image _image;

    [SerializeField] private FileBrowser _filepicker_prefab;
    [SerializeField] private GradientSettingsManager _settings_manager;

    public void on_click() {
        FileBrowser filepicker = Instantiate(_filepicker_prefab);
        filepicker.gameObject.SetActive(true);

        string[] filters = {".png", ".jpg", ".jpeg", ".bmp", ".tga", ".gif"};
        filepicker.show((string path) => {
            if (File.Exists(path)) {
                byte[] fileData = File.ReadAllBytes(path);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

                _image.sprite =  Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
            }
            _settings_manager.image_path = path;
        }, filters);
    }

    private void update_sprite() {
        //_image.sprite = _settings.background_image;
    }

    void OnEnable() {
        _image.sprite = _settings_manager.settings.background_image;
    }

    void Awake() {
        _image = GetComponent<Image>();
        //_image.sprite = _settings.background_image;
    }
}
