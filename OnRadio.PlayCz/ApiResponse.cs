using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnRadio.PlayCz
{
    public class ApiResponse<T>
    {
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
        /*
        [JsonProperty(PropertyName = "_lifetime")]
        public T Lifetime { get; set; }

        [JsonProperty(PropertyName = "_cachedAt")]
        public T CachedAt { get; set; }

        [JsonProperty(PropertyName = "_expireAt")]
        public T ExpireAt { get; set; }

        [JsonProperty(PropertyName = "_cacheID")]
        public T CacheID { get; set; }

        [JsonProperty(PropertyName = "_cacheUsed")]
        public T CacheUsed { get; set; }
        */
    }
}