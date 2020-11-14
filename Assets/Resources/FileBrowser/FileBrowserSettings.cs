using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public struct ThumbMap {
    public string name;
    public List<string> extensions;
    public Sprite image;
}


[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Filebrowser Settings", order = 1)]
public class FileBrowserSettings : ScriptableObject
{
    public Sprite _default;
    [SerializeField] private List<ThumbMap> _thumb_mappings;
    public List<ThumbMap> thumb_mappings {get {return _thumb_mappings;}}
    
    //private string[] _image_extensions = {".png", ".jpg", ".jpeg", ".bmp", ".tga", ".gif"};

    public Sprite get_thumb_for_type(FileInfo info) {
        //if (System.Array.IndexOf(_image_extensions, info.Extension) >= 0) {
        //    return load_sprite_from_file(info.FullName);
        //}
        foreach(ThumbMap image_map in _thumb_mappings) {
            if(image_map.extensions.IndexOf(info.Extension) >= 0) {
                if (image_map.image != null) {
                    return image_map.image;
                } else {
                    return load_sprite_from_file(info.FullName);
                }
            }
        }
        return _default;
    }

    public void Awake() {
        //_image_mappings.Add(new ImageMap{} );
    }

    private static Sprite load_sprite_from_file(string filePath) {
        Sprite sprite = null;
        
        if (File.Exists(filePath)) {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.

            sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));            
        }
        return sprite;
    }

}
