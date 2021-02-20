using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GameRowView : GameView
{
    [SerializeField] private Button _pin_button = null;

    private splitter.GameListView _game_list_view = null;

    private void Awake()
    {
        _game_list_view = GameObject.FindGameObjectWithTag("game_list_view").GetComponent<splitter.GameListView>();
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
