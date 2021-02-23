using System;
using TMPro;
using UnityEngine;

public class SumOfBest : MonoBehaviour
{
    [SerializeField] private TMP_Text _sob      = null;
    [SerializeField] private TMP_Text _attempts = null;

    private splitter.RunModel _model;
    void OnEnable () {
        //@todo: set up delegates to update when gold is updated
        //SplitsManager
        //SplitsManager.on_reset += update_gold;
        SplitsManager.on_run_end += update_gold;
        SplitsManager.on_attempts_update += update_attempts;
    }

    private void update_attempts(int num, int finished) {
        _attempts.text = "[" + num + "/" + finished + "]" + " " + ((float)finished/num*100.0).ToString("0.0") + "%";
        //_attempts.text = String.Format("{0} {1}")
    }

    private void update_gold() {
        _model = GameView.load_game_model(PlayerPrefs.GetString("active_game"));
        long sob = 0;
        foreach (splitter.SplitMeta meta in _model.run.split_meta) {
            sob += meta.gold;
        }
        TimeSpan ts = TimeSpan.FromMilliseconds(sob);
        string format = "";
        if (ts.Hours > 0) {
            format += "HH";
        }
        _sob.text = ts.ToString(@"mm\:ss\.ff");

    }

    void Start() {
        update_gold();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
