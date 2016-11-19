using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnRadio.PlayCz
{
    public class PlayCzMusicService : IMusicService
    {
        private readonly IHttpClient _httpClient;
        private const string BaseUrl = "http://api.play.cz";
        private const string BaseOnAirUrl = "http://onair.play.cz";

        private const string RadiosPath = @"/json/getRadios/internet";


        public PlayCzMusicService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="HttpRequestException">When api endpoint isn't reachable.</exception>
        /// <returns></returns>
        public async Task<List<RadioModel>> GetRadiosAsync()
        {
            var radios = new List<RadioModel>();

            var response = await _httpClient.GetStringAsync(BaseUrl + RadiosPath);
            var result = JsonConvert.DeserializeObject<ApiResponse<Dictionary<string, ApiRadioItem>>>(response);

            foreach (var item in result.Data)
            {
                var radio = new RadioModel()
                {
                    Id = item.Value.Shortcut,
                    Title = item.Value.Title,
                    Description = item.Value.Description,
                    Url = item.Value.Weburl,
                    LogoUrl = item.Value.Logo,
                    Listenters = item.Value.Listeners,
                    OnAir = item.Value.Onair
                };

                radios.Add(radio);
            }


            return radios;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<SongModel> GetOnAirAsync(string radioId)
        {
            try
            {
                var response =
                    await _httpClient.GetStringAsync(BaseOnAirUrl + @"/json/" + WebUtility.UrlEncode(radioId) + ".json");

                var result = JsonConvert.DeserializeObject<ApiSongItem>(response);

                return new SongModel()
                {
                    Title = result.Song,
                    Artist = result.Artist,
                    ThumbnailUrl = result.Img
                };
            }
            catch (HttpRequestException)
            {
                return SongModel.CreateUndefined();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<HistorySongModel>> GetOnAirHistoryAsync(string radioId)
        {
            try
            {
                var response =
                    await _httpClient.GetStringAsync(BaseOnAirUrl + @"/json/" + WebUtility.UrlEncode(radioId) + "-history.json");

                var result = JsonConvert.DeserializeObject<List<ApiSongItem>>(response);

                return result.Select(item => new HistorySongModel()
                {
                    Title = item.Song,
                    Artist = item.Artist,
                    ThumbnailUrl = item.Img,
                    PlayedAt = item.Date
                }).ToList();
            }
            catch (HttpRequestException)
            {
                return new List<HistorySongModel>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="HttpRequestException">When api endpoint isn't reachable.</exception>
        /// <returns></returns>
        public async Task<StreamModel> GetRadioStreamAsync(string radioId, string format, int bitrate)
        {
            var url = BaseUrl + @"/json/getStreamMobile/" + WebUtility.UrlEncode(radioId) + '/' +
                      WebUtility.UrlEncode(format) + '/' + bitrate;
            var response = await _httpClient.GetStringAsync(url);

            JObject result = JObject.Parse(response);

            var stream = result["data"]["stream"].ToObject<ApiStreamItem>();

            return new StreamModel()
            {
                IsActive = stream.IsActive,
                Listeners = stream.Listeners,
                StreamUrl = stream.Pubpoint,
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="HttpRequestException">When api endpoint isn't reachable.</exception>
        /// <returns></returns>
        public async Task<List<StreamFormatModel>> GetAllRadioStreamsAsync(string radioId)
        {
            var response = await _httpClient.GetStringAsync(BaseUrl + @"/json/getAllStreamsMobile?shortcut=" + WebUtility.UrlEncode(radioId));

            var result = JsonConvert.DeserializeObject<ApiResponse<ApiStreamFormatsItem>>(response);

            return result.Data.Streams
                .Select(item => new StreamFormatModel
                {
                    Format = item.Key,
                    Bitrates = item.Value.Select(int.Parse).ToList()
                })
                .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="HttpRequestException">When api endpoint isn't reachable.</exception>
        /// <returns></returns>
        public async Task<List<StyleModel>> GetStyles()
        {
            var response = await _httpClient.GetStringAsync(BaseUrl + @"/json/getStyles");

            var result = JsonConvert.DeserializeObject<ApiResponse<List<ApiStyleItem>>>(response);

            return result.Data
                .Select(item => new StyleModel
                {
                    Title = item.Title,
                    Description = item.Description,
                    Count = item.Count
                })
                .ToList();
        }

        public async Task<RadioInfoModel> GetRadioInfo(string radioId)
        {
            var response =
                await _httpClient.GetStringAsync(BaseUrl + @"/json/getRadioInfo/" + WebUtility.UrlEncode(radioId));

            JObject result = JObject.Parse(response);

            var info = result["data"]["radio_info"].ToObject<ApiRadioInfo>();

            return new RadioInfoModel()
            {
                Id = radioId,
                Web1 = info.Web1,
                Web2 = info.Web2,
                Email1 = info.Email1,
                Email2 = info.Email2,
                Phone1 = info.Phone1,
                Phone2 = info.Phone2,
                Address = info.Address,
                Facebook = info.Social.Facebook,
                Gplus = info.Social.Gplus,
                Twitter = info.Social.Twitter,
                Youtube = info.Social.Youtube
            };
        }
    }
}