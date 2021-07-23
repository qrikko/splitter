using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

namespace speedrun
{
    public class Authentication : MonoBehaviour
    {
        public void submit_api_key(TMP_Text apikey)
        {
            Debug.Log("trying to authenticate key: " + apikey.text);
            StartCoroutine(get_request("https://www.speedrun.com/api/v1/profile", apikey.text.Trim((char)8203)));
            //StartCoroutine(get_request("https://www.speedrun.com/api/v1/profile", apikey.text));
        }

        private IEnumerator get_request(string uri, string key)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {

                //               "vj6fyqg9htwmnwwx92xd54vff";
                request.SetRequestHeader("X-API-Key", key);
                yield return request.SendWebRequest(); 

                if (request.result == UnityWebRequest.Result.Success) {
                    Debug.Log("\nRecieved: " + request.downloadHandler.text);
                    AuthenticationModel user = JsonUtility.FromJson<AuthenticationModel>(request.downloadHandler.text);
                    PlayerPrefs.SetString("userid", user.data.id);
                    PlayerPrefs.SetString("username", user.data.names.international);
                    SceneManager.LoadScene("Dashboard", LoadSceneMode.Single);
                } else {
                    Debug.Log("Error: " + request.error);
                }
            }                
        }
    }
}
