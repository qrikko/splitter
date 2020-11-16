using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SplitRow : MonoBehaviour {


    public delegate void thumb_updated_delegate(Image thumb);
    public thumb_updated_delegate thumb_updated;

    [SerializeField] private Sprite _background = null;
    [SerializeField] private Image _thumb = null;
    [SerializeField] private Image _delta_image = null;
    [SerializeField] private TMP_Text _name = null;
    [SerializeField] private TMP_Text _delta = null;
    [SerializeField] private TMP_Text _time = null;
    [SerializeField] private Image _bg_fill = null;

    private Sprite _initial_background;

    private bool _limerick_mode = false;
    public bool limerick_mode {
        set {
            _limerick_mode = value;
            if (value == true) {

                _bg_fill.fillAmount = 1;
                _bg_fill.fillOrigin = 1;
                _name.rectTransform.SetSiblingIndex(Mathf.Max(_bg_fill.transform.GetSiblingIndex()-1, 0));

            } else {
                _bg_fill.fillAmount = 0;
                _bg_fill.fillOrigin = 0;
                _name.rectTransform.SetSiblingIndex(_bg_fill.transform.GetSiblingIndex()+1);
            }
        }
    }

    private Color _empty_color;
    [SerializeField] private Color _filled_color = Color.white;

    private void OnEnable() {
        SplitsManager.change_view_mode += change_view_mode;

        if(_model != null && _model.pause_state) {
            _initial_color = Color.red;
            _empty_color = Color.red;
            _bg_fill.color = Color.red;
            GetComponent<Image>().color = Color.red;
        }
    }
    private void OnDisable() {
        SplitsManager.change_view_mode -= change_view_mode;
    }

    private void change_view_mode(SplitsManager.ViewMode mode) {
        System.TimeSpan t;
        switch (mode) {
            case SplitsManager.ViewMode.PB:
                _time.text = System.TimeSpan.FromMilliseconds(_model.pb).ToString(@"m\:ss");
            break;
            case SplitsManager.ViewMode.GLOD:
                //_time.text = System.TimeSpan.FromMilliseconds(_model.gold).ToString(@"m\:ss\.ff");
                t = System.TimeSpan.FromMilliseconds(_model.gold);
                _time.text = string.Format("{0}:{1}.<size=16>{2}", t.Minutes, t.Seconds, t.Milliseconds);
                break;
            case SplitsManager.ViewMode.DURATION:
            case SplitsManager.ViewMode.POSSIBLE_SAVE:
                speedrun.Split i = null;
                foreach(var pb in _model.history) {
                    if (pb.attempt_index == _model.pb_index) {
                        i = pb;
                        break;
                    }
                }

                if(mode == SplitsManager.ViewMode.DURATION) {
                    t = System.TimeSpan.FromMilliseconds(i.split_duration);
                    _time.text = string.Format("{0}:{1}.<size=16>{2}", t.Minutes, t.Seconds, t.Milliseconds);
                    //_time.text = System.TimeSpan.FromMilliseconds(i.split_duration).ToString(@"m\:ss");
                } else if(mode == SplitsManager.ViewMode.POSSIBLE_SAVE) {
                    t = System.TimeSpan.FromMilliseconds(i.split_duration - _model.gold);
                    _time.text = string.Format("{0}:{1}.<size=16>{2}", t.Minutes, t.Seconds, t.Milliseconds);
                    //_time.text = System.TimeSpan.FromMilliseconds(i.split_duration - _model.gold).ToString(@"m\:ss");
                }
                break;
        }
    }

    public void progress(float fill) {
        _bg_fill.fillAmount = _limerick_mode ? 1-fill : fill;
        _bg_fill.color = Color.Lerp(_empty_color, _filled_color, fill);
    }
    public Image thumb { get { return _thumb; } }
    public Image delta_image {get {return _delta_image;}}  
    public TMP_Text delta { get { return _delta; } }

    private Color _initial_color;
    public Color initial_color { get { return _initial_color; } }

    private bool _skipped;
    public bool skipped {get {return _skipped;}}
    public void reset() {
        _bg_fill.color = _empty_color;
        _bg_fill.CrossFadeAlpha(1.0f, 0.0f, true);
        _bg_fill.fillAmount = 0;
        GetComponent<Image>().sprite = _initial_background;
    }

    public void split_in(bool skipped = false) {
        _skipped = skipped;

        if (_model.pause_state) {
            return;
        }
        _bg_fill.color = _empty_color;
        _bg_fill.CrossFadeAlpha(1.0f, 0.0f, true);
        GetComponent<Image>().sprite = _background;
        GetComponent<Image>().color = new Color(0.0f, 0.4f, 1.0f, 0.6f);
    }
    public void split_out(bool skipped = false) {
        _skipped = skipped;
        if (_model.pause_state) {
            return;
        }
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

            // @todo: I guess the way we store these, we would prefer to store the array index
            // @todo: so we don't have to search through it like this to find the pb
            if(_model.pause_state) {
                _bg_fill.color = new Color(1,0,0,1);
            } else {
                foreach (var pb in _model.history) {
                    if (pb.attempt_index == _model.pb_index) {
                        if (_model.gold >= pb.split_duration) {
                            GetComponent<Image>().color = new Color(1, 1, 0, 0.2156863f);
                            GetComponent<Image>().fillCenter = true;
                        }
                        break;
                    }
                }
            }

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

            if(thumb_updated != null) {
                thumb_updated(_thumb);
            }
        }
    }

    private void Awake()
    {
        _initial_color = GetComponent<Image>().color;
        _empty_color = _bg_fill.color;
        _initial_background = GetComponent<Image>().sprite;
    }
}
