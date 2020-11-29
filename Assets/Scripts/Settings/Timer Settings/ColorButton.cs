using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    private GradientImage _image;
    [SerializeField] private GradientImage _target;

    private enum ButtonType {
        color, left, right, top, bottom, top_left, top_right, bottom_left, bottom_right
    };
    [SerializeField] private ButtonType _type;

    private void update_color() {
        //_image.color = _settings.background_color;
    }

    void OnEnable() {
        Color c = GetComponent<Image>().color;
        switch (_type) {
            case ButtonType.color:
                _target.color = c;
                break;
            case ButtonType.left:
                _target.left_vertical(c);
                break;
            case ButtonType.right:
                _target.right_vertical(c);
                break;
            case ButtonType.top:
                _target.top_horizontal(c);
                break;
            case ButtonType.bottom:
                _target.bottom_horizontal(c);
                break;
            case ButtonType.top_left:
                _target.top_left(c);
                break;
            case ButtonType.top_right:
                _target.top_right(c);
                break;
            case ButtonType.bottom_left:
                _target.bottom_left(c);
                break;
            case ButtonType.bottom_right:
                _target.bottom_right(c);
            break;
        }
    }

    void Start() {
    //    _image = GetComponent<Image>();
    //    _image.color = _settings.background_color;
    }
}
