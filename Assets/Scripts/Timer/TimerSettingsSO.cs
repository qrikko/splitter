using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public struct SplitViewBackground {
    
    public Color color;    
    private string _image_src;
    public string image_src { get { return _image_src;} set {_image_src = value;}}
}

[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Timer Settings", order = 2)]
public class TimerSettingsSO : ScriptableObject
{
    public delegate void BGColorChangeDelegate();
    public static BGColorChangeDelegate on_bg_color_change;
    public delegate void BGImageChangeDelegate();
    public static BGImageChangeDelegate on_bg_image_change;

    public SplitViewBackground _split_view_bg;

    public Color background_color {
        get { return _split_view_bg.color;}
        set {_split_view_bg.color = value; on_bg_color_change();}
    }

    public Sprite background_image {
        get {
            string path = _split_view_bg.image_src;
            if (File.Exists(path)) {
                byte[] fileData = File.ReadAllBytes(path);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

                return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
            }
            return null;
        }
    }

    public string background_image_src {
        get { return _split_view_bg.image_src; }
        set { _split_view_bg.image_src = value; on_bg_image_change();}
    }

    
}
