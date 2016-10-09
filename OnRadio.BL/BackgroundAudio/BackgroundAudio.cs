using OnRadio.BL.Interfaces;
using System;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace OnRadio.BL.BackgroundAudio
{
    public class BackgroundAudio : IBackgroundAudio
    {
        private static readonly MediaPlayer MediaPlayer = new MediaPlayer();

        public void Play(string url)
        {
            MediaPlayer.Source = MediaSource.CreateFromUri(new Uri(url));

            MediaPlayer.Play();

            //MediaPlayer.Dispose();
        }
    }
}