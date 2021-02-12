using System;

namespace SpeedrunningLeagueAPI {
    [System.Serializable]
    public class MatchModel {
        public int match_id;
        public DateTime start_time;
        public string home;
        public string away;
        public string home_srid;
        public string away_srid;
        public string game_name;
        public string game_srid;
    }
    [System.Serializable]
    public class MatchesModel {
        public MatchModel[] data;
    }

}