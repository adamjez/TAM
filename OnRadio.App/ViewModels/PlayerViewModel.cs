using OnRadio.BL.Services;
using Windows.Media.Playback;
using Bezysoftware.Navigation;
using Bezysoftware.Navigation.Activation;
using OnRadio.BL.Models;

namespace OnRadio.App.ViewModels
{
    public class PlayerViewModel : LoadingViewModelBase, IActivate<RadioModel>
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


        public void Activate(NavigationType navigationType, RadioModel data)
        {
            Radio = data;
        }
    }
}