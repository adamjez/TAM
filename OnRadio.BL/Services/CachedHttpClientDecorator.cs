using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnRadio.BL.Interfaces;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json.Linq;
using OnRadio.DAL;

namespace OnRadio.BL.Services
{
    public class CachedHttpClientDecorator : IHttpClient
    {
        private readonly IHttpClient _httpClient;
        public CachedHttpClientDecorator(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetStringAsync(string url)
        {
            var cache = LocalDatabaseStorage.GetDataFromCache(url);
            if (cache != null && DateTime.UtcNow < Convert.ToDateTime(cache.ExpireAt))
            {
                return cache.Data;
            }

            var result = await _httpClient.GetStringAsync(url);

            try
            {
                var lifetime = JToken.Parse(result).Value<int?>("_lifetime");

                if (lifetime != null)
                {
                    var expirationDate = DateTime.UtcNow.AddSeconds((int)lifetime);

                    LocalDatabaseStorage.InsertOrUpdateCachedData(url, Convert.ToDateTime(expirationDate), result);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error during deserialization of element '_expireAt' in HTTP response: " + e);
                Debug.WriteLine("Caching is not performed.");
           }

            return result;
        }
    }
}