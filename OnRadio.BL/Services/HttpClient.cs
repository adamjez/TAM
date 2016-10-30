using System;
using System.Threading.Tasks;
using OnRadio.BL.Interfaces;

namespace OnRadio.BL.Services
{
    public class HttpClient : IHttpClient
    {
        public async Task<String> GetStringAsync(string url)
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                return await httpClient.GetStringAsync(url);
            }
        }
    }
}