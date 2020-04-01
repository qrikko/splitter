using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameListContent : MonoBehaviour
{
    [SerializeField] private GameView _game_view_prefab = null;
    public GameView game_view_prefab { get { return _game_view_prefab; } }
}
