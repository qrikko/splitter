using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterSettingsManager : MonoBehaviour
{
    private SettingsPanel[] _panels;
    // Start is called before the first frame update

    public void save_all() {
        foreach(GradientSettingsManager m in GetComponentsInChildren<GradientSettingsManager>(true)) {
            m.save_settings();
        }
    }

    public void Activate(SettingsPanel p) {
        foreach(SettingsPanel panel in _panels) {
            panel.gameObject.SetActive (false);
        }
        p.gameObject.SetActive(true);
    }

    void Awake() {
        _panels = GetComponentsInChildren<SettingsPanel>(true);
    }
}
