using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour {
    public enum WindowType {
        Tall,
        Wide,
        Settings,
        Free
    }

    public bool _tween = false;
    [Range(1, 10)] public int _tween_speed = 1;

    public WindowType _type;

    public int _width;
    public int _height;

    IEnumerator tween(int w, int h) {
        RectTransform _tween_transform = GetComponent<RectTransform>();
        while(w <= _width && h <= _height){
            yield return new WaitForSeconds(2.2f);
            
            w = System.Math.Max(w+_tween_speed, _width);
            h = System.Math.Max(h+_tween_speed, _height);
            Screen.SetResolution(
                w,
                h,
                false
            );
            _tween_transform.sizeDelta = new Vector2(w-365,h);
        }
    }

    void OnEnable() {
        if (_tween) {
            int w = Screen.width;
            int h = Screen.height;
            StartCoroutine(tween(w, h));
        } else {
            Screen.SetResolution(_width, _height, false);
        }
    }

    void OnDisable() {
        if (_tween) {
            // not sure where to do this.. kind of want to hijack the disable and perform the tween-hide and only really
            // disable once it is done.
        }
    }
}
