using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp;
using OnRadio.BL.Helpers;

namespace OnRadio.BL.Services
{
    public class LastRadiosStorage
    {
        private const string KeyRadiosHistory = "LastRadios";
        private const int LastRadioCapacity = 10;

        public async Task Add(string id)
        {
            var helper = new RoamingObjectStorageHelper();
            FIFOStack<string> lastRadios;

            if (await helper.FileExistsAsync(KeyRadiosHistory))
            {
                lastRadios = await helper.ReadFileAsync<FIFOStack<string>>(KeyRadiosHistory);
            }
            else
            {
                lastRadios = new FIFOStack<string> {Capacity = LastRadioCapacity };
            }

            lastRadios.Push(id);
            lastRadios.DebugPrint();

            await helper.SaveFileAsync(KeyRadiosHistory, lastRadios);
        }

        public async Task<IEnumerable<string>> Get()
        {
            var helper = new RoamingObjectStorageHelper();

            if (!await helper.FileExistsAsync(KeyRadiosHistory))
            {
                return new List<string>();
            }

            var stack = await helper.ReadFileAsync<FIFOStack<string>>(KeyRadiosHistory);

            return stack.Get();
        }
    }
}