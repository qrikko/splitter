using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class BreadCrumbs : MonoBehaviour
{
    [SerializeField] private BreadCrumbButton _button_prefab;
    private string _current_path;

    void OnEnable() {
        FolderBrowserRow.folder_clicked += update_breadcrumbs;
        BreadCrumbButton.click_action += click_breadcrumbs;
    }
    void OnDisable() {
        FolderBrowserRow.folder_clicked -= update_breadcrumbs;
        BreadCrumbButton.click_action -= click_breadcrumbs;
    }

    public void click_breadcrumbs(string directory) {
        string new_path = "";
        foreach(string d in _current_path.Split(System.IO.Path.DirectorySeparatorChar)) {
            new_path += d;
            if (d == directory) {
                break;
            }
            new_path += System.IO.Path.DirectorySeparatorChar;
        }
        update_breadcrumbs(new_path);
    }

    public void update_breadcrumbs(string path) {
        _current_path = path;
        
        foreach(Transform t in transform) {
            Destroy (t.gameObject);
        }

        string[] directories = path.Split(System.IO.Path.DirectorySeparatorChar);
        foreach(string d in directories) {
            BreadCrumbButton b = BreadCrumbButton.Instantiate(_button_prefab, transform);
            b.text = d;
            if (d.Length == 0) {
                b.text = System.IO.Path.DirectorySeparatorChar.ToString();
            }
        }
    }
 
}
