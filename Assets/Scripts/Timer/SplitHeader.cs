using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SplitHeader : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake() {
        _text = GetComponent<TMP_Text>();
    }
    private void OnEnable() {
        SplitsManager.change_view_mode += change_view_mode;
    }

    private void OnDisable() {
        SplitsManager.change_view_mode -= change_view_mode;
    }

    private void change_view_mode (SplitsManager.ViewMode mode) {
        switch (mode) {
            case SplitsManager.ViewMode.PB:
                _text.text = "PB";
                break;
            case SplitsManager.ViewMode.GLOD:
                _text.text = "GLOD";
                break;
            case SplitsManager.ViewMode.DURATION:
                _text.text = "PBD";
                break;
            case SplitsManager.ViewMode.POSSIBLE_SAVE:
                _text.text = "SAVE";
                break;
        }

    }
}
