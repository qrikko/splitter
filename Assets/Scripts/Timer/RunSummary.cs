using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

public class RunSummary : MonoBehaviour {
    [SerializeField] private Image _pb_comparison = null;
    [SerializeField] private Image _glod_comparison = null;
    [SerializeField] private Slider _run_progress = null;
    [SerializeField] private Image _final_split = null;
    [SerializeField] private TMP_Text _split_name = null;
    [SerializeField] private TMP_Text _total_time_left = null;
    [SerializeField] private TMP_Text _split_delta = null;

    [SerializeField] private Timer _timer = null;

    private long _last_split_time = 0;
    private long _pb_time = 0;
    private long _gold_time = 0;
    private long _run_pb = 0;

    private void OnEnable()
    {
        SplitsManager.on_run_start += run_start;
        SplitsManager.on_run_end += run_end;
        SplitsManager.on_reset += reset;
        SplitsManager.on_split += split;
    }

    private void run_start (long run_pb, Image last_split_thumb)
    {
        _run_pb = run_pb;
        _final_split.sprite = last_split_thumb.sprite;
        _final_split.preserveAspect = true;
    }

    private void split(string name, long split_time, long gold_time, long pb_time)
    {
        _split_name.text = name;
        _last_split_time = split_time;
        _gold_time = gold_time;
        _pb_time = pb_time;
    }

    private void run_end() { }

    private void reset(string split_name)
    {
        _pb_comparison.fillAmount = 0;
        _run_progress.value = 0;
        _total_time_left.text = "-";
        _total_time_left.color = Color.white;
        _split_delta.text = "-";
        _split_delta.color = Color.white;
        _split_name.text = split_name;
        _glod_comparison.fillAmount = 1;
    }

    private void Awake()
    {
        _split_name.text = "";
    }

    private void Update() {
        if (_timer.stopwatch.IsRunning)
        {
            long ms = _timer.stopwatch.ElapsedMilliseconds - _last_split_time;
            _glod_comparison.fillAmount = 1.0f - (float)ms / _gold_time;

            long pb_time_left = _pb_time - _timer.stopwatch.ElapsedMilliseconds;
            System.TimeSpan ts = System.TimeSpan.FromMilliseconds(pb_time_left);
            string ts_string = ts.Minutes == 0 ? @"s\.f" : @"mm\:ss";

            if (pb_time_left > 0) {
                _split_delta.color = Color.green;
                _split_delta.text = "-" + ts.ToString(ts_string);
            } else {
                _split_delta.color = Color.red;
                _split_delta.text = "+" + ts.ToString(ts_string);
            }

            // update total time
            long run_time_left = _run_pb - _timer.stopwatch.ElapsedMilliseconds;
            System.TimeSpan time = System.TimeSpan.FromMilliseconds(run_time_left);
            if (run_time_left > 0)
            {
                _total_time_left.color = Color.green;
                _total_time_left.text = time.ToString(@"m\:ss");
            } else
            {
                _total_time_left.color = Color.red;
                _total_time_left.text = "+ " + time.ToString(@"m\:ss");
            }
            
        }
    }
}
