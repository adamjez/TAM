using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnRadio.BL.Services;
using Windows.Media.Playback;
using Windows.UI;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Views;
using OnRadio.App.Messages;
using OnRadio.App.Views;
using OnRadio.BL.Helpers;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;
using OnRadio.DAL;

namespace OnRadio.App.ViewModels
{
    public class PlayerViewModel : LoadingViewModelBase, IDisposable
    {
        private readonly IMusicService _musicService;
        private readonly PlaybackService _playbackService;
        private readonly INavigationService _navigationService;
        private readonly MediaNotify _mediaNotify;
        private RelayCommand _openRadioListCommand;
        private RelayCommand _togglePlayPauseCommand;
        private RelayCommand _navigateToPlayerCommand;
        private RelayCommand _toggleRadioIsFavoriteCommand;
        private RelayCommand _togglePlaybackQualityCommand;

        private RadioModel _radio;
        private MusicInformation _information;

        private bool _radioLoaded;

        public RelayCommand OpenRadioListCommand =>
           _openRadioListCommand ?? (_openRadioListCommand = new RelayCommand(OpenRadioList));

        public RelayCommand TogglePlayPauseCommand =>
           _togglePlayPauseCommand ?? (_togglePlayPauseCommand = new RelayCommand(TogglePlayPause));

        public RelayCommand TogglePlaybackQualityCommand =>
            _togglePlaybackQualityCommand ?? (_togglePlaybackQualityCommand = new RelayCommand(TogglePlaybackQuality));

        public RelayCommand NavigateToPlayerCommand =>
           _navigateToPlayerCommand ?? (_navigateToPlayerCommand = new RelayCommand(NavigateToPlayer));

        public RelayCommand ToggleRadioIsFavoriteCommand =>
           _toggleRadioIsFavoriteCommand ?? (_toggleRadioIsFavoriteCommand = new RelayCommand(ToggleRadioIsFavorite));

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

