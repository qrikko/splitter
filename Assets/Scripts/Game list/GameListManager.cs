using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

using Newtonsoft.Json;

public class GameListManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _username = null;
    public static speedrun.PlatformCache _platform_cache = new speedrun.PlatformCache();
    // Start is called before the first frame update
    void Awake()
    {
        Screen.SetResolution(365,625, false);
        _username.text = PlayerPrefs.GetString("username");
        StartCoroutine(get_platforms());
    }

    // should prefetch these and keep them in a structure so that we can fetch them there instead..
    private IEnumerator get_platforms() {
        string url = "https://www.speedrun.com/api/v1/platforms";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.isNetworkError) {
            Debug.Log("Error: " + request.error);
        } else {
            speedrun.PlatformMeta platforms = JsonConvert.DeserializeObject<speedrun.PlatformMeta>(request.downloadHandler.text);
            //_platform_cache = new speedrun.PlatformCache();

            foreach(speedrun.PlatformData p in platforms.data) {
                _platform_cache.platforms[p.id] = new speedrun.Platform(p.name, p.released);
            }
        }
    }
}
