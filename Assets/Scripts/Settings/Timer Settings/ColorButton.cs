using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    private Image _image;
    [SerializeField] private TimerSettingsSO _settings;

    private void update_color() {
        _image.color = _settings.background_color;
    }

    void OnEnable() {
        TimerSettingsSO.on_bg_color_change += update_color;
    }
    void OnDisable() {
        TimerSettingsSO.on_bg_color_change -= update_color;
    }

    void Start() {
        _image = GetComponent<Image>();
        _image.color = _settings.background_color;
    }
}
