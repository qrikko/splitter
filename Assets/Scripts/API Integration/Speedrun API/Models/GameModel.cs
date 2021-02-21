using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace speedrun
{
    [System.Serializable]
    public class CategoryPlayers
    {
        public string type;
        public int value;
    }

    [System.Serializable]
    public class CategoryModel
    {
        public string id;
        public string name;
        public string weblink;
        public string type;
        public string rules;
        public CategoryPlayers players;
        public string miscellaneous;
        public Links[] links;
    }

    [System.Serializable]
    public class Categories
    {
        public CategoryModel[] data;
    }

    [System.Serializable]
    public class GameData : GenericGameModel {
        public string id;
        public Names names;
        public string abbreviation;
        public string weblink;
        public int released;
        [JsonProperty(PropertyName = "release-date")]
        public string release_date;
        public Ruleset ruleset;
        public bool romhack;
        public string[] gametypes;
        public string[] platforms;
        public string[] regions;
        public string[] genres;
        public string[] engines;
        public string[] developers;
        public string[] publishers;
        // not sure how to handle moderators
        public string created;
        public Assets assets;
        public Links[] links;

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

                var tmp = Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(sr.ReadToEnd());
                
                id              = tmp.id;
                names           = tmp.names;
                abbreviation    = tmp.abbreviation;
                weblink         = tmp.weblink;
                released        = tmp.released;
                release_date    = tmp.release_date;
                ruleset         = tmp.ruleset;
                romhack         = tmp.romhack;
                gametypes       = tmp.gametypes;
                platforms       = tmp.platforms;
                regions         = tmp.regions;
                genres          = tmp.genres;
                engines         = tmp.engines;
                developers      = tmp.developers;
                publishers      = tmp.publishers;
                created         = tmp.created;
                assets          = tmp.assets;
                links           = tmp.links;

                title           = tmp.title;
                guid            = tmp.guid;
                api_uri         = tmp.api_uri;

                sr.Close();
                fs.Close();
            }
        }
    }

    [System.Serializable]
    public class Assets
    {
        public Image logo;
        [JsonProperty(PropertyName = "cover-tiny")]
        public Image cover_tiny;
        [JsonProperty(PropertyName = "cover-small")]
        public Image cover_small;
        [JsonProperty(PropertyName = "cover-medium")]
        public Image cover_medium;
        [JsonProperty(PropertyName = "cover-large")]
        public Image cover_large;
        public Image icon;
        [JsonProperty(PropertyName = "trophy-1st")]
        public Image trophy_1st;
        [JsonProperty(PropertyName = "trophy-2nd")]
        public Image trophy_2nd;
        [JsonProperty(PropertyName = "trophy-3rd")]
        public Image trophy_3rd;
        [JsonProperty(PropertyName = "trophy-4th")]
        public Image trophy_4th;
        public Image background;
        public Image foreground;
    }

    [System.Serializable]
    public class Image
    {
        public string uri;
        public int widht;
        public int height;
    }

    [System.Serializable]
    public class Ruleset
    {
        [JsonProperty(PropertyName = "show-milliseconds")]
        public bool show_milliseconds;
        [JsonProperty(PropertyName = "require-verification")]
        public bool require_verification;
        [JsonProperty(PropertyName = "require-video")]
        public bool require_video;
        [JsonProperty(PropertyName = "run-times")]
        public string[] run_times;
        [JsonProperty(PropertyName = "default-time")]
        public string default_time;
        [JsonProperty(PropertyName = "emulators-allowed")]
        public bool emulators_allowed;
    }

    [System.Serializable]
    public class GameModel {
        public GameData data;
    }
}