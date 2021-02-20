using System.Collections;
using System.Collections.Generic;

namespace mmlbapi {
    [System.Serializable]
    public class Categories {
        public int id;
        public string name;
    }

    [System.Serializable]
    public class Series {
        public int id;
        public string name;
    }

    [System.Serializable]
    public class SeriesModel {
        public Series[] series;
    }

    [System.Serializable]
    public class GameModel : GenericGameModel {
        public int id;
        public int series_id;
        public string name;
        public string short_name;
        public int has_gametime;
        public int uses_gametime;
        public string forum;
        public Categories[] categories;

        public override void save() {
            string path = UnityEngine.Application.persistentDataPath + "/game_cache/" + guid + "/game.model";
            // Need to check if the path to the file exists, or create it, or is FileMode.OpenOrCreate enough?
            // if(System.IO.File.Exists(path)) {}

            if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(path)) == false) {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            }
            
            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate);
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            sw.Write(json);
            sw.Close();
            fs.Close();
        }

        public override void load(string game_id) {            
            string path = UnityEngine.Application.persistentDataPath + "/game_cache/" + game_id + "/game.model";
            // Need to check if the path to the file exists, or create it, or is FileMode.OpenOrCreate enough?
            
            if(System.IO.File.Exists(path)) {
                System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open);
                System.IO.StreamReader sr = new System.IO.StreamReader(fs);

                var tmp = Newtonsoft.Json.JsonConvert.DeserializeObject<GameModel>(sr.ReadToEnd());
                
                id = tmp.id;
                series_id = tmp.series_id;
                name = tmp.name;
                short_name = tmp.short_name;
                has_gametime = tmp.has_gametime;
                uses_gametime = tmp.uses_gametime;
                forum = tmp.forum;
                categories = tmp.categories;

                title = tmp.title;
                guid = tmp.guid;
                api_uri = tmp.api_uri;

                sr.Close();
                fs.Close();
            }
        }
    }

    [System.Serializable]
    public class GamesListModel {
        public GameModel[] games;
    }

    [System.Serializable]
    public class GamesModel {
        public GamesListModel games_list;
        public System.DateTime update_date;
    }
}
