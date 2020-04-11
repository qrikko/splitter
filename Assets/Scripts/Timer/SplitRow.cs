using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SplitRow : MonoBehaviour
{
    [SerializeField] private Sprite _background = null;
    [SerializeField] private Image _thumb = null;
    [SerializeField] private Image _delta_image = null;
    [SerializeField] private TMP_Text _name = null;
    [SerializeField] private TMP_Text _delta = null;
    [SerializeField] private TMP_Text _time = null;
    [SerializeField] private Image _bg_fill = null;

    private Color _empty_color;
    [SerializeField] private Color _filled_color = Color.white;

    public void progress(float fill) {
        _bg_fill.fillAmount = fill;
        _bg_fill.color = Color.Lerp(_empty_color, _filled_color, fill);
    }
    public Image thumb { get { return _thumb; } }
    public Image delta_image {get {return _delta_image;}}  
    public TMP_Text delta { get { return _delta; } }

    private Color _initial_color;
    public Color initial_color { get { return _initial_color; } }

    public void reset() {
        _bg_fill.color = _empty_color;
        _bg_fill.CrossFadeAlpha(1.0f, 0.0f, true);
        _bg_fill.fillAmount = 0;
    }

    public void split_in() {
        _bg_fill.color = _empty_color;
        _bg_fill.CrossFadeAlpha(1.0f, 0.0f, true);
        GetComponent<Image>().sprite = _background;
        GetComponent<Image>().color = new Color(0.0f, 0.4f, 1.0f, 0.6f);
    }
    public void split_out()
    {
        GetComponent<Image>().sprite = null;
        GetComponent<Image>().color = _initial_color;

        _bg_fill.CrossFadeAlpha(0, 0.4f, true);
    }


    public TMP_Text time { get { return _time; } }

    private speedrun.SplitMeta _model;
    public speedrun.SplitMeta model { 
        get { return _model;}
        set {
            _model = value;
            _name.text = _model.name;
            _delta_image.fillAmount = 0;
            _delta.text = "-";
            _delta.color = Color.white;
            
            if (_model.history.Count == 0) {
                _time.text = "-";                
            } else {
                //_time.text = System.TimeSpan.FromMilliseconds(_model.history[_model.pb_index].split_time).ToString(@"m\:ss");
                _time.text = System.TimeSpan.FromMilliseconds(_model.pb).ToString(@"m\:ss");
            }
            GetComponent<Image>().color = _initial_color;
            if (_model.thumb_path != null) {
                StartCoroutine(fetch_thumb());
            }
        }
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

    private void Awake()
    {
        _initial_color = GetComponent<Image>().color;
        _empty_color = _bg_fill.color;
    }
}
