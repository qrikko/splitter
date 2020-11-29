using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    [SerializeField] Image _target;
    [SerializeField] GameObject[] _panels;

    private int _current_id = 0;
    public int current_id {
        set {
            _panels[_current_id].SetActive(false);
            _current_id = value;
            _panels[_current_id].SetActive(true);

            
            //_target.color = Color.White;
        }
    }

}
