using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitVerticalBar : MonoBehaviour
{
    [SerializeField][Range(0,1)] float _min_offset = 0.3f;
    private Scrollbar _scrollbar;
    void OnEnable() {
        SplitsManager.on_reset += to_top;
    }

    public void split(float offset) {
        if(offset > _min_offset) {
            _scrollbar.value = 1-offset;
        }
    }

    private void to_top(string s) {
        _scrollbar.value = 1;
    }

    void Awake() {
        _scrollbar = GetComponent<Scrollbar>();
    }
}
