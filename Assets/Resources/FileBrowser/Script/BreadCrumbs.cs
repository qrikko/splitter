using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

using System.IO;

public class BreadCrumbs : MonoBehaviour
{
    [SerializeField] private BreadCrumbButton _button_prefab;
    private string _current_path;

    public delegate void ClickBreadcrumbsDelegate(string path);
    public static ClickBreadcrumbsDelegate ClickAction;

    void OnEnable() {
        FolderBrowserRow.folder_clicked += update_breadcrumbs;
        BreadCrumbButton.click_action += click_breadcrumbs;
        FileView.go_up_action += update_breadcrumbs;
    }
    void OnDisable() {
        FolderBrowserRow.folder_clicked -= update_breadcrumbs;
        BreadCrumbButton.click_action -= click_breadcrumbs;
        FileView.go_up_action -= update_breadcrumbs;
    }

    public void click_breadcrumbs(string directory) {
        if(directory == Path.DirectorySeparatorChar.ToString()) { // root path
            update_breadcrumbs("");
            ClickAction("/");
            return;
        }
        string new_path = "";
        foreach(string d in _current_path.Split(Path.DirectorySeparatorChar)) {            
            new_path += d;
            if (d == directory) {
                break;
            }
            new_path += Path.DirectorySeparatorChar;
        }
        update_breadcrumbs(new_path);
        ClickAction(new_path);
    }

    public void update_breadcrumbs(string path) {
        path = (path=="/") ? "" : path;
        _current_path = path;
        
        foreach(Transform t in transform) {
            Destroy (t.gameObject);
        }

        string[] directories = path.Split(Path.DirectorySeparatorChar);
        foreach(string d in directories) {
            BreadCrumbButton b = BreadCrumbButton.Instantiate(_button_prefab, transform);
            b.text = d;
            if (d.Length == 0) {
                b.text = "/";
            }
        }
    }

    void Start() {
        string path = PlayerPrefs.GetString("browser_path") == ""
            ? Application.persistentDataPath
            : Path.GetDirectoryName(PlayerPrefs.GetString("browser_path"));
        update_breadcrumbs(path);
    }
 
}
