﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("username") && PlayerPrefs.HasKey("userid"))
        {
            SceneManager.LoadScene("Dashboard");
        }
    }
}
