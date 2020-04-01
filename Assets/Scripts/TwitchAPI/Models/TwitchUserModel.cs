using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace TwitchAPI {
    [System.Serializable]
    public class UserModel {
        public string id; // or string?
        public string login;
        public string display_name;
        public string type;
        public string broadcaster_type;
        public string description;
        public string profile_image_url;
        public string offline_image_url;
        public int view_count;
    }

    [System.Serializable]
    public class TwitchUserModel {
        public UserModel[] data;
    }
}
