using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

using System.IO;
using TMPro;

public class FileBrowserRow : MonoBehaviour
{
    private Image _thumb;
    private FileInfo _info;
    public FileInfo info { 
        set { 
            _info = value; 
           // _filename.text = _info.Name;
        } 
    }

    public delegate void file_clicked_action(string path);
    public static event file_clicked_action file_clicked; 

    public void row_clicked() {
        file_clicked(_info.FullName);
    }

    private IEnumerator fetch_thumb() {
        string path = "file://" + _info.FullName;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(path);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        } else
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
            _thumb.sprite = sprite;
            _thumb.preserveAspect = true;
        }
    }
    void Awake () {
        _thumb = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(fetch_thumb());
    }
}
