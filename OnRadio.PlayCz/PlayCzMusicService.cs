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
using OnRadio.DAL;
using System.Diagnostics;

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
            string dataDeserialized;
            var radios = new List<RadioModel>();

            if (LocalDatabaseStorage.IsCached(DateTime.Now, CachedDataType.getRadios))
            {
                dataDeserialized = LocalDatabaseStorage.getDataFromCache(CachedDataType.getRadios);
            }

            else
            {
                var response = await _httpClient.GetStringAsync(BaseUrl + RadiosPath);
                dataDeserialized = JObject.Parse(response)["data"].ToString();
                try
                {
                    var expirationDate = JObject.Parse(response)["_expireAt"].ToString();
                    //cacheId = parseResponsePartial["_cacheId"].ToString();  // Not used for now.

                    LocalDatabaseStorage.InsertOrUpdateCachedData(CachedDataType.getRadios, Convert.ToDateTime(expirationDate), dataDeserialized);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("[<RadioModel> GetRadiosAsync] Error during deserialization of element '_expireAt' in HTTP response: " + e);
                    Debug.WriteLine("Caching is not performed.");
                }
            }

            var result = JsonConvert.DeserializeObject<Dictionary<string, ApiRadioItem>>(dataDeserialized);

            foreach (var item in result)
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
            string dataDeserialized;

            if (LocalDatabaseStorage.IsCached(DateTime.Now, CachedDataType.getRadioStream))
            {
                dataDeserialized = LocalDatabaseStorage.getDataFromCache(CachedDataType.getRadioStream);
            }

            else
            {

                var url = BaseUrl + @"/json/getStreamMobile/" + WebUtility.UrlEncode(radioId) + '/' +
                      WebUtility.UrlEncode(format) + '/' + bitrate;
                var response = await _httpClient.GetStringAsync(url);
                dataDeserialized = JObject.Parse(response)["data"].ToString();

                try
                {
                    var expirationDate = JObject.Parse(response)["_expireAt"].ToString();
                    LocalDatabaseStorage.InsertOrUpdateCachedData(CachedDataType.getRadioStream, Convert.ToDateTime(expirationDate), dataDeserialized);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("[<StyleModel> GetStyles] Error during deserialization of element '_expireAt' in HTTP response: " + e);
                    Debug.WriteLine("Caching is not performed.");
                }
            }

            JObject result = JObject.Parse(dataDeserialized);

            var stream = result["stream"].ToObject<ApiStreamItem>();

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
            string dataDeserialized;

            if (LocalDatabaseStorage.IsCached(DateTime.Now, CachedDataType.getAllRadioStreams))
            {
                dataDeserialized = LocalDatabaseStorage.getDataFromCache(CachedDataType.getAllRadioStreams);
            }

            else
            {
                var response = await _httpClient.GetStringAsync(BaseUrl + @"/json/getAllStreamsMobile?shortcut=" + WebUtility.UrlEncode(radioId));
                dataDeserialized = JObject.Parse(response)["data"].ToString();

                try
                {
                    var expirationDate = JObject.Parse(response)["_expireAt"].ToString();
                    LocalDatabaseStorage.InsertOrUpdateCachedData(CachedDataType.getAllRadioStreams, Convert.ToDateTime(expirationDate), dataDeserialized);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("[<StyleModel> GetStyles] Error during deserialization of element '_expireAt' in HTTP response: " + e);
                    Debug.WriteLine("Caching is not performed.");
                }
            }
            var result = JsonConvert.DeserializeObject<ApiStreamFormatsItem>(dataDeserialized);

            return result.Streams
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
            string dataDeserialized;

            if (LocalDatabaseStorage.IsCached(DateTime.Now, CachedDataType.getStyles))
            {
                dataDeserialized = LocalDatabaseStorage.getDataFromCache(CachedDataType.getStyles);
            }

            else
            {
                var response = await _httpClient.GetStringAsync(BaseUrl + @"/json/getStyles");
                dataDeserialized = JObject.Parse(response)["data"].ToString();
                try
                {
                    var expirationDate = JObject.Parse(response)["_expireAt"].ToString();
                    LocalDatabaseStorage.InsertOrUpdateCachedData(CachedDataType.getStyles, Convert.ToDateTime(expirationDate), dataDeserialized);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("[<StyleModel> GetStyles] Error during deserialization of element '_expireAt' in HTTP response: " + e);
                    Debug.WriteLine("Caching is not performed.");
                }
            }

            var result = JsonConvert.DeserializeObject<List<ApiStyleItem>>(dataDeserialized);

            return result
                .Select(item => new StyleModel
                {
                    Title = item.Title,
                    Description = item.Description,
                    Count = item.Count
                })
                .ToList();
        }
    }
}