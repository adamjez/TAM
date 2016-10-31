using System.Collections.Generic;
using Newtonsoft.Json;

namespace OnRadio.PlayCz
{
    public class ApiStreamFormatsItem
    {
        [JsonProperty(PropertyName = "streams")]
        public Dictionary<string, List<string>> Streams { get; set; }

        [JsonProperty(PropertyName = "shortcut")]
        public string Shortcut { get; set; }
    }
}