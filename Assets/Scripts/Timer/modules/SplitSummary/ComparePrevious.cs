using System;
using TMPro;
using UnityEngine;

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

    private void last_split_compare(TimeSpan ts, Color c) {
        string ts_string = @"HH\:mm\:ss";
        if (ts.Hours == 0) {
            if (ts.Minutes == 0) {
                ts_string = @"s\.f";
            } else {
                ts_string = @"mm\:ss";
            }
        }
        _comparison.text = ts.ToString(ts_string);
        _comparison.color = c;
    }

    private void alast_split_compare(long time, Color c) {
        System.TimeSpan ts = System.TimeSpan.FromMilliseconds(time);
        string ts_string = @"HH\:mm\:ss";
        if (ts.Hours == 0) {
            if (ts.Minutes == 0) {
                ts_string = @"s\.f";
            } else {
                ts_string = @"mm\:ss";
            }
        }

        _comparison.color = c;
        _comparison.text = ts.ToString(ts_string);
    }
}
