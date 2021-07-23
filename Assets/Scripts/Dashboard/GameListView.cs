using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Newtonsoft.Json;

using PinnedGames = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>;

namespace splitter {
    public class GameListView : MonoBehaviour {
        [SerializeField] private GameListContent _game_list_content = null;
        private PinnedGames _pinned = new PinnedGames();

        public bool toggle_pin_game(GenericGameModel m) {
            bool result = false;

            if (_pinned.ContainsKey(m.api_uri) == false) {
                _pinned[m.api_uri] = new List<string>();
            }

            if (_pinned[m.api_uri].Contains(m.guid)) {
                //unpin
                _pinned[m.api_uri].Remove(m.guid);
                result = false;
            } else {
                // pin it!
                _pinned[m.api_uri].Add(m.guid);
                result = true;

                // and write the game model to cache!
                m.save();
            }

            save_pinned_games();
            load_pinned_games();
            return result;
        }

// @todo: this is not fixed... or used currently
        private IEnumerator get_game_request(string uri)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success) {
                    // expects speedrun.GameModel, but should be api agnostic here
                    //GameModel game_list = JsonUtility.FromJson<GameModel>(request.downloadHandler.text);

                    // Loop through user.data, and for each create a new row from a prefab
                        GameView game_view = Instantiate(_game_list_content.game_view_prefab, _game_list_content.transform);
                    //    game_view.set_game(game_list.data.id);
                } else {
                    Debug.Log("Error: " + request.error);                    
                }
            }
        }

        public void load_pinned_games() {
            string path = Application.persistentDataPath + "/pinned.json";
            if (System.IO.File.Exists(path))
            {
                // read from the file into _pinned
                System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open);
                System.IO.StreamReader sr = new System.IO.StreamReader(fs);

                _pinned = JsonConvert.DeserializeObject<PinnedGames>(sr.ReadToEnd());

                // clear the view before we populate it
                foreach (Transform t in _game_list_content.transform) {
                    Destroy(t.gameObject);
                }

                foreach (KeyValuePair<string, List<string>> api in _pinned) {
                    foreach(string guid in api.Value) {
                        GameView game_view = Instantiate(_game_list_content.game_view_prefab, _game_list_content.transform);
                        game_view.load(guid, api.Key);
                        //--game_view.set_model()
                        //game_view.set_game(game.run.game);
                    }
                }

                sr.Close();
                fs.Close();
            }
        }

        public void save_pinned_games () {
            string path = Application.persistentDataPath + "/pinned.json";
            
            // read from the file into _pinned
            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);

            string json = JsonConvert.SerializeObject(_pinned, Formatting.Indented);
            sw.Write(json);
            sw.Close();
            fs.Close();
        }

        public void refresh_game_list() {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("userid"))) {
                Debug.LogWarning("no user is set, to be able to fetch your runs we expect a user...");
            } else {
                string uri = "https://www.speedrun.com/api/v1/users/" + PlayerPrefs.GetString("userid") + "/personal-bests";
                StartCoroutine(get_request(uri));
            }
        }

// @todo: this is old or unused
        private IEnumerator get_request(string uri) {
            using (UnityWebRequest request = UnityWebRequest.Get(uri)) {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success) {
                    // expects speedrun.GameListModel, which is removed to make it api agnostic!
                    //GameListModel game_list = JsonUtility.FromJson<GameListModel>(request.downloadHandler.text);

                    // Loop through user.data, and for each create a new row from a prefab
//                    foreach (GameListData game in game_list.data)
//                    {
//                        GameView game_view = Instantiate(_game_list_content.game_view_prefab, _game_list_content.transform);
//                        game_view.set_game(game.run.game);
//                    }
                } else {
                    Debug.Log("Error: " + request.error);
                }
            }
        }

        void Start() {
            refresh_game_list();
            load_pinned_games();
        }
    }
}