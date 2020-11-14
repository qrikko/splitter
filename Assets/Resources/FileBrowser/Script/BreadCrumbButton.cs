using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class BreadCrumbButton : MonoBehaviour
{
    private TMP_Text _text;
    public string text {get {return _text.text;} set {_text.text = value;}}
    
    
    public delegate void ClickDelegate(string path);
    public static event ClickDelegate click_action;

    public void on_click() {
        click_action(_text.text);
    }

    void Awake () {
        _text = GetComponentInChildren<TMP_Text>();
    }
}
