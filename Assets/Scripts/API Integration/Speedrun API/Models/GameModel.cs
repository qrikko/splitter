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
    public class GameData
    {
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