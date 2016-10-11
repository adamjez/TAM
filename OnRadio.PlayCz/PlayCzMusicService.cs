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
        private const string baseOnAirUrl = "http://onair.play.cz";

        public async Task<List<RadioModel>> GetRadiosAsync()
        {
            var radios = new List<RadioModel>();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(baseUrl + @"/json/getRadios/internet");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ApiResponse<ApiRadioItem>>(responseContent);

                    foreach (var item in result.Data)
                    {
                        var radio = new RadioModel()
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

        public async Task<SongModel> GetOnAir(string radioId)
        {
            using (var client = new HttpClient())
            {
                // ToDo: process radio.Id with url encode
                var response = await client.GetAsync(baseOnAirUrl + @"/json/" + radioId + ".json");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<ApiSongItem>(responseContent);

                    return new SongModel()
                    {
                        Title = result.Song,
                        Artist = result.Artist,
                        ThumbnailUrl = result.Img
                    };
                }
            }

            return SongModel.CreateUndefined();
        }

        public async Task<List<StreamModel>> GetRadioStreamUrlAsync(string radioId)
        {
            var streams = new List<StreamModel>();

            using (var client = new HttpClient())
            {
                // ToDo: process radio.Id with url encode
                var response = await client.GetAsync(baseUrl + @"/json/getStreamMobile/" + radioId);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    JObject result = JObject.Parse(responseContent);

                    var stream = result["data"]["stream"].ToObject<ApiStreamItem>();

                    streams.Add(new StreamModel()
                    {
                        IsActive = stream.IsActive,
                        Listeners = stream.Listeners,
                        StreamUrl = stream.Pubpoint,
                    });
                }
            }

            return streams;
        }
    }
}