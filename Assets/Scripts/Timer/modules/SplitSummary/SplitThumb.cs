using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SplitThumb : MonoBehaviour {
    private Image _thumb;

    private void update_thumb(Image thumb) {
        _thumb.sprite = thumb.sprite;
    }

    void OnEnable() {
        _thumb = GetComponent<Image>();
        SplitsManager.on_update_split_thumb += update_thumb;
    }
}
