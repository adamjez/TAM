using System.Collections.Generic;
using System.Threading.Tasks;
using OnRadio.BL.Interfaces;

namespace OnRadio.BL.Services
{
    public class CachedHttpClientDecorator : IHttpClient
    {
        private readonly IHttpClient _httpClient;
        private readonly Dictionary<string, string> _cache;
        public CachedHttpClientDecorator(IHttpClient httpClient)
        {
            _httpClient = httpClient;
            _cache = new Dictionary<string, string>();
        }

        public async Task<string> GetStringAsync(string url)
        {
            string result;
            if (_cache.TryGetValue(url, out result))
            {
                return result;
            }

            result = await _httpClient.GetStringAsync(url);

            _cache[url] = result;

            return result;
        }
    }
}