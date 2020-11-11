using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportExportSplits : MonoBehaviour
{
    [SerializeField] private FileBrowser _filepicker = null;
    public void on_click() {
        FileBrowser filepicker = Instantiate(_filepicker);
        filepicker.gameObject.SetActive(true);

        string[] filters = {".lss"};
        filepicker.show((string path) => {
            Debug.Log(path);
        }, filters);
    }
}
