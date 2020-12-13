using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class OpacitySetup : MonoBehaviour {
    public float alpha {
        set {GetComponent<Slider>().value = value; }
    }
}
