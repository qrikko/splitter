using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameRowView : GameView
{
    [SerializeField] private Button _pin_button = null;
    [SerializeField] private FileBrowser _filebrowser;

    private splitter.GameListView _game_list_view = null;

    private void Awake()
    {
        _game_list_view = GameObject.FindGameObjectWithTag("game_list_view").GetComponent<splitter.GameListView>();
    }

    public void select_asset() {
        FileBrowser img_picker = Instantiate(_filebrowser);
        img_picker.gameObject.SetActive(true);
        string[] filters = {".png", ".jpg", ".jpeg", ".bmp", ".tga", ".gif"};
        
        img_picker.show((string path) => {
            string cache_path = Application.persistentDataPath + "/game_cache/" + _model.guid + "/assets/";
            System.IO.Directory.CreateDirectory(cache_path);
            cache_path += "Thumb" + System.IO.Path.GetExtension(path);
            System.IO.File.Copy(path, cache_path, true);
            _model.get_asset(splitter.AssetType.Thumb, (Sprite s) => {_thumb.sprite = s;});
        });
    }

    public void toggle_pin()
    {
        if (_game_list_view.toggle_pin_game(_model))
        {
            _pin_button.GetComponent<Image>().color = Color.blue;
        } else
        {
            _pin_button.GetComponent<Image>().color = Color.gray;
        }
    }
}
