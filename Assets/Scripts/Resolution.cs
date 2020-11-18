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

    public WindowType _type;

    public int _width;
    public int _height;

    void Awake() {
        Screen.SetResolution(_width, _height, false);        
    }
}
