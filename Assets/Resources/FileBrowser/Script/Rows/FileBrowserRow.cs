using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] private FileBrowserSettings _settings;

    public void row_clicked() {
        file_clicked(_info.FullName);
    }

    
    void Awake () {
        _thumb = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start() {
        _thumb.sprite = _settings.get_thumb_for_type(_info);
        _thumb.preserveAspect = true;
    }
}
