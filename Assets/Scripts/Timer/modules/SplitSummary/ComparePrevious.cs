using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ComparePrevious : MonoBehaviour {
    [SerializeField] TMP_Text _comparison = null;

    void OnEnable() {
        SplitsManager.on_split_compare += last_split_compare;
        SplitsManager.on_reset += reset;
    }

    private void reset(string unused) {
        _comparison.color = Color.white;
        _comparison.text = "-";
    }

    private void last_split_compare(long time, Color c) {
        System.TimeSpan ts = System.TimeSpan.FromMilliseconds(time);
        string ts_string = ts.Minutes == 0 ? @"s\.f" : @"mm\:ss";

        _comparison.color = c;
        _comparison.text = ts.ToString(ts_string);
    }
}
