using OnRadio.BL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnRadio.BL.Interfaces
{
    public interface IMusicService
    {
        Task<List<RadioModel>> GetRadiosAsync();

        Task<List<StreamModel>> GetRadioStreamUrlAsync(string radioId);

        Task<SongModel> GetOnAir(string radioId);
    }
}