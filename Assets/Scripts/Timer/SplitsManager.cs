using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// idea is that this one kind of keeps track of the split rows, and uses delagates to just notify what is going on.
public enum RunEvents {
    start,
    reset,
    end
};
public class SplitsManager : MonoBehaviour {
    [SerializeField] private SplitRow _split_prefab = null;
    [SerializeField] private Image _pb_compare;
    [SerializeField] private Timer _timer = null;
    [SerializeField] private Slider _run_total = null;
    [SerializeField] private Animator _animator;

    private speedrun.RunAttempt _current_attempt = null;

    private speedrun.RunModel _model;
    private string _id = null;
    private int _split_index = 0;

    private speedrun.Split _previous_split = null;
    private SplitRow _current_split_row;

    

    public delegate void run_event_delegate(RunEvents run_event);
    public static run_event_delegate run_event;

    public delegate void run_start_delegate(long run_pb, Image thumb);
    public static run_start_delegate on_run_start;
    public delegate void run_end_delegate();
    public static run_end_delegate on_run_end;

    public delegate void update_attempts_delegate(int num, int finished);
    public static update_attempts_delegate on_attempts_update;

    public delegate void reset_delegate(string split_name);
    public static reset_delegate on_reset;

    public delegate void split_delegate(string split, long split_time, long gold_time, long pb_time, Image thumb);
    public static split_delegate on_split;
    
    public void start_run()
    {
        if (_animator != null) {
            _animator.SetTrigger("start");
        }
        //run_event(RunEvents.start);
        speedrun.RunAttempt new_attempt = new speedrun.RunAttempt();
        new_attempt.attempt_index = _model.run.game_meta.attempts_count;

        int attempt_count = 0;
        foreach(var a in _model.run.attempts) {
            if (a.finished == true) {
                attempt_count ++;
            }
        }

        on_attempts_update(new_attempt.attempt_index, attempt_count);

        DateTime now = DateTime.Now;
        new_attempt.start_datetime = now.ToString("yyyyMMddTHH:mm.ss");

        GetComponentsInChildren<SplitRow>()[_split_index].split_in();
//        GetComponentsInChildren<SplitRow>()[_split_index].GetComponent<Image>().color = Color.blue;
        _current_split_row = GetComponentsInChildren<SplitRow>()[0];
        _current_attempt = new_attempt;
        _model.run.game_meta.attempts_count++;
        _previous_split = null;
        _timer.new_run();

        SplitRow last_split = GetComponentsInChildren<SplitRow>()[GetComponentsInChildren<SplitRow>().Length - 1];
        on_run_start(last_split.model.pb, last_split.thumb);
        on_split(
            _current_split_row.model.name, 
            0, 
            _current_split_row.model.gold, 
            _current_split_row.model.pb,
            _current_split_row.thumb
        );
    }

    private void reset()
    {
        //run_event(RunEvents.reset);
        SplitRow[] rows = GetComponentsInChildren<SplitRow>();
        for(int i=0; i<rows.Length; i++)
        {
            SplitRow s = rows[i];
            s.model = _model.run.split_meta[i];
            s.reset();
        }
        _split_index = 0;
        
        _timer.reset();
        on_reset(rows[0].model.name);
    }

    private void restart_run()
    {
        if (_animator != null) {
            _animator.SetTrigger("stop");
        }
        _current_attempt.end_datetime = DateTime.Now.ToString("yyyyMMddTHH:mm.ss");
        _current_attempt.finished = false;
        _model.run.attempts.Add(_current_attempt);

        _pb_compare.fillAmount = 0;

        // write the attempt to file, update golds and pb times
        // also.. perhaps write to file more often, but for now this is where I'll do it.
        string path = PlayerPrefs.GetString(_id);
        if (path == null || path == "") {
            path = Application.persistentDataPath + "/" + _id + "/splits/split.json";
        }
        
        _model.save(path);

        reset();
    }

    private void end_run() {
        if (_animator != null) {
            _animator.SetTrigger("stop");
        }
        //run_event(RunEvents.end);
        long pb = _model.run.split_meta[_split_index-1].pb;
        _timer.end_run(pb);

        if (_timer.stopwatch.ElapsedMilliseconds < pb || pb==0)
        {
            
            foreach (speedrun.SplitMeta sm in _model.run.split_meta)
            {
                sm.pb = sm.history[sm.history.Count - 1].split_time;
                sm.pb_index = _current_attempt.attempt_index;
            }
            _model.run.game_meta.pb_attempt_index = _current_attempt.attempt_index;
        }
        
        // this shouldn't be done here, should be done in RunSummary, use delegate!
       // _pb_compare.fillAmount = 0;

        _split_index = 0;

        _current_attempt.end_datetime = DateTime.Now.ToString("yyyyMMddTHH:mm.ss");
        _current_attempt.finished = true;
        _model.run.attempts.Add(_current_attempt);


        // write the attempt to file, update golds and pb times
        // also.. perhaps write to file more often, but for now this is where I'll do it.
        string path = PlayerPrefs.GetString(_id);
        if (path == null || path == "") {
            path = Application.persistentDataPath + "/" + _id + "/splits/split.json";
        }
        _model.save(path);
        on_run_end();
    }

    private speedrun.Split create_split(long split_time) {
        speedrun.Split split = new speedrun.Split();
        split.attempt_index = _current_attempt.attempt_index;
        split.split_time = split_time;
        _model.run.split_meta[_split_index].history.Add(split);

        _current_split_row.split_out();
        return split;
    }
    public void skip_split() {
        speedrun.Split split = create_split(-1);
        _current_split_row.delta.text = "-";
        split.split_duration = 0; // or -1 or something?
        GetComponentsInChildren<SplitRow>()[_split_index].split_in(); // might need to tell it it can't gold!

        _previous_split = split;
        _current_split_row = GetComponentsInChildren<SplitRow>()[_split_index];

        on_split(
                _current_split_row.model.name,
                _previous_split.split_time,
                _current_split_row.model.gold,
                _current_split_row.model.pb,
                _current_split_row.thumb
        );
    }

