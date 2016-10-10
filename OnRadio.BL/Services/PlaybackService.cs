using OnRadio.BL.Interfaces;
using System;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace OnRadio.BL.Services
{
    /// <summary>
    /// The central authority on playback in the application
    /// providing access to the player and active playlist.
    /// </summary>
    public class PlaybackService
    {
        /// <summary>
        /// The data model of the active playlist. An application might
        /// have a database of items representing a user's media library,
        /// but here we use a simple list loaded from a JSON asset.
        /// </summary>

        public PlaybackService()
        {
            // Create the player instance
            Player = new MediaPlayer
            {
                AutoPlay = false
            };
        }

        /// <summary>
        /// This application only requires a single shared MediaPlayer
        /// that all pages have access to. The instance could have
        /// also been stored in Application.Resources or in an
        /// application defined data model.
        /// </summary>
        public MediaPlayer Player { get; private set; }

        public void PlayFromUrl(string url)
        {
            Player.Source = MediaSource.CreateFromUri(new Uri(url));
            Player.Play();
        }
    }
}