using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using OnRadio.App.Views;
using OnRadio.BL.Interfaces;

namespace OnRadio.App.Services
{
    public class CortanaService
    {
        private readonly IMusicService _musicService;

        public CortanaService(IMusicService musicService)
        {
            _musicService = musicService;
        }

        /// <summary>
        /// Whenever a trip is modified, we trigger an update of the voice command Phrase list. This allows
        /// voice commands such as "Adventure Works Show trip to {destination} to be up to date with saved
        /// Trips.
        /// </summary>
        public async Task UpdateRadioPhraseList(string commandSetKey)
        {
            try
            {
                // Update the destination phrase list, so that Cortana voice commands can use destinations added by users.
                // When saving a trip, the UI navigates automatically back to this page, so the phrase list will be
                // updated automatically.
                VoiceCommandDefinition commandDefinitions;

                if (VoiceCommandDefinitionManager.InstalledCommandDefinitions.TryGetValue(commandSetKey, out commandDefinitions))
                {
                    List<string> destinations = new List<string>();
                    foreach (var radio in await _musicService.GetRadiosAsync())
                    {
                        destinations.Add(radio.Title);
                    }

                    await commandDefinitions.SetPhraseListAsync("radio", destinations);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Updating Phrase list for VCDs: " + ex);
            }
        }
    }
}