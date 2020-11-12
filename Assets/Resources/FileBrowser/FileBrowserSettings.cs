using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public struct ImageMap {
    public List<string> extensions;
    public Sprite image;
}


[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Filebrowser Settings", order = 1)]
public class FileBrowserSettings : ScriptableObject
{
    public Sprite _default;
    public List<ImageMap> _image_mappings;
    
    private string[] _image_extensions = {".png", ".jpg", ".jpeg", ".bmp", ".tga", ".gif"};

    public Sprite get_thumb_for_type(string type) {
        if (System.Array.IndexOf(_image_extensions, type) >= 0) {
            Debug.Log("Image File, need to load and return the specific thumb!");
        }
        return _default;
    }

    public void Awake() {
        //_image_mappings.Add(new ImageMap{} );
    }

    private IEnumerator fetch_thumb(string file_path) {
        // this is useful for Images, not so much for other files, need a way to load apropriate
        // graphics for types other than images that can themself be used as thumbs..
        string path = "file://" + file_path;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(path);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        } else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
//            _thumb.sprite = sprite;
//            _thumb.preserveAspect = true;
        }
    }

}
