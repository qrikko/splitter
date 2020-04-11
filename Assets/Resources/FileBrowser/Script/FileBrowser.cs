using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileBrowser : MonoBehaviour
{
    private Split _split;
    public Split split { set { _split = value;}}


    void OnEnable() {
        Debug.Log("PrintOnEnable: script was enabled");
        FileBrowserRow.file_clicked += selected_image;
    }
    void OnDisable() {
        Debug.Log("PrintOnEnable: script was DISABLED");
        FileBrowserRow.file_clicked -= selected_image;
    }
    
    public void selected_image(string path) {
        PlayerPrefs.SetString("browser_path", path);
        _split.thumb_selected(path);
        Destroy (transform.gameObject.GetComponentInParent<Canvas>().gameObject);

        //Destroy();
    }    
}
