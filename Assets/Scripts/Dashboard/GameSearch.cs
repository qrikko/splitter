using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
//using speedrun;

public struct GenericGameModel {
    public string title;
    public int guid;
}

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

/*    private IEnumerator search_game(string terms)
    {        
        string uri = "http://www.speedrun.com/api/v1/games?name=" + UnityWebRequest.EscapeURL(terms);
        
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.isNetworkError) {
                Debug.Log("Error: " + request.error);
            } else {
                GameSearchModel game_list = JsonUtility.FromJson<GameSearchModel>(request.downloadHandler.text);

                foreach (Transform child in _game_list_content.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }

                // Loop through user.data, and for each create a new row from a prefab
                foreach (GameData game in game_list.data)
                {
                    GameView game_view = Instantiate(_game_list_content.game_view_prefab, _game_list_content.transform);
                    game_view.set_game(game.id);
                }
            }
        }
        
    }
*/

}
