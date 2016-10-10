using OnRadio.BL.Interfaces;
using System;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace OnRadio.BL.BackgroundAudio
{
    public class BackgroundAudio : IBackgroundAudio, IDisposable
    {
        public BackgroundAudio()
        {
            // Create the player instance
            MediaPlayer = new MediaPlayer { AutoPlay = false };
        }

        public MediaPlayer MediaPlayer { get; private set; }

        public void Play(string url)
        {
            MediaPlayer.Source = MediaSource.CreateFromUri(new Uri(url));

            MediaPlayer.Play();
        }

        public void Dispose()
        {
            MediaPlayer.Dispose();
        }
    }
}