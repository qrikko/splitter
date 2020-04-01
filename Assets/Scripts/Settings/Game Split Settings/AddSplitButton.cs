using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSplitButton : MonoBehaviour {
    [SerializeField] private GameObject _splits = null;
    [SerializeField] private GameObject _split_prefab = null;

    public void add_split() {
        GameObject.Instantiate(_split_prefab, _splits.transform);
    }
}
