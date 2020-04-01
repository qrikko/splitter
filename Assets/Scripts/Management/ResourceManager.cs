using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public enum SpeedrunImageTypes {
    thumb,
    count
}

public class ResourceManager : MonoBehaviour {
    public delegate void sr_game_image_callback(Sprite s);
    public void sr_game_image(SpeedrunImageTypes type, sr_game_image_callback callback) {
        // fetch image and send it via the delegate
        // the main idea:
        // 1. check if we already have the image cached in memory
        // 2. if not, check if we have it cached on disk
        // 3. if not, fetch it from sr.com and cache it to disk and primary cache

        // 3.
        //string url = ""
        //StartCoroutine();
    }

    public static ResourceManager instance {
        get {
            if (_instance == null) {
                _instance = new ResourceManager();
            }
            return _instance;
        }
    }

    private static ResourceManager _instance;
    private ResourceManager() {
        if (_instance == null) {
            _instance = new ResourceManager();
        }
    }
}
