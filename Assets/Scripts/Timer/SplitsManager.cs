using System;
using UnityEngine;
using UnityEngine.UI;

public class SplitsManager : MonoBehaviour {
    [SerializeField] private SplitRow _split_prefab = null;
    [SerializeField] private Image _pb_compare      = null;
    [SerializeField] private Timer _timer           = null;
    [SerializeField] private Slider _run_total      = null;
    [SerializeField] private Animator _animator     = null;

    private System.TimeSpan _previous_diff = System.TimeSpan.Zero;
    private speedrun.RunAttempt _current_attempt = null;

    private speedrun.RunModel _model;
    private string _id = null;
    private int _split_index = 0;

    private speedrun.Split _previous_split = null;
    private SplitRow _current_split_row;

    public delegate void run_start_delegate(long run_pb, Image thumb);
    public static run_start_delegate on_run_start;

    public delegate void run_end_delegate();
    public static run_end_delegate on_run_end;

    public delegate void update_attempts_delegate(int num, int finished);
    public static update_attempts_delegate on_attempts_update;

    public delegate void reset_delegate(string split_name);
    public static reset_delegate on_reset;

    public delegate void split_delegate(string split, long split_time, long gold_time, long pb_time);
    public static split_delegate on_split;

    public delegate void gold_comparison_delegatea(long time, Color c);
    public static gold_comparison_delegatea on_split_comparea;

    public delegate void gold_comparison_delegate(System.TimeSpan ts, Color c);
    public static gold_comparison_delegate on_split_compare;

    public delegate void update_split_thumb_delegate(Image img);
    public static update_split_thumb_delegate on_update_split_thumb;

    public delegate void final_split_image_delegate(Image img);
    public static final_split_image_delegate final_split_image;

    private void save() {
        string path = PlayerPrefs.GetString(_id);
        if (path == null || path == "") {
            path = Application.persistentDataPath + "/" + _id + "/splits/split.json";
        }
        _model.save(path);
    }

    public void start_run() {
        if (_animator != null) {
            _animator.SetTrigger("start");
        }

        _previous_diff = System.TimeSpan.Zero;
        speedrun.RunAttempt new_attempt = new speedrun.RunAttempt();
        new_attempt.attempt_index = _model.run.game_meta.attempts_count;

        DateTime now = DateTime.Now;
        new_attempt.start_datetime = now.ToString("yyyyMMddTHH:mm.ss");

        GetComponentsInChildren<SplitRow>()[_split_index].split_in();
        _current_split_row = GetComponentsInChildren<SplitRow>()[0];
        _current_attempt = new_attempt;
        _model.run.game_meta.attempts_count++;
        _previous_split = null;
        _timer.new_run();

        //SplitRow last_split = GetComponentsInChildren<SplitRow>()[GetComponentsInChildren<SplitRow>().Length - 1];
        //on_run_start(last_split.model.pb, last_split.thumb);
        on_split(
            _current_split_row.model.name, 
            0, 
            _current_split_row.model.gold, 
            _current_split_row.model.pb
        );
        on_update_split_thumb(_current_split_row.thumb);
    }

    private void reset() {
        SplitRow[] rows = GetComponentsInChildren<SplitRow>();
        for(int i=0; i<rows.Length; i++) {
            SplitRow s = rows[i];
            s.model = _model.run.split_meta[i];
            s.reset();
        }
        _split_index = 0;

        _timer.reset();
        on_reset(rows[0].model.name);
        on_run_end();
    }

    private void restart_run() {
        if (_animator != null) {
            _animator.SetTrigger("stop");
        }
        _current_attempt.end_datetime = DateTime.Now.ToString("yyyyMMddTHH:mm.ss");
        _current_attempt.finished = false;
        _model.run.attempts.Add(_current_attempt);

        _pb_compare.fillAmount = 0;

        on_attempts_update(_model.run.game_meta.attempts_count, _model.run.game_meta.finished_count);

        save();
        reset();
    }

