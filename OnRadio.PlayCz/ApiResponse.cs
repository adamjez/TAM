using Newtonsoft.Json;
using System.Collections.Generic;

namespace OnRadio.PlayCz
{
    public class ApiResponse<T>
    {
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }
    }
}