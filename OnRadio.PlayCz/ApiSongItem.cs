using System;
using Newtonsoft.Json;

namespace OnRadio.PlayCz
{
    public class ApiSongItem
    {
        [JsonProperty(PropertyName = "song")]
        public string Song { get; set; }

        [JsonProperty(PropertyName = "artist")]
        public string Artist { get; set; }

        [JsonProperty(PropertyName = "img")]
        public string Img { get; set; }

        [JsonProperty(PropertyName = "itunes_url")]
        public string ItunesUrl { get; set; }

        [JsonProperty(PropertyName = "itunes_preview")]
        public string ItunesPreview { get; set; }

        [JsonProperty(PropertyName = "itunes_price")]
        public string ItunesPrice { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }
    }
}