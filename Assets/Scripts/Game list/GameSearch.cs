using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using speedrun;

public class GameSearch : MonoBehaviour
{
    [SerializeField] GameListContent _game_list_content = null;
    
    public void update_game_search()
    {
        string terms = GetComponent<TMP_InputField>().text;
        if (terms.Length > 2)
        {
            StartCoroutine(search_game(terms));
        }
    }

    private IEnumerator search_game(string terms)
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
}
