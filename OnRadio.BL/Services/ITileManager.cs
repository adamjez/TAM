using System.Threading.Tasks;
using OnRadio.BL.Models;

namespace OnRadio.BL.Services
{
    public interface ITileManager
    {
        Task<bool> CreateTileAsync(RadioModel radio);
        Task<bool> RemoveTileAsync(RadioModel radio);
        bool Exists(RadioModel radio);
    }
}