/* This script is only meant for debugging reasons, to help keep track of
    things like IDs and hashed values since it's a bit of a pain to handle them.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DebugInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void print_debug() {
        Debug.Log("active game: " + PlayerPrefs.GetString("userid"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) {
            print_debug();
        }
        
    }
}
