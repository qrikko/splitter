using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LastSplit : MonoBehaviour {
    void OnEnable() {
        SplitsManager.final_split_image += set_image;
    }

    void OnDisable() {
        SplitsManager.final_split_image -= set_image;
    }

    private void set_image(Image img) {
        GetComponent<Image>().sprite = img.sprite;
        GetComponent<Image>().preserveAspect = true;
    }
}
