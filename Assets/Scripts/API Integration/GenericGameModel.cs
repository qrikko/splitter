using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AssetCache = System.Collections.Generic.Dictionary<splitter.AssetType, UnityEngine.UI.Image>;

namespace splitter {
    public enum AssetType {
        Thumb,
        Background,
        Foreground,
        GoldTrophy,
        SilverTrophy,
        BronzeTrophy,
        Count
    };

    [System.Serializable]
    public abstract class GenericGameModel {
        public string title;
        public string guid;
        public string api_uri;

        protected AssetCache _asset_cache = new AssetCache();

        public abstract void save();
        public abstract void load(string game_id);
        public abstract UnityEngine.UI.Image get_asset(AssetType type);
    }
}