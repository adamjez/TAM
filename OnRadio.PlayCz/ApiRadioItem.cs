using Newtonsoft.Json;
using System.Collections.Generic;
using OnRadio.PlayCz.Utilities;

namespace OnRadio.PlayCz
{
    public class ApiRadioItem
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "weburl")]
        public string Weburl { get; set; }

        [JsonProperty(PropertyName = "shortcut")]
        public string Shortcut { get; set; }

        [JsonProperty(PropertyName = "logo")]
        public string Logo { get; set; }

        [JsonProperty(PropertyName = "listeners")]
        public int Listeners { get; set; }

        //[JsonConverter(typeof(BoolConverter))]
        [JsonProperty(PropertyName = "onair")]
        public bool Onair { get; set; }

        [JsonProperty(PropertyName = "style")]
        public IList<string> Style { get; set; }

        [JsonProperty(PropertyName = "region")]
        public IList<string> Region { get; set; }
    }
}