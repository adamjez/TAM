using System.Threading.Tasks;
using OnRadio.App.Services;

namespace OnRadio.BL.Services
{
    public interface IImageManager
    {
        Task<DownloadResult> DownloadAndSaveAsync(string uri, string fileName);

        Task DeleteAsync(string fileName);
    }
}