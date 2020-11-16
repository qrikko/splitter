using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

using Newtonsoft.Json;

using UnityEngine.UI;
using TMPro;

public class SpeedrunAPIGameSplitController : MonoBehaviour {
    [SerializeField] private Split _split_row_prefab        = null;
    [SerializeField] private GameObject _split_container    = null;

    [SerializeField] private TMP_InputField _split_name = null;
    [SerializeField] private TMP_Dropdown _saved_splits = null;
    [SerializeField] private TMP_InputField _name       = null;
    [SerializeField] private TMP_Dropdown _category     = null;
    [SerializeField] private TMP_InputField _start_time = null;
    [SerializeField] private TMP_InputField _attempts   = null;
    [SerializeField] private Image _thumb               = null;

    [SerializeField] private GameObject _name_select = null;

    private speedrun.RunModel _split_model;
    private speedrun.Categories _categories;
    private string _id;

    public void set_limerick(bool true_or_false) {
        _split_model.run.game_meta.limerick = true_or_false;
    }

    public void back_button() {
        save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Dashboard");
    }
    public void to_splits() {
        save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Timer");
    }

    public void update_category()
    {
        _split_model.run.game_meta.catergory = _category.options[_category.value].text;
        save();
    }

    public void update_start_time()
    {
        _split_model.run.game_meta.start_offset = _start_time.text;
    }

    public void update_attempts ()
    {
        _split_model.run.game_meta.attempts_count = int.Parse(_attempts.text);
    }

    // name the file?
    private void save()
    {
        string path = Application.persistentDataPath + "/" + _id + "/splits/" + _split_name.text + ".json";
        
        if (_split_model != null) {
            _split_model.save(path);
        }
    }

    private IEnumerator fetch_categories(string uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            } else
            {
                _categories = JsonConvert.DeserializeObject<speedrun.Categories>(request.downloadHandler.text);
                List<string> options = new List<string>();
                foreach (var i in _categories.data)
                {
                    options.Add(i.name);
                }
                _category.AddOptions(options);
            }
        }
    }

    public void delete_splits() {
        string path_to_delete = PlayerPrefs.GetString(_id);
        if (File.Exists(PlayerPrefs.GetString(_id))) {
            File.Delete(PlayerPrefs.GetString(_id));
            // also need to do more cleanup!
            
            _saved_splits.options.RemoveAt(_saved_splits.value);
            
            _split_name.text = Path.GetFileNameWithoutExtension(_saved_splits.options[1].text);
            _saved_splits.value = 1;
            string name = _saved_splits.options[1].text;
           
            string path = Application.persistentDataPath + "/" + _id + "/splits/" + name;
            PlayerPrefs.SetString(_id, path);

            BinaryFormatter formatter = new BinaryFormatter();
            string meta_path = Application.persistentDataPath + "/" + _id + "/splits/meta.data";
            FileStream stream = new FileStream(meta_path, FileMode.Create);

            formatter.Serialize(stream, _split_name.text);
            stream.Flush();
            stream.Close();
            //load_splits();
        }
    }

    public void cancle_new_splits() {
        _name_select.gameObject.SetActive(false);
        string id = Path.GetFileName(PlayerPrefs.GetString(_id));
        for (int i = 0; i < _saved_splits.options.Count; i++) {
            TMP_Dropdown.OptionData o = _saved_splits.options[i];
            if (o.text == id) {
                _saved_splits.value = i;
                break;
            }
        }
    }
    public void create_new_splits(TMP_InputField name) {
        // take the name and add it to the _saved_splists, also set the value

        string path = Application.persistentDataPath + "/" + _id + "/splits/" + name.text + ".json";
        PlayerPrefs.SetString(_id, path);

        foreach(Transform t in _split_container.transform) {
            Destroy(t.gameObject);
        }

        _saved_splits.options.Add(new TMP_Dropdown.OptionData(name.text + ".json"));
        _saved_splits.value = _saved_splits.options.Count -1;

        _split_name.text = Path.GetFileNameWithoutExtension(name.text);
        _start_time.text = "";
        _category.value = 0;
        _attempts.text = "0";
        _name_select.gameObject.SetActive(false);

        save();
    }

    public void new_splits() {
        //_split_model.run.split_meta;
        //SceneManager.LoadScene("Enter splits name", LoadSceneMode.Additive);
        _name_select.gameObject.SetActive(true);
        
        //_saved_splits.value = 0;
    }

    public void add_split() {
        speedrun.SplitMeta split = new speedrun.SplitMeta();
        _split_model.run.split_meta.Add(split);

        Split s = Split.Instantiate(_split_row_prefab, _split_container.transform);
        s.model = split;
    }

