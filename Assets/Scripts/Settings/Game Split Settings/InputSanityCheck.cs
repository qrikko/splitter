using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System.Text.RegularExpressions;

public class InputSanityCheck : MonoBehaviour
{
    [SerializeField] private string _rule;
    [SerializeField] private string _format;

    private TMP_InputField _input;
    public void verify() {
        if(Regex.Match(_input.text, _rule).Success) {
            _input.image.color = Color.green;
            string[] tmp = _input.text.Split('.');
            
            _input.text = string.Format(_format, float.Parse(_input.text));

        } else {
            _input.image.color = Color.red;
        }
    }

    void Start () {
        _input = GetComponent<TMP_InputField>();
    }
}
