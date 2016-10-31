using System.Linq;
using System.Threading.Tasks;
using OnRadio.BL.Services;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using OnRadio.App.Views;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;

namespace OnRadio.App.ViewModels
{
    public class PlayerViewModel : LoadingViewModelBase
    {
        private readonly IMusicService _musicService;
        private readonly PlaybackService _playbackService;
        private INavigationService _navigationService;
        private RelayCommand _openRadioListCommand;
        private RelayCommand _togglePlayPauseCommand;

        private RadioModel _radio;
        private MusicInformation _information;

        public RelayCommand OpenRadioListCommand =>
           _openRadioListCommand ?? (_openRadioListCommand = new RelayCommand(OpenRadioList));

        public RelayCommand TogglePlayPauseCommand =>
           _togglePlayPauseCommand ?? (_togglePlayPauseCommand = new RelayCommand(TogglePlayPause));


        public MusicInformation Information
        {
            get { return _information; }
            private set { Set(ref _information, value); }
        }

        public PlayerViewModel(IMusicService musicService, PlaybackService playbackService, INavigationService navigationService)
        {
            _musicService = musicService;
            _playbackService = playbackService;
            _navigationService = navigationService;
        }

        private void OpenRadioList()
        {
            _navigationService.NavigateTo(nameof(RadioList));
        }

        public RadioModel Radio
        {
            get { return _radio; }
            set { Set(ref _radio, value); }
        }

        public void TogglePlayPause()
        {
            var player = _playbackService.Player;
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

        protected override async Task LoadData()
        {
            _playbackService.Player.Pause();
            await LoadRadioAsync();
            RadioLoaded();
        }

        public async Task<bool> LoadRadioAsync()
        {
            if (Radio == null)
                return false;
            
            var streams = await _musicService.GetAllRadioStreamsAsync(Radio.Id);

            var selectedStream = streams.First();
            var selectedBitrate = selectedStream.Bitrates.First();
            var stream = await _musicService.GetRadioStreamAsync(Radio.Id, selectedStream.Format, selectedBitrate);

            _playbackService.Stream = stream;

            if (Radio.OnAir)
            {
                var song = await _musicService.GetOnAirAsync(Radio.Id);
                Information = song.CreateMusicInformation();

            }
            else
            {
                Information = Radio.CreateMusicInformation();
            }

            _playbackService.SetMusicInformation(Information);

            return true;
        }

        public void RadioLoaded()
        {
            _playbackService.Player.Play();
        }
    }
}