// need func to reload splits on _saved_splits change
    public void load_splits(int index) {
        save();
        
        // this is written to know what split file to load when we start the splits        
        BinaryFormatter formatter = new BinaryFormatter();
        string meta_path = Application.persistentDataPath + "/" + _id + "/splits/meta.data";
        FileStream stream = new FileStream(meta_path, FileMode.Create);

        formatter.Serialize(stream, _split_name.text);
        stream.Flush();
        stream.Close();

        if (index == 0) {
            new_splits();
            return;
        }
        _split_name.text = Path.GetFileNameWithoutExtension(_saved_splits.options[index].text);
        string path = Application.persistentDataPath + "/" + _id + "/splits/" + _split_name.text + ".json";
        PlayerPrefs.SetString(_id, path);

        load_splits();
    }

    private void populate_splits() {
        string path = Application.persistentDataPath + "/" + _id + "/splits/";
        DirectoryInfo d = new DirectoryInfo(path);
        FileInfo[] splits = d.GetFiles("*.json");
                
        string saved_splits_name = PlayerPrefs.GetString(_id);
        string filename = Path.GetFileName(saved_splits_name);
        
        for(int i=0; i<splits.Length; i++) {
            FileInfo s = splits[i];
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(s.Name);
            _saved_splits.options.Add(option);
            
            if (s.Name == filename) {
                _saved_splits.value = _saved_splits.options.Count;
            }
        }
        //_saved_splits.AddOptions(options);
    }

    private void load_splits() {
        _split_model = GameView.load_game_model(_id);

        _name.text = _split_model.run.game_meta.name;
        _start_time.text = _split_model.run.game_meta.start_offset;
        _attempts.text = _split_model.run.game_meta.attempts_count.ToString();

        Texture2D tex = null;
        byte[] data;

        string thumb_path = Application.persistentDataPath + "/" + _id + "/game_thumb.png";
        if (File.Exists(thumb_path)) {
            data = File.ReadAllBytes(thumb_path);
            tex = new Texture2D(2, 2);
            tex.LoadImage(data);
            _thumb.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));   
        }

        foreach(Transform s in _split_container.transform) {
            Destroy(s.gameObject);
        }

        foreach (speedrun.SplitMeta s in _split_model.run.split_meta ) {
            Split split = Split.Instantiate(_split_row_prefab, _split_container.transform);
            split.model = s;
        }
    }

    // Start is called before the first frame update
    void Start() {
        _name_select.gameObject.SetActive(false);
        Screen.SetResolution(625,450, false);
        
        _id = PlayerPrefs.GetString("active_game");
        
        string uri = "https://www.speedrun.com/api/v1/games/" + _id + "/categories";
        StartCoroutine(fetch_categories(uri));

        string meta_path = Application.persistentDataPath + "/" + _id + "/splits/meta.data";
        if (File.Exists(meta_path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(meta_path, FileMode.Open);
            _split_name.text = formatter.Deserialize(stream) as string;
            stream.Flush();
            stream.Close();
        } else {
            _split_name.text = "split";
            string path = Application.persistentDataPath + "/" + _id + "/splits/split.json";
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            PlayerPrefs.SetString(_id, path);

            BinaryFormatter formatter = new BinaryFormatter();            
            FileStream stream = new FileStream(meta_path, FileMode.Create);

            formatter.Serialize(stream, _split_name.text);
            stream.Flush();
            stream.Close();

            //_split_name.text = "split.json";
            create_new_splits(_split_name);
        }
        
        populate_splits();
        load_splits();
    }

    public void request_save() {
        string path = Application.persistentDataPath + "/" + _id + "/splits/" + _split_name.text + ".json";
        _split_model.save(path);
    }

    public void request_init(string splitname = null) {
        if (splitname != null) {
            PlayerPrefs.SetString(_id, splitname);
        }
        
        var option = _saved_splits.options[0];
        _saved_splits.options.Clear();
        _saved_splits.options.Add(option);
        populate_splits();
        
        load_splits();
    }

    void OnEnable () {
        Split.on_glod_reset += request_save;
        ImportExportSplits.on_import_done += request_init;
    }
    void OnDisable () {
        Split.on_glod_reset -= request_save;
        ImportExportSplits.on_import_done -= request_init;
    }
}
