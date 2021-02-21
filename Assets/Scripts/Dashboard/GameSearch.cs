using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class GameSearch : MonoBehaviour {
    [SerializeField] GameListContent _game_list_content = null;
    [SerializeField] private bool _include_src = true;
    [SerializeField] private bool _include_mmlb = true;

    [SerializeField] private speedrun.SpeedrunAPI srcapi;
    [SerializeField] private mmlbapi.MegamanLeaderboardsAPI mmlbapi;
    
    public void update_game_search() {
        string terms = GetComponent<TMP_InputField>().text;
        if (terms.Length > 2) {
            //List<GenericGameModel> games = new List<GenericGameModel>(5);
            foreach (Transform gv in _game_list_content.transform) {
                Destroy (gv.gameObject);
            }

            if (_include_src) {
                srcapi.populate_search(terms, _game_list_content);                
            }
            if (_include_mmlb) {
                mmlbapi.populate_search(terms, _game_list_content);
            }

           // StartCoroutine(search_game(terms));
        }
    }
}
