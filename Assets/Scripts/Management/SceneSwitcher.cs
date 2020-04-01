using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public void load_scene(string name) {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }


}

