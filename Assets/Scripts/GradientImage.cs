using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientImage : Image {
        private Material _material;

    public override Color color {
        set {
            _material.SetColor("_TopLeftColor", value);
            _material.SetColor("_BottomLeftColor", value);
            _material.SetColor("_TopRightColor", value);
            _material.SetColor("_BottomRightColor", value);
        }
    }
    /*public new void color (Color c) {
        _material.SetColor("_TopLeftColor", c);
        _material.SetColor("_BottomLeftColor", c);
        _material.SetColor("_TopRightColor", c);
        _material.SetColor("_BottomRightColor", c);
    }*/
    public void left_vertical(Color c) {
        _material.SetColor("_TopLeftColor", c);
        _material.SetColor("_BottomLeftColor", c);
    }
    public void right_vertical(Color c) {
        _material.SetColor("_TopRightColor", c);
        _material.SetColor("_BottomRightColor", c);
    }
    public void top_horizontal (Color c) {
        _material.SetColor("_TopLeftColor", c);
        _material.SetColor("_TopRightColor", c);
    }
    public void bottom_horizontal (Color c) {
        _material.SetColor("_BottomLeftColor", c);
        _material.SetColor("_BottomRightColor", c);
    }
    public void top_left (Color c) {
        _material.SetColor("_TopLeftColor", c);
    }
    public void top_right (Color c) {
        _material.SetColor("_TopRightColor", c);
    }
    public void bottom_left (Color c) {
        _material.SetColor("_BottomLeftColor", c);
    }
    public void bottom_right (Color c) {
        _material.SetColor("_BottomRightColor", c);
    }

    private void reset_material() {
/*        _material.SetColor("_TopLeftColor", Color.White);
        _material.SetColor("_BottomLeftColor", Color.White);
        _material.SetColor("_TopRightColor", Color.White);
        _material.SetColor("_BottomRightColor", Color.White);*/
    }

    protected override void Awake() {
        _material = material;// GetComponent<Image>().material;
    }
    
}
