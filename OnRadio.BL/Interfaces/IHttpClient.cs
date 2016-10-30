using System.Net.Http;
using System.Threading.Tasks;

namespace OnRadio.BL.Interfaces
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string url);
    }
}