using OnRadio.App.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnRadio.App.Services
{
    public class PlayCzMusicService : IMusicService
    {
        public Task<List<RadioItem>> GetRadiosAsync()
        {
            var radios = new List<RadioItem>
            {
                new RadioItem() {Title = "Radio 1", Description = "Rock"},
                new RadioItem() {Title = "Kiss Morava", Description = "Pop"}
            };

            return Task.FromResult(radios);
        }
    }
}