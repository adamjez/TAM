using OnRadio.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnRadio.App.Services
{
    public interface IMusicService
    {
        Task<List<RadioItem>> GetRadiosAsync();
    }
}