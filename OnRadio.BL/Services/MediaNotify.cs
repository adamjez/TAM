using System;
using Windows.Media.Playback;
using Windows.System.Threading;

namespace OnRadio.BL.Services
{
    public class MediaNotify : IMediaNotify
    {
        private const int Period = 10;
        private ThreadPoolTimer _threadPoolTimer;

        public event EventHandler MediaUpdated;
        public bool Enabled { get; set; }

        public MediaNotify(PlaybackService playbackService)
        {
            playbackService.AddMediaController(this);
        }

        public void Update(MediaPlaybackSession sender, object args)
        {
            if (!Enabled)
            {
                return;
            }

            switch (sender.PlaybackState)
            {
                case MediaPlaybackState.Playing:
                    InitializeBackgroundWork();
                    break;
                case MediaPlaybackState.None:
                case MediaPlaybackState.Paused:
                    CancelBackgroundWork();
                    break;
            }
        }


        private void CancelBackgroundWork()
        {
            _threadPoolTimer?.Cancel();
        }

        private void InitializeBackgroundWork()
        {
            CancelBackgroundWork();

            _threadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(source =>
            {
                MediaUpdated?.Invoke(this, EventArgs.Empty);

            }, TimeSpan.FromSeconds(Period));
        }
    }
}