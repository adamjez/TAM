using OnRadio.BL.Models;
using System;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;

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
                AutoPlay = false,
                SystemMediaTransportControls =
                {
                    IsEnabled = true,
                    IsPreviousEnabled = false,
                    IsNextEnabled = false,
                    IsPauseEnabled = false,
                    IsStopEnabled = true,
                }
            };
        }

        /// <summary>
        /// This application only requires a single shared MediaPlayer
        /// that all pages have access to. The instance could have
        /// also been stored in Application.Resources or in an
        /// application defined data model.
        /// </summary>
        public MediaPlayer Player { get; private set; }

        public StreamModel Stream { get; set; }

        public void Play()
        {
            if (Stream == null)
                return;

            Player.Source = MediaSource.CreateFromUri(new Uri(Stream.StreamUrl));
            Player.AudioCategory = MediaPlayerAudioCategory.Media;
            Player.Play();
        }

        public void Stop()
        {
            Player.Pause();
            Player.Source = null;
        }

        public void SetMusicInformation(MusicInformation information)
        {
            if (information == null)
                return;

            // Get the updater.
            var updater = Player.SystemMediaTransportControls.DisplayUpdater;

            updater.ClearAll();

            // Music metadata.
            updater.Type = MediaPlaybackType.Music;

            updater.MusicProperties.Title = information.Title;
            updater.MusicProperties.Artist = information.Artist;
            updater.MusicProperties.AlbumTitle = information.Album;

            // Set the album art thumbnail.
            // RandomAccessStreamReference is defined in Windows.Storage.Streams
            if (!string.IsNullOrEmpty(information.ThumbnailUrl))
            {
                updater.Thumbnail =
                   RandomAccessStreamReference.CreateFromUri(new Uri(information.ThumbnailUrl));
            }
            // Update the system media transport controls.
            updater.Update();
        }

        public void ClearMusicInformation()
        {
            var updater = Player.SystemMediaTransportControls.DisplayUpdater;
            updater.ClearAll();
            updater.Update();
        }

        public void AddMediaController(IMediaNotify controller)
        {
            Player.PlaybackSession.PlaybackStateChanged += controller.Update;
        }
    }
}