using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AssetCache = System.Collections.Generic.Dictionary<splitter.AssetType, UnityEngine.Sprite>;
using DownloadCallback = System.Func<object, System.ComponentModel.AsyncCompletedEventArgs>;

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

    public delegate void ImageAvaliable(Sprite sprite);

    [System.Serializable]
    public abstract class GenericGameModel {
        public string title;
        public string guid;
        public string api_uri;
        

        protected AssetCache _asset_cache = new AssetCache();

        public abstract void save();
        public abstract void load(string game_id);
        public abstract void get_asset(AssetType type, ImageAvaliable callback);

        protected void image_from_cache(string path, splitter.AssetType type) {
            if (System.IO.File.Exists(path) == false) {
                _asset_cache[type] = null;
                return;
            }

            byte[] img_data = System.IO.File.ReadAllBytes(path);
            var foo = System.IO.File.GetAttributes(path);
        //    int w = assets.cover_medium.width;
        //    int h = assets.cover_medium.height;
            UnityEngine.Texture2D tex = new UnityEngine.Texture2D(
                0, 0, TextureFormat.RGB24, false
            );
            tex.LoadImage(img_data);
            
            Rect rect = new Rect(0,0,tex.width,tex.height);
            Sprite sprite = Sprite.Create(tex, rect, new Vector2(0.5f,0.0f), 1.0f);

            _asset_cache[type] = sprite;
        }
    }
}