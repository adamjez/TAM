using Newtonsoft.Json;

namespace OnRadio.PlayCz
{
    public class ApiRadioInfoSocial
    {
        [JsonProperty(PropertyName = "facebook")]
        public string Facebook { get; set; }

        [JsonProperty(PropertyName = "twitter")]
        public string Twitter { get; set; }

        [JsonProperty(PropertyName = "gplus")]
        public string Gplus { get; set; }

        [JsonProperty(PropertyName = "youtube")]
        public string Youtube { get; set; }
    }
}