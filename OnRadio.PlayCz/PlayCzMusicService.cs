using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnRadio.PlayCz
{
    public class PlayCzMusicService : IMusicService
    {
        private const string baseUrl = "http://api.play.cz";

        public async Task<List<RadioItem>> GetRadiosAsync()
        {
            var radios = new List<RadioItem>();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(baseUrl + @"/json/getRadios/internet");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse<ApiRadioItem>>(responseContent);

                    foreach (var item in result.Data)
                    {
                        var radio = new RadioItem()
                        {
                            Id = item.Value.Shortcut,
                            Title = item.Key,
                            Description = item.Value.Description,
                            Url = item.Value.Weburl,
                            LogoUrl = item.Value.Logo,
                        };

                        radios.Add(radio);
                    }
                }
            }

            return radios;
        }

        public async Task<StreamItem> GetRadioStreamUrlAsync(RadioItem radio)
        {
            using (var client = new HttpClient())
            {
                // ToDo: process radio.Id with url encode
                var response = await client.GetAsync(baseUrl + @"/json/getStreamMobile/" + radio.Id);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    JObject result = JObject.Parse(responseContent);

                    var stream = result["data"]["stream"].ToObject<ApiStreamItem>();

                    return new StreamItem()
                    {
                        IsActive = stream.IsActive,
                        Listeners = stream.Listeners,
                        StreamUrl = stream.Pubpoint
                    };
                }
            }

            return null;
        }
    }
}