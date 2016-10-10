using OnRadio.BL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnRadio.BL.Interfaces
{
    public interface IMusicService
    {
        Task<List<RadioItem>> GetRadiosAsync();

        Task<StreamItem> GetRadioStreamUrlAsync(RadioItem radio);
    }
}