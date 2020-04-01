using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitSettings : MonoBehaviour
{
    [SerializeField] private GameObject _preferences = null;

    private void Awake()
    {
        _preferences.SetActive(false);
    }
    
    public void select() {
        if (_preferences.activeInHierarchy)
        {
            _preferences.GetComponent<Animator>().SetTrigger("close");
        } else
        {
            _preferences.SetActive(true);
            _preferences.GetComponent<Animator>().SetTrigger("open");
        }
    }
}
