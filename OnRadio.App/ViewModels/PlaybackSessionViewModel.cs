using System;
using Windows.Media.Playback;
using GalaSoft.MvvmLight;
using Microsoft.Toolkit.Uwp;


namespace OnRadio.App.ViewModels
{
    public class PlaybackSessionViewModel : ViewModelBase, IDisposable
    {
        bool disposed;
        readonly MediaPlaybackSession playbackSession;

        public MediaPlaybackState PlaybackState => playbackSession.PlaybackState;

        public PlaybackSessionViewModel(MediaPlaybackSession playbackSession)
        {
            this.playbackSession = playbackSession;

            playbackSession.PlaybackStateChanged += PlaybackSession_PlaybackStateChanged;
        }

        private async void PlaybackSession_PlaybackStateChanged(MediaPlaybackSession sender, object args)
        {
            if (disposed) return;

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                if (disposed) return;
                RaisePropertyChanged(nameof(PlaybackState));
            });
        }

        public void Dispose()
        {
            if (disposed)
                return;

            playbackSession.PlaybackStateChanged -= PlaybackSession_PlaybackStateChanged;

            disposed = true;
        }
    }
}