using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;
using Newtonsoft.Json;

namespace TwitchAPI {
    public class TwitchAPI : MonoBehaviour {
        public delegate void fetch_twitch_user_callback(UserModel m);
        public delegate void fetch_twitch_user_image_callback(Sprite s);

        private Dictionary<string, Sprite> _twitch_user_image_cache = new Dictionary<string, Sprite>();
        private Dictionary<string, UserModel> _twitch_user_cache = new Dictionary<string, UserModel>();

        public void fetch_runner_image(string login, fetch_twitch_user_image_callback callback) {
            if (_twitch_user_image_cache.ContainsKey(login)) {
                Debug.Log("Runner Thumb[" + login + "]: image in cache, return it...");
                callback(_twitch_user_image_cache[login]);
            } else if (_twitch_user_cache.ContainsKey(login)) {
                Debug.Log("Runner Thumb[" + login + "]: not in image cache, check on disk...");
                string path = Application.persistentDataPath + "/runners/" + login + "/runner_thumb.png";
                if (File.Exists(path)) {
                    Debug.Log("Runner Thumb[" + login + "]: On disk, load it and cache it for reuse!");
                    byte [] data = File.ReadAllBytes(path);
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(data);
                    _twitch_user_image_cache[login] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
                    callback(_twitch_user_image_cache[login]);
                } else {
                    Debug.Log("Runner Thumb[" + login + "]: Model cached, but not image, need to fetch");
                    StartCoroutine(request_user_image(login, callback));
                }
            } else {
                // no cache exists, so need to cache both image and model!
                Debug.Log("Runner Thumb[" + login + "]: not in image or model -cache...");
                string path = Application.persistentDataPath + "/runners/" + login + "/runner_thumb.png";
                if (File.Exists(path)) {
                    Debug.Log("Runner Thumb[" + login + "]: On disk, load it and cache it for reuse!");
                    byte [] data = File.ReadAllBytes(path);
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(data);
                    _twitch_user_image_cache[login] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
                    callback(_twitch_user_image_cache[login]);
                } else {
                    Debug.Log("Runner Thumb[" + login + "]: not on disk, not in model and not in image cache, need to refetch all");
                    StartCoroutine(request_user(login, (UserModel m) => {
                        _twitch_user_cache[login] = m;
                        StartCoroutine(request_user_image(login, callback));
                    }));
                }
            }
        }

        public void fetch_runner(string login, fetch_twitch_user_callback callback) {
            if (_twitch_user_cache.ContainsKey(login)) {
                callback(_twitch_user_cache[login]);
            } else {
                StartCoroutine(request_user(login, callback));
            }
        }

        private IEnumerator request_user_image(string login, fetch_twitch_user_image_callback callback) {
            string url = _twitch_user_cache[login].profile_image_url;
            UnityWebRequest texture_request = UnityWebRequestTexture.GetTexture(url);
            texture_request.SetRequestHeader("Client-ID", "nm92v8wkvmrhulyqrhfct9vljid72k");
            yield return texture_request.SendWebRequest();

            if (texture_request.result == UnityWebRequest.Result.Success) {
                Texture2D texture = ((DownloadHandlerTexture)texture_request.downloadHandler).texture;
                _twitch_user_image_cache[login] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
                callback(_twitch_user_image_cache[login]);

                // Store the file locally
                byte[] img = texture.EncodeToPNG();
                string path = Application.persistentDataPath + "/runners/" + login + "/runner_thumb.png";
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllBytes(path, img);
            } else {
                Debug.Log(texture_request.error);
            }
        }

        private IEnumerator request_user(string login, fetch_twitch_user_callback callback) {
            string uri = "https://api.twitch.tv/helix/users?login=" + login;
            UnityWebRequest request = UnityWebRequest.Get(uri);
            request.SetRequestHeader("Client-ID", "nm92v8wkvmrhulyqrhfct9vljid72k");

            yield return request.SendWebRequest();

            if (request.isNetworkError) {
                Debug.Log("Error: " + request.error);
            } else {
                _twitch_user_cache[login] = JsonConvert.DeserializeObject<TwitchUserModel>(request.downloadHandler.text).data[0];

                callback(_twitch_user_cache[login]);
            }
        }

        private static TwitchAPI _instance;

        public static TwitchAPI instance {
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