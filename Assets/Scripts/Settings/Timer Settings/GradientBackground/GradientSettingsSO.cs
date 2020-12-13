using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

[System.Serializable]
public class Gradient{
    public Color tl = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    public Color tr = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    public Color bl = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    public Color br = new Color(1.0f, 1.0f, 1.0f, 0.0f);
}

[System.Serializable]
public class VerticalGradient {
    public Color top = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    public Color bottom = new Color(1.0f, 1.0f, 1.0f, 0.0f);
}

[System.Serializable]
public class HorizontalGradient {
    public Color left = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    public Color right = new Color(1.0f, 1.0f, 1.0f, 0.0f);
}

[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Gradient Settings", order = 2)]
public class GradientSettingsSO : ScriptableObject {
    public enum ColorType { Solid, VerticalGradient, HorizontalGradient, CornerGradiant }
    
    public Gradient gradient;
    public VerticalGradient vertical_gradient;
    public HorizontalGradient horizontal_gradient;
    public Color color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    public string image_src = "";
    public bool keep_ratio;
    [Range(0,1)] public float alpha = 1.0f;

    public ColorType color_type;
    
    public Color background_color {
        get { return color;}
        set { color = value; }
    }

    public Sprite background_image {
        get {
            if (File.Exists(image_src)) {
                byte[] fileData = File.ReadAllBytes(image_src);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

                return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
            }
            return null;
        }
    }

    public string background_image_src {
        get { return image_src; }
        set { image_src = value;}
    }

}
