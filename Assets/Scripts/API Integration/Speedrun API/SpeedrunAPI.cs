using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace speedrun {
    public enum GameImageTypes {
        thumb,
        count
    };

    public class SpeedrunAPI : MonoBehaviour {
        private Dictionary<string, GameModel> _game_model_cache = new Dictionary<string, GameModel>();
        private Dictionary<string, Sprite> _game_thumb_cache = new Dictionary<string, Sprite>();
        public delegate void fetch_game_model_callback(GameModel result);
        public delegate void fetch_game_image_callback(Sprite s);

        public void populate_search(string terms, GameListContent games) {
            StartCoroutine(search_game(terms, games));
        }

        private IEnumerator search_game(string terms, GameListContent games) {
            string uri = "http://www.speedrun.com/api/v1/games?name=" + UnityWebRequest.EscapeURL(terms);
            using (UnityWebRequest request = UnityWebRequest.Get(uri)) {
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("User-Agent", "splitter/2.2.2, multi-platform speedrun timer! https://github.com/qrikko/splitter");
                yield return request.SendWebRequest();
                
                if (request.result == UnityWebRequest.Result.Success) {
                    //GameSearchModel game_list = JsonUtility.FromJson<GameSearchModel>(request.downloadHandler.text);
                    GameSearchModel game_list = JsonConvert.DeserializeObject<GameSearchModel>(request.downloadHandler.text);

                    foreach (GameData data in game_list.data) {
                        GameView game_view = Instantiate(games.game_view_prefab, games.transform);
                        //game_view.set_game(data.id);
                        game_view.set_model(data);
                    }
                } else {
                    Debug.Log("Error: " + request.error);
                }
            }
        }

        public void fetch_game_image(string game_id, GameImageTypes type, fetch_game_image_callback callback) {
            // which cache to use might be based on type, we'll start out only doing thumbs..
            if (_game_thumb_cache.ContainsKey(game_id)) {
                Debug.Log("Game Thumb[" + game_id + "]: image in cache, return it...");
                callback(_game_thumb_cache[game_id]);
            } else if (_game_model_cache.ContainsKey(game_id)) {
                Debug.Log("Game Thumb[" + game_id + "]: not in image cache, check on disk...");
                string path = Application.persistentDataPath + "/" + game_id + "/game_thumb.png";
                if (File.Exists(path)) {
                    Debug.Log("Game Thumb[" + game_id + "]: On disk, load it and cache it for reuse!");
                    byte [] data = File.ReadAllBytes(path);
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(data);
                    _game_thumb_cache[game_id] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
                    callback(_game_thumb_cache[game_id]);
                } else {
                    Debug.Log("Game Thumb[" + game_id + "]: Model cached, but not image, need to fetch");
                    StartCoroutine(request_game_image(game_id, callback));
                }
            } else {
                // no cache exists, so need to cache both image and model!
                Debug.Log("Game Thumb[" + game_id + "]: not in image or model -cache...");
                string path = Application.persistentDataPath + "/" + game_id + "/game_thumb.png";
                if (File.Exists(path)) {
                    Debug.Log("Game Thumb[" + game_id + "]: On disk, load and cache the image!");

                    byte [] data = File.ReadAllBytes(path);
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(data);
                    _game_thumb_cache[game_id] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
                    callback(_game_thumb_cache[game_id]);
                } else {
                    Debug.Log("Game Thumb[" + game_id + "]: not on disk, not in model and not in image cache, need to refetch all");
                    StartCoroutine(request_game(game_id, (GameModel m) => {
                        _game_model_cache[game_id] = m;
                        StartCoroutine(request_game_image(game_id, callback));
                    }));
                }
            }
        }

        public void fetch_game_model(string game_id, fetch_game_model_callback callback) {
            if (_game_model_cache.ContainsKey(game_id)) {
                callback(_game_model_cache[game_id]);
            } else {
                StartCoroutine(request_game(game_id, callback));
            }
        }

        private IEnumerator request_game_image(string id, fetch_game_image_callback callback) {
            string url = _game_model_cache[id].data.assets.cover_small.uri;
            UnityWebRequest texture_request = UnityWebRequestTexture.GetTexture(url);
            yield return texture_request.SendWebRequest();

            if (texture_request.result == UnityWebRequest.Result.Success) {
                Texture2D texture = ((DownloadHandlerTexture)texture_request.downloadHandler).texture;
                _game_thumb_cache[id] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
                callback(_game_thumb_cache[id]);

                // Store the file locally
                byte[] img = texture.EncodeToPNG();
                string path = Application.persistentDataPath + "/" + id + "/game_thumb.png";
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllBytes(path, img);
            } else {
                Debug.Log(texture_request.error);
            }
        }
//#pragma comment (Private);
        // Private
        private IEnumerator request_game (string game_id, fetch_game_model_callback callback) {
            string uri = "https://www.speedrun.com/api/v1/games/" + game_id;
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success) {
                    _game_model_cache[game_id] = JsonConvert.DeserializeObject<speedrun.GameModel>(request.downloadHandler.text);

                    callback(_game_model_cache[game_id]);
                } else {
                    Debug.Log("Error: " + request.error);
                }
            }
        }

        private static SpeedrunAPI _instance;

        public static SpeedrunAPI instance {
            get {
                return _instance;
            }
        }

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
        }
    }
}
