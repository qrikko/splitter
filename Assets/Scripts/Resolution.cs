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
        float w_curr = w;
        float h_curr = h;
        while(w <= _width && h <= _height){
            yield return new WaitForEndOfFrame();
            
            w_curr = System.Math.Max(w_curr+_tween_speed*Time.deltaTime, _width);
            h_curr = System.Math.Max(h_curr+_tween_speed*Time.deltaTime, _height);
            w = (int)w_curr;
            h = (int)h_curr;
            if (w_curr > w+1 || h_curr > h+1) {
                Screen.SetResolution(
                    w,
                    h,
                    false
                );
            }
            _tween_transform.sizeDelta = new Vector2(w_curr-365,h_curr);
        }
    }

    void OnEnable() {
        // I wanted to animate the change, but it doesn't work so I'll keep the code if I want to give it another go..
        /*
        if (_tween) {
            int w = Screen.width;
            int h = Screen.height;
            StartCoroutine(tween(w, h));
        } else {
        */
            Screen.SetResolution(_width, _height, false);
        //}
    }

    void OnDisable() {
        if (_tween) {
            // not sure where to do this.. kind of want to hijack the disable and perform the tween-hide and only really
            // disable once it is done.
        }
    }
}