                if(value == null)
                    _playbackService.ClearMusicInformation();
                else
                    _playbackService.SetMusicInformation(Information);
            }
        }

        public StreamModel Stream => _playbackService.Stream;

        public RadioModel Radio
        {
            get { return _radio; }
            set { Set(ref _radio, value); }
        }

        public PlaybackSessionViewModel PlaybackSession { get; private set; }


        public PlayerViewModel(IMusicService musicService, PlaybackService playbackService, INavigationService navigationService,
            MediaNotify mediaNotify)
        {
            _musicService = musicService;
            _playbackService = playbackService;
            _navigationService = navigationService;
            _mediaNotify = mediaNotify;

            PlaybackSession = new PlaybackSessionViewModel(playbackService.Player.PlaybackSession);
            _mediaNotify.MediaUpdated += BackgroundMediaUpdate;
        }

        public Brush BackgroundBrush => new SolidColorBrush { Color = Color.FromArgb(100, 0, 0, 0) };

        private void OpenRadioList()
        {
            _navigationService.NavigateTo(nameof(RadioList), "refresh");
        }

        public void TogglePlayPause()
        {
            var player = _playbackService.Player;
            switch (player.PlaybackSession.PlaybackState)
            {
                case MediaPlaybackState.Playing:
                    _playbackService.Stop();
                    Information = Radio.CreateMusicInformation();
                    break;

                case MediaPlaybackState.Paused:
                case MediaPlaybackState.None:
                    _playbackService.Play();
                    _playbackService.SetMusicInformation(Information);
                    break;
            }
        }

        public void NavigateToPlayer()
        {
            
            _navigationService.NavigateTo(nameof(Player), true);
        }

        public override void Initialize(object argument)
        {
            var radio = argument as RadioModel;

            if (radio != null)
            {
                NavigatedViaModel(radio);
            }
            else if (argument is string)
            {
                // Normal navigation from radio list
                NavigatedViaId((string)argument);
            }
            else if (argument is bool)
            {
                // Page navigated back -> data should be already loaded
            }
            else
            {
                throw new ArgumentException("Page was accessed without proper argument");
            }

            base.Initialize(argument);
        }

        private void NavigatedViaModel(RadioModel radio)
        {
            // Normal navigation from radio list
            if (Loaded && Radio?.Id != radio.Id)
            {
                // Loading different radio from radio list
                Clear();
            }

            Radio = radio;
            _radioLoaded = true;
        }

        private void NavigatedViaId(string argument)
        {
            if (Loaded && Radio?.Id != argument)
            {
                // Loading different radio 
                Clear();
            }
            // Page navigated from Secondary Tile
            Radio = new RadioModel() {Id = argument};
        }

        private void Clear()
        {
            Information = null;
            Radio = null;
            _radioLoaded = false;
            Loaded = false;
        }

        protected override async Task LoadData()
        {
            _playbackService.Stop();

            
            if (!_radioLoaded)
            {
                await LoadRadioAsync();
            }

            _mediaNotify.Enabled = Radio.OnAir;

            await LoadStreamAsync();

            _playbackService.Play();

            await LoadInfoAsync();
        }

        private async Task LoadRadioAsync()
        {
            Radio = (await _musicService.GetRadiosAsync())
                .FirstOrDefault(radio => String.Compare(radio.Id, Radio.Id, StringComparison.OrdinalIgnoreCase) == 0);

            // ToDo: handle this a show this in proper way
            if (Radio == null)
                throw new ArgumentException("Radio doesn't exists");

            Radio.IsFavorite = LocalDatabaseStorage.IsFavorite(Radio.Id);

            _radioLoaded = true;
        }

        public async Task LoadStreamAsync()
        {
            var streams = await _musicService.GetAllRadioStreamsAsync(Radio.Id);

            Radio.Streams = new List<StreamModel>();

            // Select Low Quality stream
            var selectedStream = streams.OrderBy(x => x.Bitrates.Min()).First();
            var selectedBitrate = selectedStream.Bitrates.Min();
            var stream = await _musicService.GetRadioStreamAsync(Radio.Id, selectedStream.Format, selectedBitrate);
            stream.Quality = StreamModel.StreamQuality.Low;
            Radio.Streams.Add(stream);

            // Select High Quality stream
            selectedStream = streams.OrderBy(x => x.Bitrates.Max()).First();
            selectedBitrate = selectedStream.Bitrates.Max();
            stream = await _musicService.GetRadioStreamAsync(Radio.Id, selectedStream.Format, selectedBitrate);
            stream.Quality = StreamModel.StreamQuality.High;
            Radio.Streams.Add(stream);

            _playbackService.Stream = stream;
            RaisePropertyChanged(() => Stream);
        }

        public async Task LoadInfoAsync()
        {
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

        public void TogglePlaybackQuality()
        {
            if (Radio == null)
                return;

            StreamModel newQualityStream = null;
            switch (Stream.Quality)
            {
                case StreamModel.StreamQuality.High:
                    newQualityStream = Radio.Streams.FirstOrDefault(x => x.Quality == StreamModel.StreamQuality.Low);
                    break;
                case StreamModel.StreamQuality.Low:
                    newQualityStream = Radio.Streams.FirstOrDefault(x => x.Quality == StreamModel.StreamQuality.High);
                    break;
                default:
                    throw new ArgumentException("Unhandled stream quality value");
            }

            if (newQualityStream == null)
                return;
            _playbackService.Stream = newQualityStream;
            if (PlaybackSession.PlaybackState != MediaPlaybackState.Paused)
            {
                _playbackService.Stop();
                _playbackService.Play();
            }
                
            RaisePropertyChanged(()=>Stream);
        }

        public void ToggleRadioIsFavorite()
        {
            if(Radio == null) 
                return;

            if (Radio.IsFavorite)
            {
                LocalDatabaseStorage.DeleteFavorite(Radio.Id);
            }
            else
            {
                LocalDatabaseStorage.InsertFavorite(Radio.Id);
            }
            Radio.IsFavorite = !Radio.IsFavorite;
            MessengerInstance.Send(new FavoriteChangeMessage(this, Radio.Id, Radio.IsFavorite));
            RaisePropertyChanged(()=>Radio);
        }

        public void Dispose()
        {
            _mediaNotify.MediaUpdated -= BackgroundMediaUpdate;
            PlaybackSession.Dispose();
        }
    }
}