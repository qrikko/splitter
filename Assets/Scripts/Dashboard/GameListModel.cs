using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace speedrun
{
    [System.Serializable]
    public class GameListRun
    {
        public string id;
        public string weblink;
        public string game;
        public string level;
        public string category;
        public Videos videos;
        public string comment;
        public Status status;
        public Players[] players;
        public string date;
        public string submitted;
        public Times times;
        public GameSystem system;
        public SplitResources splits;
        // dunno how to handle values
        public Links[] links;
    }

    [System.Serializable]
    public class SplitResources
    {
        public string rel;
        public string uri;
    }

    [System.Serializable]
    public class GameSystem
    {
        public string platform;
        public bool emulated;
        public string region;
    }

    [System.Serializable]
    public class Times
    {
        public string primary;
        public string primary_t;
        public string realtime;
        public string realtime_t;
        public string realtime_noloads;
        public string realtime_noloads_t;
        public string ingame;
        public string ingame_t;
    }

    [System.Serializable]
    public class Players
    {
        public string rel;
        public string id;
        public string uri;
    }

    [System.Serializable]
    public class Status
    {
        public string status;
        public string examiner;
        [JsonProperty(PropertyName = "verify-date")]
        public string verify_date;
    }

    [System.Serializable]
    public class Videos
    {
        public Links[] links;
    }

    [System.Serializable]
    public class GameListData
    {
        public int place;
        public GameListRun run;
    }

    [System.Serializable]
    public class GameListModel
    {
        public GameListData[] data;

    }

    [System.Serializable]
    public class GameSearchModel
    {
        public GameData[] data;
    }
}