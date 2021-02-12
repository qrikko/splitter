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

            if (texture_request.isNetworkError || texture_request.isHttpError) {
                Debug.Log(texture_request.error);
            } else {
                Texture2D texture = ((DownloadHandlerTexture)texture_request.downloadHandler).texture;
                _game_thumb_cache[id] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
                callback(_game_thumb_cache[id]);

                // Store the file locally
                byte[] img = texture.EncodeToPNG();
                string path = Application.persistentDataPath + "/" + id + "/game_thumb.png";
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllBytes(path, img);
            }
        }
//#pragma comment (Private);
        // Private
        private IEnumerator request_game (string game_id, fetch_game_model_callback callback) {
            string uri = "https://www.speedrun.com/api/v1/games/" + game_id;
            using (UnityWebRequest request = UnityWebRequest.Get(uri))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError) {
                    Debug.Log("Error: " + request.error);
                } else {
                    _game_model_cache[game_id] = JsonConvert.DeserializeObject<speedrun.GameModel>(request.downloadHandler.text);

                    callback(_game_model_cache[game_id]);
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
