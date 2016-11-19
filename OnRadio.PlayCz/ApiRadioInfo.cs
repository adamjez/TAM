using Newtonsoft.Json;

namespace OnRadio.PlayCz
{
    public class ApiRadioInfo
    {
        [JsonProperty(PropertyName = "web1")]
        public string Web1 { get; set; }

        [JsonProperty(PropertyName = "web2")]
        public string Web2 { get; set; }

        [JsonProperty(PropertyName = "phone1")]
        public string Phone1 { get; set; }

        [JsonProperty(PropertyName = "phone2")]
        public string Phone2 { get; set; }

        [JsonProperty(PropertyName = "email1")]
        public string Email1 { get; set; }

        [JsonProperty(PropertyName = "email2")]
        public string Email2 { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "social")]
        public ApiRadioInfoSocial Social { get; set; }
    }
}