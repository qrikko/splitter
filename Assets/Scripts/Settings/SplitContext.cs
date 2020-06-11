using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitContext : MonoBehaviour {
    [SerializeField] private Toggle _pause_split = null;

    private speedrun.SplitMeta _model = null;
    private void OnEnable() {
        Split.on_select += on_select;
    }

    private void on_select(speedrun.SplitMeta model) {
        _model = model;
        _pause_split.isOn = model.pause_state;
    }

    public void toggle(bool true_or_false) {
        _model.pause_state = true_or_false;
    }
}
