using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileBrowser : MonoBehaviour
{
    private Split _split;
    public Split split { set { _split = value;}}

    public delegate void FileBrowserCallback(string path);
    private FileBrowserCallback _callback = null;

    [SerializeField] private FileView _file = null;
    [SerializeField] private FileBrowserSettings _settings = null;

    public void set_filters (System.Int32 index) {
        _file.set_filters(_settings.thumb_mappings[index].extensions.ToArray());
    }

    public void show(FileBrowserCallback callback, string[] filters=null) {                
        _callback = callback;
        if (_file == null) {
            _file = new FileView();
        }
        _file.set_filters(filters);
    }

    void OnEnable() {
        FileBrowserRow.file_clicked += selected_image;
    }
    void OnDisable() {
        FileBrowserRow.file_clicked -= selected_image;
    }
    
    public void selected_image(string path) {
        _callback(path);
        PlayerPrefs.SetString("browser_path", path);
        if (_split) {
            _split.thumb_selected(path);
        }
        Destroy (transform.gameObject.GetComponentInParent<Canvas>().gameObject);

        //Destroy();
    }    
}
