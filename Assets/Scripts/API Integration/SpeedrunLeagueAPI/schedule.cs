using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine;

namespace SpeedrunningLeagueAPI {
    public class schedule : MonoBehaviour {
        [SerializeField] private SL_MatchRow _match_row_prefab  = null;
        private MatchesModel _model;
        public MatchesModel model {
            set {
                _model = value;
                foreach (MatchModel m in _model.data) {
                    SL_MatchRow.Instantiate(_match_row_prefab, transform).model = m;

                }
            }
        }

        void Start() {
            string url = "http://localhost:1337/api/" + PlayerPrefs.GetString("userid") + "/matches";
            Debug.Log(url);
            StartCoroutine(get_request(url));
        }

        private IEnumerator get_request(string uri) {
            using (UnityWebRequest request = UnityWebRequest.Get(uri)) {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success) {
                    Debug.Log("\nRecieved: " + request.downloadHandler.text);
                    model = JsonConvert.DeserializeObject<MatchesModel>(request.downloadHandler.text);
                } else {
                    Debug.Log("Error: " + request.error);
                }
            }
        }
    }
}