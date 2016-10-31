using OnRadio.BL.Services;
using Windows.Media.Playback;
using OnRadio.BL.Models;

namespace OnRadio.App.ViewModels
{
    public class PlayerViewModel : LoadingViewModelBase
    {
        private readonly PlaybackService _service;
        private RadioModel _radio;

        public PlayerViewModel(PlaybackService service)
        {
            _service = service;
        }

        public RadioModel Radio
        {
            get { return _radio; }
            set { Set(ref _radio, value); }
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

        public override void Initialize(object argument)
        {
            var radio = argument as RadioModel;

            if (radio != null)
            {
                Radio = radio;
            }

            base.Initialize(argument);
        }
    }
}