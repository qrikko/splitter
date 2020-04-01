using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using TMPro;

public class GameView : MonoBehaviour
{
    [SerializeField] private Image _thumb = null;
    [SerializeField] private TMP_Text _title = null;

    protected speedrun.GameModel _model;

    // Start is called before the first frame update
    public void start_game_syncronized() {
        // 1. register run with server
        string url = srl.APIServer.url_root + "/api/register/";
        StartCoroutine(register_run(url));
    }

    private IEnumerator register_run(string url) {
        WWWForm form = new WWWForm();
        // helps us know if this is home or away arena, if the game id is the runners home game we are the home player
        // not foolproof but somewhere to start.
        form.AddField("gameid", _model.data.id);
        // need the twitch name which we can get using the userid.
        form.AddField("runnerid", PlayerPrefs.GetString("userid"));
        // we also need to figure out which match this is, there are a couple of ways ultimately
        // but my first go at it, will be to take the match closest in time from NOW(), where playerid is one of the players
        // and gameid is the game for the match.

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            } else {
                //_model = JsonConvert.DeserializeObject<speedrun.GameModel>(request.downloadHandler.text);
            }
        }
    }

    public void start_game() {
        PlayerPrefs.SetString("active_game", _model.data.id);
        string path = Application.persistentDataPath + "/" + _model.data.id + "/splits/split.json";

        if (File.Exists(path)){
            SceneManager.LoadScene("Timer");
        } else {
            SceneManager.LoadScene("Timer Settings");
        }
        save_game_model();
    }

    public void start_settings() {
        PlayerPrefs.SetString("active_game", _model.data.id);
        string path = Application.persistentDataPath + "/" + _model.data.id + "/splits/split.json";

        SceneManager.LoadScene("Timer Settings");
        save_game_model();
    }

    public void set_game(string game)
    {
        string uri = "https://www.speedrun.com/api/v1/games/" + game;
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

                _model = JsonConvert.DeserializeObject<speedrun.GameModel>(request.downloadHandler.text);

                string path = Application.persistentDataPath + "/" + _model.data.id;
                Directory.CreateDirectory(path);

                if (_title != null) {
                    _title.text = _model.data.names.international;
                }
                // Removed functionality since it really doesn't seem to work at all!
                
                //     speedrun.PlatformCache cache = GameListManager._platform_cache;
                //     foreach(string key in _model.data.platforms) {
                //         _system.text = "";
                //         if (cache.platforms.ContainsKey(key)) {
                //             _system.text += ", " +  GameListManager._platform_cache.platforms[key].name;
                //         } else {
                //             _system.text = "Unknown";
                //         }
                //     }
                // }
                StartCoroutine(get_texture());
            }
        }
    }

    private IEnumerator get_texture()
    {
        string url = _model.data.assets.cover_medium.uri;
        UnityWebRequest texture_request = UnityWebRequestTexture.GetTexture(url);
        yield return texture_request.SendWebRequest();

        if (texture_request.isNetworkError || texture_request.isHttpError)
        {
            Debug.Log(texture_request.error);
        } else
        {
            Texture2D texture = ((DownloadHandlerTexture)texture_request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
            _thumb.sprite = sprite;

            // Store the file locally
            byte[] img = texture.EncodeToPNG();
            string path = Application.persistentDataPath + "/" + _model.data.id + "/game_thumb.png";
            File.WriteAllBytes(path, img);
        }
    }
    
    private void save_game_model()
    {
        BinaryFormatter formatter = new BinaryFormatter();
                
        string path = Application.persistentDataPath + "/" + _model.data.id + "/gameinfo.bgi";
        //Directory.CreateDirectory(Path.GetDirectoryName(path));

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, _model);
        stream.Flush();
        stream.Close();
    }

    public static speedrun.RunModel load_game_model(string game_id)
    {
        //string path = Application.persistentDataPath + "/" + game_id + "/splits/split.json";
        string path = PlayerPrefs.GetString(game_id);

        if (File.Exists(path))
        {
            //BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            string json = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            speedrun.RunModel model = JsonConvert.DeserializeObject<speedrun.RunModel>(json);
            return model;
        } else {
            Debug.Log("no file found for: " + path);
            Debug.Log("creating from known data");

            string game_path = Application.persistentDataPath + "/" + game_id + "/gameinfo.bgi";

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(game_path, FileMode.Open);
            speedrun.GameModel game_model = formatter.Deserialize(fs) as speedrun.GameModel;
            fs.Close();

            speedrun.RunModel model = new speedrun.RunModel();
            model.run = new speedrun.Run();
            model.run.game_meta.thumb_path = "game_thumb.png";
            model.run.game_meta.name = game_model.data.names.international;

            // also need to write it to file, so we aren't missing it next time!

            return model;
        }
    }
}
