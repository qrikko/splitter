using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

using TMPro;

public class Split : MonoBehaviour
{
    [SerializeField] private Image _thumb = null;
    [SerializeField] private TMP_InputField _name = null;
    [SerializeField] private TMP_InputField _split_time = null;
    [SerializeField] private TMP_InputField _best_segment = null;
    [SerializeField] private FileBrowser _imagepicker = null;

    private string _thumb_path;
    
    private speedrun.SplitMeta _model;
    public speedrun.SplitMeta model { 
        get { 
            return _model;
        } set {
            _model = value;
            _name.text = _model.name;
            _split_time.text = _model.pb.ToString();
            _best_segment.text = _model.gold.ToString();
            _thumb_path = _model.thumb_path;
            StartCoroutine (fetch_thumb());
        }
    }

    public void thumb_selected(string path) {
        _model.thumb_path = path;
        StartCoroutine(fetch_thumb());
        
    }

    public void select_thumb() {
        // gotten from file browser        
        //UnityEngine.SceneManagement.SceneManager.LoadScene("FileBrowser", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        
        FileBrowser imagepicker = Instantiate(_imagepicker);
        imagepicker.gameObject.SetActive(true);
        imagepicker.split = this;        
    }

    private IEnumerator fetch_thumb() {
        string path = "file://" + _model.thumb_path;
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

    public void name_changed(string name) {
        _model.name = name;
    }
}
