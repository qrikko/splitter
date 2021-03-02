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
    [SerializeField] protected Image _thumb = null;
    [SerializeField] private TMP_Text _title = null;
    [SerializeField] private Sprite _load_indicator = null;

    public string title {
        get { return _title.text; }
        set { _title.text = value; }
    }

    protected splitter.GenericGameModel _model;
    
    //@todo: Should be able to abstract the difference here as well I think..
    public void set_model(mmlbapi.GameModel model) {
        _model = model;
        _model.guid = model.id.ToString();
        _model.title = model.name;
        _model.api_uri = "http://megamanleaderboards.net/api/";
        
        _title.text = _model.title;
        _thumb.sprite = _load_indicator;

        _model.get_asset(splitter.AssetType.Thumb, (Sprite s) => {
            _thumb.sprite = s;
        });
    }

    public void set_model(speedrun.GameData model) {
        _model = model;
        _model.guid = model.id;
        _model.title = model.names.international;
        _model.api_uri = "https://www.speedrun.com/api/v1/";
        
        _title.text = _model.title;
        _thumb.sprite = _load_indicator;
        
        _model.get_asset(splitter.AssetType.Thumb, (Sprite s) => {
            _thumb.sprite = s;
        });
    }

    public void load(string guid, string api) {
        if(api == "http://megamanleaderboards.net/api/") {
            mmlbapi.GameModel model = new mmlbapi.GameModel();
            model.load(guid);
            set_model(model);
        } else if (api == "https://www.speedrun.com/api/v1/") {
            speedrun.GameData model = new speedrun.GameData();
            model.load(guid);
            set_model(model);
        }
    }

    public void start_game() {
        PlayerPrefs.SetString("active_game", _model.guid);
        string path = PlayerPrefs.GetString(_model.guid.ToString());

        if (File.Exists(path)) {
            SceneManager.LoadScene("Timer");
        } else {
            SceneManager.LoadScene("Split Settings");
        }        
    }

    public void start_settings() {
        PlayerPrefs.SetString("active_game", _model.guid.ToString());
        string path = Application.persistentDataPath + "/game_cache/" + _model.guid + "/splits/" + "split.json";

        SceneManager.LoadScene("Split Settings");
    }

    public void set_game(string guid) {
        PlayerPrefs.SetString("active_game", guid);
    }
    
    private void save_game_model()
    {
        BinaryFormatter formatter = new BinaryFormatter();
                
        string path = Application.persistentDataPath + "/game_cache/" + _model.guid + "/game.model";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, _model);
        stream.Flush();
        stream.Close();
    }

    public static splitter.RunModel load_game_model(string game_id) {
        string path = PlayerPrefs.GetString(game_id);
        
        if (File.Exists(path)) {
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            
            string json = sr.ReadToEnd();
            sr.Close();
            fs.Close();

            splitter.RunModel model = JsonConvert.DeserializeObject<splitter.RunModel>(json);
            return model;
        } else {
            Debug.Log("no file found for: " + path);
            Debug.Log("creating from known data");

            string game_path = Application.persistentDataPath + "/game_cache/" + game_id + "/game.model";
            FileStream fs = new FileStream(game_path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

//            splitter.GenericGameModel game_model = JsonConvert.DeserializeObject<splitter.GenericGameModel>(sr.ReadToEnd());            

            splitter.RunModel model = new splitter.RunModel();
            model.run = new splitter.Run();
            model.run.game_meta.thumb_path = "game_thumb.png";
//            model.run.game_meta.name = game_model.title;
            
            return model;
        }
    }
}
