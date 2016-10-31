using Newtonsoft.Json;

namespace OnRadio.PlayCz
{
    public class ApiStyleItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }
}