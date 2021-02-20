using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectNameForSplit : MonoBehaviour
{
    [SerializeField] TMP_InputField _filename = null;
    public void done() {
        string id = PlayerPrefs.GetString("active_game");
        string path = Application.persistentDataPath + "/game_cache/" + id + "/splits/" + _filename.text + "/splits.json";
        PlayerPrefs.SetString(id, _filename.text);
        SceneManager.UnloadSceneAsync("Enter splits name");
    }
}
