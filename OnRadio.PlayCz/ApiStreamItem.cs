using Newtonsoft.Json;
using OnRadio.PlayCz.Utilities;

namespace OnRadio.PlayCz
{
    public class ApiStreamItem
    {
        [JsonProperty(PropertyName = "pubpoint")]
        public string Pubpoint { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "listeners")]
        public int Listeners { get; set; }
    }
}