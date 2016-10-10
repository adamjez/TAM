using OnRadio.BL.Services;
using System;
using Windows.Media.Playback;

namespace OnRadio.App.ViewModels
{
    public class PlayerViewModel : LoadingViewModelBase, IDisposable
    {
        private readonly PlaybackService _service;
        private bool _disposed;

        public PlayerViewModel(PlaybackService service)
        {
            _service = service;
        }

        public void TogglePlayPause()
        {
            var player = _service.Player;
            switch (player.PlaybackSession.PlaybackState)
            {
                case MediaPlaybackState.Playing:
                    player.Pause();
                    break;

                case MediaPlaybackState.Paused:
                    player.Play();
                    break;
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
        }
    }
}