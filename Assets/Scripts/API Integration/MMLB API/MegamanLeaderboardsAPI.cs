using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace mmlbapi {
    public class MegamanLeaderboardsAPI : MonoBehaviour {
        private GamesModel _games = new GamesModel();

/*
        public List<GameModel> game_search (string terms) {
            List<GameModel> search_results = new List<GameModel>();
            foreach(GameModel game in _games.games_list.games) {
                if (game.name.ToLower().Contains(terms.ToLower())) {
                    search_results.Add(game);
                }
            }
            return search_results;
        }
*/

        public void populate_search(string terms, GameListContent games) {
            foreach(GameModel data in _games.games_list.games) {
                if (data.name.ToLower().Contains(terms.ToLower())) {
                    GameView game_view = Instantiate(games.game_view_prefab, games.transform);
                    game_view.title = data.name;
                }
            }
        }

/*
        private IEnumerator search_game(string terms, GameListContent games) {
            string uri = "http://www.speedrun.com/api/v1/games?name=" + UnityWebRequest.EscapeURL(terms);
            using (UnityWebRequest request = UnityWebRequest.Get(uri)) {
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                
                if (request.isNetworkError) {
                    Debug.Log("Error: " + request.error);
                } else {
                    GameSearchModel game_list = JsonUtility.FromJson<GameSearchModel>(request.downloadHandler.text);

                    foreach (GameData data in game_list.data) {
                        GameView game_view = Instantiate(games.game_view_prefab, games.transform);
                        game_view.set_game(data.id);
                    }
                }
            }
        }
*/


        private IEnumerator fetch_game_list() {
            string uri = "http://megamanleaderboards.net/api/games.php";
            
            using (UnityWebRequest request = UnityWebRequest.Get(uri)) {
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                if (request.isNetworkError) {
                    Debug.Log("Error: " + request.error);
                } else {
                    _games.games_list = JsonUtility.FromJson<GamesListModel>(request.downloadHandler.text);
                    _games.update_date = System.DateTime.Now;

                    BinaryFormatter bf = new BinaryFormatter();
                
                    string path = Application.persistentDataPath + "/mmlb_cache/";
                    Directory.CreateDirectory(path);
                    path += "games.mmlb";

                    FileStream fs = new FileStream(path, FileMode.Create);
                    bf.Serialize(fs, _games);
                    
                    fs.Flush();
                    fs.Close();
                }
            }
        }

        // Start is called before the first frame update
        void Awake() {
            //@todo: should save the json-file for cached use and check if we already have it so we don't
            // generate a lot of traffic for something that is not changing very often, we might need a way to 
            // push a forced update to it perhaps we could do a fresh download once per year.. month? or if we have a search
            // not resulting in anything, we could push a forced update, and only allow a forced push once a day or something?

            string path = Application.persistentDataPath + "/mmlb_cache/" + "games.mmlb";
            if (File.Exists(path)) {
                BinaryFormatter bf = new BinaryFormatter();
                
                FileStream fs = new FileStream(path, FileMode.Open);
                _games = bf.Deserialize(fs) as GamesModel;
                fs.Flush();
                fs.Close();
            } else {
                StartCoroutine(fetch_game_list());
            }            
        }
    }
}
