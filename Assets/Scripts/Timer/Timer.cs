using System;
using System.Diagnostics;

using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public enum TimeState {
        Stopped,
        Paused,
        Running,
        Countdown,
        Done
    };
    private TimeState _state = TimeState.Stopped;
    public TimeState state {
        get {return _state;}
    }

    [SerializeField] private TMP_Text _text = null;
    [SerializeField] private TMP_ColorGradient _gradient_settings;

    private string _initial_text;

    private Stopwatch _stopwatch = new Stopwatch();
  //  public Stopwatch stopwatch { get { return _stopwatch; } }

    public long elapsed_ms {
        get { return _stopwatch.ElapsedMilliseconds; }
    }
    public TimeSpan elapsed_ts {
        get { return _stopwatch.Elapsed; }
    }

    private float _offset;
    private Stopwatch _offset_stopwatch = new Stopwatch();

    private VertexGradient _initial_gradient;

    public delegate void run_start_delegate();
    public static run_start_delegate on_run_start;

    public void end_run(long pb) {
        _state = TimeState.Done;
        _stopwatch.Stop();

        VertexGradient gradient;
        if (_stopwatch.ElapsedMilliseconds < pb || pb==0) {
            gradient.topLeft = Color.cyan;
            gradient.topRight = Color.cyan;
            gradient.bottomLeft = Color.blue;
            gradient.bottomRight = Color.blue;
        } else {
            gradient.topLeft = new Color(0.26f, 0.25f, 0.0f);
            gradient.topRight = gradient.topLeft;
            gradient.bottomLeft = new Color(1.0f, 0.44f, 0.43f);
            gradient.bottomRight = gradient.bottomLeft;
        }
        
        _text.colorGradient = gradient;
    }

    public float offset { 
        set {
            _offset = value;
            
            System.TimeSpan ts = _stopwatch.Elapsed;
            ts = ts.Subtract(System.TimeSpan.FromSeconds(-_offset));
            if (ts.Ticks < 0) {
                _initial_text = string.Format("-{0:00}:{1:00}<size='40'>.{2:ff}</size>", 
                    Mathf.Abs(ts.Minutes), 
                    Mathf.Abs(ts.Seconds), 
                    ts
                );
            } else {
                _initial_text = string.Format("{0:00}:{1:00}<size='40'>.{2:ff}</size>", ts.Minutes, ts.Seconds, ts);
            }
            _text.text = _initial_text;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _initial_gradient = new VertexGradient(
            _gradient_settings.topLeft,
            _gradient_settings.topRight,
            _gradient_settings.bottomRight,
            _gradient_settings.bottomLeft
        );
        //_initial_gradient = _text.colorGradient;
        //_is_running = true;
        //_stopwatch.Start();
        //_initial_text = _text.text;
    }

    public void pause() {
        _state = TimeState.Paused;
        _stopwatch.Stop();
    }
    public void resume() {
        _state = TimeState.Running;
        _stopwatch.Start();
    }

    public void toggle_pause()
    {
        if (Input.anyKeyDown) {
            return;
        }
        if (_state == TimeState.Running)
        {
            _state = TimeState.Paused;
            _stopwatch.Stop();
        } else if(_state == TimeState.Paused){
            _state = TimeState.Running;
            _stopwatch.Start();
        }
    }

    public void reset()
    {
        _initial_gradient = new VertexGradient(
            _gradient_settings.topLeft,
            _gradient_settings.topRight,
            _gradient_settings.bottomRight,
            _gradient_settings.bottomLeft
        );
        _text.colorGradient = _initial_gradient;
        _state = TimeState.Stopped;
        _stopwatch.Stop();
        _stopwatch.Reset();
        _text.text = _initial_text;

        _offset_stopwatch.Stop();
        _offset_stopwatch.Reset();        
    }

    public void new_run()
    {
        _offset_stopwatch.Reset();
        _offset_stopwatch.Start();
        _state = TimeState.Countdown;
        //_stopwatch.Start();
    }

    private void format_time (System.TimeSpan ts) {
//        System.TimeSpan ts = _stopwatch.Elapsed;
        //ts = ts.Subtract(System.TimeSpan.FromSeconds(-_offset));
        if (ts.Ticks < 0) {
            _text.text = "-";
        } else {
            _text.text = "";
        }

        if (ts.Hours == 0) {
            _text.text += string.Format("{0:00}:{1:00}<size='40'>.{2:ff}</size>",
                    Mathf.Abs(ts.Minutes),
                    Mathf.Abs(ts.Seconds),
                    ts
            );
        } else {
            _text.text += string.Format("{0:0}:{1:00}:{2:00}<size='40'>.{3:ff}</size>",
                    Mathf.Abs(ts.Hours),
                    Mathf.Abs(ts.Minutes),
                    Mathf.Abs(ts.Seconds),
                    ts
            );
        }
        //_text.text = string.Format("{0:00}:{1:00}<size='40'>.{2:ff}</size>", ts.Minutes, ts.Seconds, ts);
        //_text.text = string.Format(@"{0:mm\:ss\.ff}", ts);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_offset_stopwatch.IsRunning) {
            System.TimeSpan ts = System.TimeSpan.FromSeconds(Mathf.Abs(_offset));
            if(_offset_stopwatch.Elapsed.Ticks >= ts.Ticks) {
                _offset_stopwatch.Stop();
                _offset_stopwatch.Reset();
                _stopwatch.Start();
                _state = TimeState.Running;
                on_run_start();
                return;
            }
            format_time(_offset_stopwatch.Elapsed.Subtract(ts));
        }
        if (_stopwatch.IsRunning) {
            format_time(_stopwatch.Elapsed);
        }
    }
}
