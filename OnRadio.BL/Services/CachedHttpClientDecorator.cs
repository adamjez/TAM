using System.Collections.Generic;
using System.Threading.Tasks;
using OnRadio.BL.Interfaces;

namespace OnRadio.BL.Services
{
    public class CachedHttpClientDecorator : IHttpClient
    {
        private readonly IHttpClient _httpClient;
        private static readonly Dictionary<string, string> Cache = new Dictionary<string, string>();
        public CachedHttpClientDecorator(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetStringAsync(string url)
        {
            string result;
            if (Cache.TryGetValue(url, out result))
            {
                return result;
            }

            result = await _httpClient.GetStringAsync(url);

            //Cache[url] = result;

            return result;
        }
    }
}