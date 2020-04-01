using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using TMPro;

public class FolderBrowserRow : AbstractBrowserRow
{
    private DirectoryInfo _info;
    public DirectoryInfo info { 
        set { 
            _info=value; 
            _filename.text = _info.Name;
        }
    }

    public delegate void folder_clicked_action(string path);
    public static event folder_clicked_action folder_clicked; 

    public void row_clicked() {
        folder_clicked(_info.FullName);
    }
}
