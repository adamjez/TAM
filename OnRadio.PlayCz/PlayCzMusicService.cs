using Newtonsoft.Json;
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

        public Task<string> GetRadioStreamUrlAsync(RadioItem radio)
        {
            return Task.FromResult("http://icecast3.play.cz/bonton-128.mp3");
        }
    }
}