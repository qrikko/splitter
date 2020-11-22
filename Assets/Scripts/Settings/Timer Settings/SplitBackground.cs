using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitBackground : MonoBehaviour
{
    [SerializeField] TimerSettingsSO _settings;

    private Image _image;

    private void update_color() {
        _image.color = _settings.background_color;
    }
    private void update_image() {
        _image.sprite = _settings.background_image;
    }

    void OnEnable() {
        TimerSettingsSO.on_bg_color_change += update_color;
        TimerSettingsSO.on_bg_image_change += update_image;
    }

    void OnDisable() {
        TimerSettingsSO.on_bg_color_change -= update_color;
        TimerSettingsSO.on_bg_image_change -= update_image;
    }

    void Awake() {
        _image = GetComponent<Image>();
        _image.color = _settings.background_color;
        _image.sprite = _settings.background_image;
    }
}
