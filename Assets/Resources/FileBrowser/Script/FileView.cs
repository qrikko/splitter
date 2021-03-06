﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class FileView : MonoBehaviour
{
    [SerializeField] private GameObject _folder_container = null;
    [SerializeField] private GameObject _file_container = null;
    [SerializeField] private FolderBrowserRow _folder_row_prefab = null;
    [SerializeField] private FileBrowserRow _file_row_prefab = null;

    // make sure this is not the default, it should be provided for the image case..
    private List<string> _extensions = new List<string>(){ ".png", ".jpg", ".jpeg", ".bmp", ".tga", ".gif"};

    private bool _show_hidden = false;
    public bool show_hidden {
        set {
            _show_hidden = value; 
            if (_current_folder != null) {
                list_folder(_current_folder.FullName);
            }
        }
    }

    //private List<string> _extensions = new List<string>();
    public void set_filters (string[] filters) {
        _extensions.Clear();
        if (filters != null) {
            foreach(string s in filters) {
                _extensions.Add(s);
            }
        }
        if (_current_folder != null) {
            list_folder(_current_folder.FullName);
        }
    }
    
    private DirectoryInfo _current_folder;

    public delegate void GoUpDelegate(string path);
    public static GoUpDelegate go_up_action;

    public void go_up()
    {
        if (_current_folder==null || _current_folder.Parent == null) {
            return;
        }
        _current_folder = _current_folder.Parent;
        list_folder(_current_folder.FullName);
        go_up_action(_current_folder.FullName);
    }

    public void list_files(DirectoryInfo di) {
        FileInfo[] files = di.GetFiles();
        
        foreach (FileInfo file in files) {
            if (_show_hidden == false && file.Attributes.HasFlag(FileAttributes.Hidden)) {
                continue;
            }
            if (_extensions.Contains(file.Extension) || _extensions.Count == 0) {
                FileBrowserRow row = GameObject.Instantiate(_file_row_prefab, _file_container.transform);
                row.info = file;
            }
        }
    }

    private void list_folder(string path) {
        if (_file_container == null) return;
        foreach (Transform go in _file_container.transform) {
            Destroy (go.gameObject);
        }

        foreach (Transform go in _folder_container.transform)
        {
            Destroy(go.gameObject);
        }

        //_up_button.info = _current_folder.Parent;
        FolderBrowserRow prev_row = GameObject.Instantiate(_folder_row_prefab, _folder_container.transform);
        prev_row.set_previous(new DirectoryInfo(Path.GetDirectoryName(path)));

        _current_folder = new DirectoryInfo(path);        
        foreach (DirectoryInfo folder in _current_folder.GetDirectories()) {
            if (_show_hidden == false && folder.Attributes.HasFlag(FileAttributes.Hidden)) {
                continue;
            }
            FolderBrowserRow row = GameObject.Instantiate(_folder_row_prefab, _folder_container.transform);
            row.info = folder;
        }
        
        list_files(_current_folder); // @todo: should this really be sent in, or just use the member?
    }

    // Start is called before the first frame update
    void Start() {
        string path = PlayerPrefs.GetString("browser_path") == ""
            ? Application.persistentDataPath
            : Path.GetDirectoryName(PlayerPrefs.GetString("browser_path"));
        //string path = "/home/qrikko/Documents/Unity Projects/Splitter/Assets/Graphics/";
        list_folder(path);
    }

    void OnEnable() {
        FolderBrowserRow.folder_clicked += list_folder;
        BreadCrumbs.ClickAction += list_folder;
        //FileBrowserRow.file_clicked += list_folder;
    }
    void OnDisable() {
        FolderBrowserRow.folder_clicked -= list_folder;
        BreadCrumbs.ClickAction -= list_folder;
        //FileBrowserRow.file_clicked += list_folder;
    }
}
