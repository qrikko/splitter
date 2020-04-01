using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using Newtonsoft.Json;

namespace speedrun
{
    [System.Serializable]
    public class PinnedGames
    {
        public List<string> pinned = new List<string>();
    }


    public class GameListView : MonoBehaviour
    {
        
        [SerializeField] private GameListContent _game_list_content = null;

        private PinnedGames _pinned = new PinnedGames();

        public bool toggle_pin_game(string id)
        {
            bool result = false;
            if (_pinned == null)
            {
                _pinned = new PinnedGames();
            }
            if (_pinned.pinned.Contains(id))
            {
                // unpin
                _pinned.pinned.Remove(id);
                result = false;
            } else
            {
                // pin
                _pinned.pinned.Add(id);
                result = true;
            }
            save_pinned_games();
            load_pinned_games();
            return result;
        }

        private IEnumerator get_game_request(string uri)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.Log("Error: " + request.error);
                } else
                {
                    Debug.Log("\nRecieved: " + request.downloadHandler.text);
                    GameModel game_list = JsonUtility.FromJson<GameModel>(request.downloadHandler.text);

                    // Loop through user.data, and for each create a new row from a prefab
                        GameView game_view = Instantiate(_game_list_content.game_view_prefab, _game_list_content.transform);
                        game_view.set_game(game_list.data.id);
                    
                }
            }
        }

        public void load_pinned_games()
        {
            // fetch the pinned games, 
            string path = Application.persistentDataPath + "pinned.json";
            if (System.IO.File.Exists(path))
            {
                // read from the file into _pinned
                System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open);
                System.IO.StreamReader sr = new System.IO.StreamReader(fs);
                _pinned = JsonConvert.DeserializeObject<PinnedGames>(sr.ReadToEnd());
                sr.Close();
                fs.Close();
            }

            foreach (string game in _pinned.pinned)
            {
                string url = "https://www.speedrun.com/api/v1/games/" + game;
                StartCoroutine(get_game_request(url));
            }
        }

        public void save_pinned_games ()
        {
            string path = Application.persistentDataPath + "pinned.json";
            
            // read from the file into _pinned
            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
            string json = JsonConvert.SerializeObject(_pinned);
            sw.Write(json);
            sw.Close();
            fs.Close();
        }
        public void refresh_game_list()
        {
            string uri = "https://www.speedrun.com/api/v1/users/" + PlayerPrefs.GetString("userid") + "/personal-bests";
            StartCoroutine(get_request(uri));
        }

        private IEnumerator get_request(string uri)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError)
                {
                    Debug.Log("Error: " + request.error);
                } else
                {
                    Debug.Log("\nRecieved: " + request.downloadHandler.text);
                    GameListModel game_list = JsonUtility.FromJson<GameListModel>(request.downloadHandler.text);

                    // Loop through user.data, and for each create a new row from a prefab
                    foreach (GameListData game in game_list.data)
                    {
                        GameView game_view = Instantiate(_game_list_content.game_view_prefab, _game_list_content.transform);
                        game_view.set_game(game.run.game);
                    }
                }
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            refresh_game_list();
            load_pinned_games();
        }
    }
}