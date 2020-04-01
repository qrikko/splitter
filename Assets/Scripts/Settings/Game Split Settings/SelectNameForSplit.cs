using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectNameForSplit : MonoBehaviour
{
    [SerializeField] TMP_InputField _filename;
    public void done() {
        string id = PlayerPrefs.GetString("active_game");
        string path = Application.persistentDataPath + "/" + id + "/splits/" + _filename.text + ".json";
        PlayerPrefs.SetString(id, _filename.text);
        SceneManager.UnloadSceneAsync("Enter splits name");
    }
}
