using System;
using System.Linq;
using System.Threading.Tasks;
using OnRadio.BL.Services;
using Windows.Media.Playback;
using Windows.UI;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using OnRadio.App.Views;
using OnRadio.BL.Helpers;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;

namespace OnRadio.App.ViewModels
{
    public class PlayerViewModel : LoadingViewModelBase
    {
        private readonly IMusicService _musicService;
        private readonly PlaybackService _playbackService;
        private readonly INavigationService _navigationService;
        private readonly MediaUpdater _mediaUpdater;
        private RelayCommand _openRadioListCommand;
        private RelayCommand _togglePlayPauseCommand;

        private RadioModel _radio;
        private MusicInformation _information;

        private bool _radioLoaded;


        public RelayCommand OpenRadioListCommand =>
           _openRadioListCommand ?? (_openRadioListCommand = new RelayCommand(OpenRadioList));

        public RelayCommand TogglePlayPauseCommand =>
           _togglePlayPauseCommand ?? (_togglePlayPauseCommand = new RelayCommand(TogglePlayPause));

        private bool _isPlaying;

        public bool IsPlaying
        {
            get { return _isPlaying; }
            private set
            {
                Set(ref _isPlaying, value);
            }
        }

        public MusicInformation Information
        {
            get { return _information; }
            set
            {
                if (Information != null && ObjectHelper.PublicInstancePropertiesEqual(Information, value))
                {
                    return;
                }

                Set(ref _information, value);
                _playbackService?.SetMusicInformation(Information);
            }
        }

        public PlayerViewModel(IMusicService musicService, PlaybackService playbackService, INavigationService navigationService,
            MediaUpdater mediaUpdater)
        {
            _musicService = musicService;
            _playbackService = playbackService;
            _navigationService = navigationService;
            _mediaUpdater = mediaUpdater;
        }

        public Brush BackgroundBrush => new SolidColorBrush {Color = Color.FromArgb(100, 0, 0, 0)};

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
                    IsPlaying = false;
                    break;

                case MediaPlaybackState.Paused:
                    player.Play();
                    IsPlaying = true;
                    break;
            }
        }

        public override void Initialize(object argument)
        {
            var radio = argument as RadioModel;

            if (radio != null)
            {
                Radio = radio;
                _radioLoaded = true;
            }
            else if (argument is string)
            {
                Radio = new RadioModel() {Id = (string)argument};;
            }
            else
            {
                throw new ArgumentException("Page was accessed without proper argument");
            }

            base.Initialize(argument);
        }


        protected override async Task LoadData()
        {
            _playbackService.Player.Pause();
            IsPlaying = false;

            if (!_radioLoaded)
            {
                Radio = (await _musicService.GetRadiosAsync())
                    .FirstOrDefault(radio => radio.Id == Radio.Id);

                // ToDo: handle this a show this in proper way
                if(Radio == null)
                    throw new ArgumentException("Radio doesn't exists");
            }

            _mediaUpdater.MediaUpdated += BackgroundMediaUpdate;
            _mediaUpdater.Enabled = Radio.OnAir;

            await LoadStreamAndInfoAsync();
            _playbackService.Player.Play();
            IsPlaying = true;
        }

        public async Task LoadStreamAndInfoAsync()
        {
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
        }

        public async void BackgroundMediaUpdate(object sender, EventArgs args)
        {
            if (Radio == null)
                return;

            var song = await _musicService.GetOnAirAsync(Radio.Id);

            await DispatcherHelper.RunAsync(() =>
            {
                Information = song.CreateMusicInformation();
            });
        }
    }
}