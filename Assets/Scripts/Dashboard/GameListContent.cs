using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameListContent : MonoBehaviour
{
    //@todo: refactor, we don't want GameView, rewrite it, it is hard connected to SRC
    // we want a Game-something which is generic and agnostic from specific backend api
    
    [SerializeField] private GameView _game_view_prefab = null;
    public GameView game_view_prefab { get { return _game_view_prefab; } }
}
