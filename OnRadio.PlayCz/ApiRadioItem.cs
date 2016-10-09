using Newtonsoft.Json;
using System.Collections.Generic;

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
        public string Listeners { get; set; }

        [JsonProperty(PropertyName = "onair")]
        public string Onair { get; set; }

        [JsonProperty(PropertyName = "style")]
        public IList<string> Style { get; set; }

        [JsonProperty(PropertyName = "region")]
        public IList<string> Region { get; set; }
    }
}