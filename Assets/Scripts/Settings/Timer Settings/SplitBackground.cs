using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitBackground : MonoBehaviour
{
    [SerializeField] TimerSettingsSO _settings;
    private Image _image;



    private void update_color() {
        //_image.color = _settings.background_color;
    }
    private void update_image() {
        _image.sprite = _settings.background_image;
    }    

    void Awake() {
        _image = GetComponent<Image>();
        //_image.color = _settings.background_color;
        _image.sprite = _settings.background_image;
    }
}
