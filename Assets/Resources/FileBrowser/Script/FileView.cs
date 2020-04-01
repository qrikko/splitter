using System.Collections;
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

    private DirectoryInfo _current_folder;

    public void go_up()
    {
        if (_current_folder==null || _current_folder.Parent == null) {
            return;
        }
        _current_folder = _current_folder.Parent;
        list_folder(_current_folder.FullName);
    }

    public void list_files(DirectoryInfo di) {
        FileInfo[] files = di.GetFiles();
        List<string> extensions = new List<string>(){ ".png", ".jpg", ".jpeg", ".bmp", ".tga", ".gif" };

        foreach (FileInfo file in files) {
            if (extensions.Contains(file.Extension)) {
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
        _current_folder = new DirectoryInfo(path);

        foreach (DirectoryInfo folder in _current_folder.GetDirectories()) {
            FolderBrowserRow row = GameObject.Instantiate(_folder_row_prefab, _folder_container.transform);
            row.info = folder;
        }
        
        list_files(_current_folder); // @todo: should this really be sent in, or just use the member?
    }

    // Start is called before the first frame update
    void Start() {
        string path = Application.persistentDataPath;
        //string path = "/home/qrikko/Documents/Unity Projects/Splitter/Assets/Graphics/";
        list_folder(path);
    }

    void OnEnable() {
        FolderBrowserRow.folder_clicked += list_folder;
        //FileBrowserRow.file_clicked += list_folder;
    }
}
