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
    public class GameModel {
        public int id;
        public int series_id;
        public string name;
        public string short_name;
        public int has_gametime;
        public int uses_gametime;
        public string forum;
        public Categories[] categories;
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