    public void split ()
    {
        /*
        speedrun.Split split = new speedrun.Split();
        split.attempt_index = _current_attempt.attempt_index;
        split.split_time = _timer.stopwatch.ElapsedMilliseconds;
        //split.split_duration = split.split_time - _last_split.split_time;
        
        _model.run.split_meta[_split_index].history.Add(split);
        //_last_split = split;
        _current_split_row.split_out();*/

        speedrun.Split split = create_split(_timer.stopwatch.ElapsedMilliseconds);
        TimeSpan elapsed = _timer.stopwatch.Elapsed; // total milliseconds for precission?
        _current_split_row.time.text = elapsed.ToString(@"m\:ss");

        long pb_time_left = _current_split_row.model.pb - _timer.stopwatch.ElapsedMilliseconds;
        System.TimeSpan ts = System.TimeSpan.FromMilliseconds(pb_time_left);
        _current_split_row.delta_image.color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        string ts_string = ts.Minutes == 0 ? @"s\.f" : @"mm\:ss";
        _current_split_row.delta.text = (pb_time_left > 0 ? "-" : "+") + ts.ToString(ts_string);

        long split_gold = _model.run.split_meta[_split_index].gold;
        long split_duration = split.split_time;
       // if (_split_index > 0)
       // {
            split.split_duration = split.split_time - (_previous_split == null ? 0 : _previous_split.split_time);
       // }
        
        if (split.split_duration < split_gold || split_gold==0)
        {
            _model.run.split_meta[_split_index].gold = split.split_duration;
            _current_split_row.GetComponent<Image>().color = new Color(1.0f, 0.92f, 0.0f, 0.25f);
        }

        if (++_split_index >= _model.run.split_meta.Count)
        {
            end_run();
        } else
        {
            GetComponentsInChildren<SplitRow>()[_split_index].split_in();
            //GetComponentsInChildren<SplitRow>()[_split_index].GetComponent<Image>().color = Color.blue;
        }
        _previous_split = split;
        _current_split_row = GetComponentsInChildren<SplitRow>()[_split_index];

        on_split(
            _current_split_row.model.name, 
            _previous_split.split_time, 
            _current_split_row.model.gold, 
            _current_split_row.model.pb,
            _current_split_row.thumb
        );
    }

    private void Update()
    {
        if (_current_split_row != null && _timer.stopwatch.IsRunning) {
            long split_time = _previous_split==null ? 0 : _previous_split.split_time;
            long gold = _current_split_row.model.gold;            
            long ms = _timer.stopwatch.ElapsedMilliseconds - split_time;

            //long pb_time_left = _pb_time - _timer.stopwatch.ElapsedMilliseconds;

            if (ms < gold) {
                _current_split_row.delta_image.fillAmount = (float)ms/gold;
            } else if (_timer.stopwatch.ElapsedMilliseconds < _current_split_row.model.pb){
                //_current_split_row.delta_image.fillAmount = 1;
                _current_split_row.delta_image.color = Color.green;
            } else {
                _current_split_row.delta_image.color = Color.red;
            }
            
            if (_timer.stopwatch.ElapsedMilliseconds < _current_split_row.model.pb) {
                _pb_compare.color = Color.green;
                float fill = (float) ms/(_current_split_row.model.pb-split_time);
                _pb_compare.fillAmount = fill;
                _current_split_row.progress(fill);
            } else {
                _pb_compare.color = Color.red;
            }

            long pb_time_left = _current_split_row.model.pb - _timer.stopwatch.ElapsedMilliseconds;
            System.TimeSpan ts = System.TimeSpan.FromMilliseconds(pb_time_left);
            string ts_string = ts.Minutes == 0 ? @"s\.f" : @"m\:ss";
            if (pb_time_left > 0) {
                _current_split_row.delta.color = Color.green;
                _current_split_row.delta.text = "-" + ts.ToString(ts_string);
            } else {
                _current_split_row.delta.color = Color.red;
                _current_split_row.delta.text = "+" + ts.ToString(ts_string);
            }
            // slider, should be moved to it's own script for split summary!
            _run_total.value = (float)_timer.stopwatch.ElapsedMilliseconds / _model.run.split_meta[_model.run.split_meta.Count-1].pb;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_timer.stopwatch.IsRunning == false)
            {
                if (_timer.stopwatch.ElapsedMilliseconds > 0)
                {
                    reset();
                } else
                {
                    start_run();
                }
            } else
            { 
                split();
            }
        } else if(Input.GetKeyDown(KeyCode.S)) {
            skip_split();
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            restart_run();
        }
    }

    void Start()
    {
        //run_event(RunEvents.reset);
        _id = PlayerPrefs.GetString("active_game");
        _model = GameView.load_game_model(_id);
        var start_offset = _model.run.game_meta.start_offset;
        if(start_offset != null && start_offset != "") {
            _timer.offset = float.Parse(_model.run.game_meta.start_offset);
        } else {
            _timer.offset = 0.0f;
        }
        //_timer.reset();

        foreach (speedrun.SplitMeta s in _model.run.split_meta)
        {
            Debug.Log(s.name);
            SplitRow split = SplitRow.Instantiate(_split_prefab, transform);
            split.model = s;

        }
    }
}
