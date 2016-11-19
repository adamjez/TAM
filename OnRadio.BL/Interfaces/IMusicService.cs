using OnRadio.BL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnRadio.BL.Interfaces
{
    public interface IMusicService
    {
        Task<List<RadioModel>> GetRadiosAsync();

        Task<StreamModel> GetRadioStreamAsync(string radioId, string format, int bitrate);

        Task<SongModel> GetOnAirAsync(string radioId);

        Task<List<StreamFormatModel>> GetAllRadioStreamsAsync(string radioId);

        Task<List<StyleModel>> GetStyles();

        Task<List<HistorySongModel>> GetOnAirHistoryAsync(string radioId);

        Task<RadioInfoModel> GetRadioInfo(string radioId);
    }
}