    private void end_run() {
        if (_animator != null) {
            _animator.SetTrigger("stop");
        }

        long pb = _model.run.split_meta[_split_index-1].pb;
        _timer.end_run(pb);

        if (_timer.stopwatch.ElapsedMilliseconds < pb || pb==0) {
            foreach (speedrun.SplitMeta sm in _model.run.split_meta) {
                sm.pb = sm.history[sm.history.Count - 1].split_time;
                sm.pb_index = _current_attempt.attempt_index;
            }
            _model.run.game_meta.pb_attempt_index = _current_attempt.attempt_index;
        }

        _split_index = 0;

        _current_attempt.end_datetime = DateTime.Now.ToString("yyyyMMddTHH:mm.ss");
        _current_attempt.finished = true;
        _model.run.attempts.Add(_current_attempt);

        _model.run.game_meta.finished_count++;
        on_attempts_update(_model.run.game_meta.attempts_count, _model.run.game_meta.finished_count);

        save();
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
                _current_split_row.model.pb
        );
    }

    public void split () {
        speedrun.Split split = create_split(_timer.stopwatch.ElapsedMilliseconds);
        TimeSpan elapsed = _timer.stopwatch.Elapsed; // total milliseconds for precission?
        _current_split_row.time.text = elapsed.ToString(@"m\:ss");

        long pb_time_left = _current_split_row.model.pb - _timer.stopwatch.ElapsedMilliseconds;
        System.TimeSpan ts = System.TimeSpan.FromMilliseconds(pb_time_left);
        _current_split_row.delta_image.color = new Color(0.0f, 0.0f, 0.0f, 0.2f);
        string ts_string = ts.Minutes == 0 ? @"s\.f" : @"mm\:ss";
        _current_split_row.delta.text = (pb_time_left > 0 ? "-" : "+") + ts.ToString(ts_string);

        long split_gold = _model.run.split_meta[_split_index].gold;
        split.split_duration = split.split_time - (_previous_split == null ? 0 : _previous_split.split_time);

        if (split.split_duration < split_gold || split_gold==0) {
            long old_gold = _model.run.split_meta[_split_index].gold;
            _model.run.split_meta[_split_index].gold = split.split_duration;
            _current_split_row.GetComponent<Image>().color = new Color(1.0f, 0.92f, 0.0f, 0.25f);

            System.TimeSpan glod_ts = System.TimeSpan.FromMilliseconds(split.split_duration - old_gold);
            on_split_compare(glod_ts, new Color(1.0f, 0.92f, 0.0f, 0.9f));
        } else {
            //long time_left = _current_split_row.model.pb - _timer.stopwatch.ElapsedMilliseconds;
            //System.TimeSpan ts = System.TimeSpan.FromMilliseconds(time_left);
            //string ts_string = ts.Minutes == 0 ? @"s\.f" : @"m\:ss";

            System.TimeSpan diff_ts = (ts - _previous_diff);

            //on_split_compare(time_left, time_left > 0 ? Color.green:Color.red);
            on_split_compare(diff_ts, diff_ts.Ticks > 0 ? Color.green:Color.red);

        }
        _previous_diff = ts;

        if (++_split_index >= _model.run.split_meta.Count) {
            end_run();
        } else {
            GetComponentsInChildren<SplitRow>()[_split_index].split_in();
        }
        _previous_split = split;
        _current_split_row = GetComponentsInChildren<SplitRow>()[_split_index];

        on_split(
            _current_split_row.model.name, 
            _previous_split.split_time, 
            _current_split_row.model.gold, 
            _current_split_row.model.pb
        );

        on_update_split_thumb(_current_split_row.thumb);
    }

    //@todo: there must be things we don't need to do directly here!
    private void Update() {
        if (_current_split_row != null && _timer.stopwatch.IsRunning) {
            float run_percent = (float)_timer.stopwatch.ElapsedMilliseconds / _model.run.split_meta[_model.run.split_meta.Count-1].pb;

            long split_time = _previous_split==null ? 0 : _previous_split.split_time;
            long gold = _current_split_row.model.gold;            
            long ms = _timer.stopwatch.ElapsedMilliseconds - split_time;

            if (ms < gold) {
                _current_split_row.delta_image.fillAmount = (float)ms/gold;
            } else if (_timer.stopwatch.ElapsedMilliseconds < _current_split_row.model.pb){
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

            _run_total.value = (float)_timer.stopwatch.ElapsedMilliseconds / _model.run.split_meta[_model.run.split_meta.Count-1].pb;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (_timer.stopwatch.IsRunning == false) {
                if (_timer.stopwatch.ElapsedMilliseconds > 0) {
                    reset();
                } else {
                    start_run();
                }
            } else {
                split();
            }
        } else if(Input.GetKeyDown(KeyCode.S)) {
            skip_split();
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            restart_run();
        }
    }

    private void set_final_split(Image img) {
        final_split_image(img);
    }

    void OnDisable() {
        SplitRow i = transform.GetChild(transform.childCount -1).GetComponent<SplitRow>();
        i.thumb_updated -= set_final_split;
    }

    void OnEnable() {
        SplitRow i = transform.GetChild(transform.childCount -1).GetComponent<SplitRow>();
        i.thumb_updated += set_final_split;
    }

    void Awake() {
        _id = PlayerPrefs.GetString("active_game");
        _model = GameView.load_game_model(_id);
        var start_offset = _model.run.game_meta.start_offset;
        if(start_offset != null && start_offset != "") {
            _timer.offset = float.Parse(_model.run.game_meta.start_offset);
        } else {
            _timer.offset = 0.0f;
        }

        foreach (speedrun.SplitMeta s in _model.run.split_meta) {
            SplitRow split = SplitRow.Instantiate(_split_prefab, transform);
            split.model = s;
        }

        if (_model.run.game_meta.finished_count == 0) {
            _model.run.game_meta.finished_count = 0;
            foreach(var a in _model.run.attempts) {
                if (a.finished == true) {
                    _model.run.game_meta.finished_count++;
                }
            }
            save();
        }
        on_attempts_update(_model.run.game_meta.attempts_count, _model.run.game_meta.finished_count);
    }
}
