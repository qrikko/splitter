using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace speedrun {
    [System.Serializable]
    public class Platform {
        public string name;
        public int released;

        public Platform(string n, int r) {
            name = n;
            released = r;
        }
    }
    [System.Serializable]
    public class PlatformCache {
        public Dictionary<string, Platform> platforms = new Dictionary<string, Platform>();
    }

    [System.Serializable]
    public class PlatformData {
        public string id;
        public string name;
        public int released;
    }

    [System.Serializable]
    public class PlatformMeta {
        public PlatformData[] data;
    }
}