using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace speedrun
{
    [System.Serializable]
    public class AuthenticationData
    {
        public string id;
        public Names names;
        public string weblink;
        [JsonProperty(PropertyName = "name-style")]
        public NameStyle name_style;
        public string role;
        public string signup;
        public Location location;
        public ExternalWebResource twitch;
        public ExternalWebResource hitbox;
        public ExternalWebResource youtube;
        public ExternalWebResource twitter;
        public ExternalWebResource speedrunslive;

        public Links[] links;
    }

    [System.Serializable]
    public class Links
    {
        public string rel;
        public string uri;
    }

    [System.Serializable]
    public class ExternalWebResource
    {
        public string uri;
    }
    
    [System.Serializable]
    public class Location
    {
        public Country country;
        public Country region;
    }

    [System.Serializable]
    public class Country
    {
        public string code;
        public Names names;
    }

    [System.Serializable]
    public class ColorGradientStop
    {
        public string light;
        public string dark;
    }

    [System.Serializable]
    public class NameStyle
    {
        public string style;
        [JsonProperty(PropertyName = "color-from")]
        public ColorGradientStop color_from;
        [JsonProperty(PropertyName = "color-to")]
        public ColorGradientStop color_to;
    }

    [System.Serializable]
    public class Names
    {
        public string international;
        public string japanese;
        public string twitch;
    }


    [System.Serializable]
    public class AuthenticationModel
    {
        public AuthenticationData data;
    }
}