using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    private enum ButtonType{
        Solid,TopLeft,TopRight,BottomLeft,BottomRight,Top,Bottom,Left,Right
    };

    [SerializeField] private ButtonType _type;
    [SerializeField] GradientSettingsManager _settings_manager;

    private Image _image;

    void OnEnable() {
        var s = _settings_manager.settings;
        switch(_type) {
            case ButtonType.Solid:
                _image.color = s.color;
            break;
            case ButtonType.Top:
                _image.color = s.vertical_gradient.top;
            break;
            case ButtonType.TopLeft:
                _image.color = s.gradient.tl;
            break;
            case ButtonType.Left:
                _image.color = s.horizontal_gradient.left;
            break;
            case ButtonType.Right:
                _image.color = s.horizontal_gradient.right;
            break;
            case ButtonType.TopRight:
                _image.color = s.gradient.tr;
            break;
            case ButtonType.BottomLeft:
                _image.color = s.gradient.bl;
            break;
            case ButtonType.Bottom:
                _image.color = s.vertical_gradient.bottom;
            break;
            case ButtonType.BottomRight:
                _image.color = s.gradient.br;
            break;
        }
        //_settings_manager.settings;
    }

    void Awake() {
        _image = GetComponent<Image>();
    }
}
