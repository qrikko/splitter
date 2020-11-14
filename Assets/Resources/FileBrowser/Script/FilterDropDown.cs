using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class FilterDropDown : MonoBehaviour
{
    [SerializeField] private FileBrowserSettings _settings;
    // Start is called before the first frame update
    void Start()
    {
        TMP_Dropdown dropdown = GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> optionsdata = new List<TMP_Dropdown.OptionData>();
        foreach(ThumbMap filter in _settings.thumb_mappings) {            
            optionsdata.Add(new TMP_Dropdown.OptionData(filter.name, filter.image));
        }
        dropdown.AddOptions(optionsdata);
    }
}
