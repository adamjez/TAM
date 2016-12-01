using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OnRadio.BL.Services;
using Windows.Media.Playback;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using OnRadio.App.Messages;
using OnRadio.App.Views;
using OnRadio.BL.Helpers;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;
using OnRadio.DAL;
using Microsoft.Toolkit.Uwp;
using OnRadio.App.Commands;
using OnRadio.App.Exceptions;
using DispatcherHelper = Microsoft.Toolkit.Uwp.DispatcherHelper;

namespace OnRadio.App.ViewModels
{
    public class PlayerViewModel : LoadingViewModelBase, IDisposable
    {
        private const string _roamingQualityKey = "streamquality";

        private readonly IMusicService _musicService;
        private readonly PlaybackService _playbackService;
        private readonly ITileManager _tileManager;
        private readonly INavigationService _navigationService;
        private readonly MediaNotify _mediaNotify;
        private readonly LastRadiosStorage _lastRadiosStorage;
        private RelayCommand _openRadioListCommand;
        private RelayCommand _togglePlayPauseCommand;
        private RelayCommand _navigateToPlayerCommand;
        private RelayCommand _toggleTimerCommand;
        private RelayCommand _runTimerCommand;
        private RelayCommand _closeTimerCommand;

        private RadioModel _radio;
        private RadioInfoModel _radioInfo;
        private MusicInformation _information;
        private List<HistorySongModel> _history;

        private int _selectedPivot;

        private bool _radioLoaded;
        private bool _isHistoryLoading;
        private int _timerHours;
        private int _timerMinutes;
        private Timer _stopPlaybackTimer;
        private StreamModel.StreamQuality _selectedStreamQuality;
        private bool _isTimerOpened;

        public RelayCommand OpenRadioListCommand =>
           _openRadioListCommand ?? (_openRadioListCommand = new RelayCommand(OpenRadioList));

        public RelayCommand TogglePlayPauseCommand =>
           _togglePlayPauseCommand ?? (_togglePlayPauseCommand = new RelayCommand(TogglePlayPause));

        public RelayCommand NavigateToPlayerCommand =>
           _navigateToPlayerCommand ?? (_navigateToPlayerCommand = new RelayCommand(NavigateToPlayer));

        public RelayCommand ToggleTimerCommand =>
            _toggleTimerCommand ?? (_toggleTimerCommand = new RelayCommand(ToggleTimer));

        public RelayCommand RunTimerCommand =>
            _runTimerCommand ?? (_runTimerCommand = new RelayCommand(RunTimer));

        public RelayCommand CloseTimerCommand =>
            _closeTimerCommand ?? (_closeTimerCommand = new RelayCommand(CloseTimerDialog));


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

        public List<StreamModel.StreamQuality> StreamQualities => new List<StreamModel.StreamQuality>
        {
            StreamModel.StreamQuality.High,
            StreamModel.StreamQuality.Low
        };

        public StreamModel.StreamQuality SelectedStreamQuality
        {
            get { return _selectedStreamQuality; }
            set
            {
                if (_selectedStreamQuality == value)
                    return;

                Set(ref _selectedStreamQuality, value);
                UpdateStreamQuality();
            }
        }

        public int TimerHours
        {
            get { return _timerHours; }
            set { Set(ref _timerHours, value); }
        }

        public int TimerMinutes
        {
            get { return _timerMinutes; }
            set { Set(ref _timerMinutes, value); }
        }

        public int SelectedPivot
        {
            get { return _selectedPivot; }
            set { Set(ref _selectedPivot, value); }
        }

        public bool IsHistoryLoading
        {
            get { return _isHistoryLoading; }
            set { Set(ref _isHistoryLoading, value); }
        }

        public bool IsTimerOpened
        {
            get { return _isTimerOpened; }
            set { Set(ref _isTimerOpened, value); }
        }

        public Timer StopPlaybackTimer
        {
            get { return _stopPlaybackTimer;}
            set { Set(ref _stopPlaybackTimer, value); }
        }

        public RadioModel Radio
        {
            get { return _radio; }
            set { Set(ref _radio, value); }
        }

        public RadioInfoModel RadioInfo
        {
            get { return _radioInfo; }
            set { Set(ref _radioInfo, value); }
        }

        public List<HistorySongModel> History
        {
            get { return _history;}
            set { Set(ref _history, value); }
        }

        public PlaybackSessionViewModel PlaybackSession { get; private set; }

        public ToggleRadioPinCommand ToggleRadioPinCommand { get; set; }

        public FavoriteRadioCommand FavoriteRadioCommand { get; set; }

        public LoadHistoryCommand LoadHistoryCommand { get; set; }

        public PlayerViewModel(IMusicService musicService, PlaybackService playbackService, ITileManager tileManager, INavigationService navigationService,
            MediaNotify mediaNotify, LastRadiosStorage lastRadiosStorage, ToggleRadioPinCommand toggleRadioPinCommand, FavoriteRadioCommand favoriteRadioCommand)
        {
            _musicService = musicService;
            _playbackService = playbackService;
            _tileManager = tileManager;
            _navigationService = navigationService;
            _mediaNotify = mediaNotify;
            _lastRadiosStorage = lastRadiosStorage;
            ToggleRadioPinCommand = toggleRadioPinCommand;
            FavoriteRadioCommand = favoriteRadioCommand;
            LoadHistoryCommand = new LoadHistoryCommand(musicService, this);

            PlaybackSession = new PlaybackSessionViewModel(playbackService.Player.PlaybackSession);
            _mediaNotify.MediaUpdated += BackgroundMediaUpdate;

            var helper = new RoamingObjectStorageHelper();

            var lastQuality = StreamModel.StreamQuality.High;
            if (helper.KeyExists(_roamingQualityKey))
            {
                lastQuality = helper.Read<StreamModel.StreamQuality>(_roamingQualityKey);
            }
            SelectedStreamQuality = lastQuality;
        }


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
                case MediaPlaybackState.Buffering:
                case MediaPlaybackState.Opening:
                    _playbackService.Stop();
                    Information = Radio.CreateMusicInformation();
                    if (StopPlaybackTimer != null)
                        StopTimer();
                    break;

                case MediaPlaybackState.Paused:
                case MediaPlaybackState.None:
                    _playbackService.Play();
                    //TODO: What if Information is still null at this point?
                    //TODO: It should be set as soon as it is available
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
            SelectedPivot = 0;
            Information = null;
            Radio = null;
            RadioInfo = null;
            History = null;
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
            Radio.IsPinned = _tileManager.Exists(Radio);
            await _lastRadiosStorage.Add(Radio.Id);
            MessengerInstance.Send(new RecentAddedMessage(this, Radio.Id));

            _mediaNotify.Enabled = Radio.OnAir;

            await LoadStreamAsync();

            _playbackService.Play();

            await LoadInfoAsync();
        }

        private async Task LoadRadioAsync()
        {
            Radio = (await _musicService.GetRadiosAsync())
                .FirstOrDefault(radio => radio.IsCorrect(Radio.Id));

            // ToDo: handle this a show this in proper way
            if (Radio == null)
                throw new RadioNotFoundException();

            Radio.IsFavorite = LocalDatabaseStorage.IsFavorite(Radio.Id);

            _radioLoaded = true;
        }

        private async Task<StreamModel> LoadBestHighQualityStream(List<StreamFormatModel> formats)
        {
            // Prioritize best quality
            // See doc/aac-vs-mp3-quality-comparison.jpg
            var orderedStreamFormats = new List<Tuple<int, string>>
            {
                new Tuple<int, string>(256, "aac"),
                new Tuple<int, string>(192, "aac"),
                new Tuple<int, string>(320, "mp3"),
                new Tuple<int, string>(160, "aac"),
                new Tuple<int, string>(192, "mp3"),
                new Tuple<int, string>(128, "aac"),
                new Tuple<int, string>(160, "mp3"),
                new Tuple<int, string>(96, "aac"),
                new Tuple<int, string>(128, "mp3"),
                new Tuple<int, string>(96, "mp3"),
                new Tuple<int, string>(64, "aac"),
                new Tuple<int, string>(64, "mp3"),
            };

            StreamFormatModel selectedStream = null;
            var selectedBitrate = 0;
            // Try to match any of those formats
            foreach (var streamFormat in orderedStreamFormats)
            {
                var bitrate = streamFormat.Item1;
                var format = streamFormat.Item2;
                selectedStream = formats.FirstOrDefault(x => x.Bitrates.Contains(bitrate) && x.Format.ToLower() == format);
                if (selectedStream != null)
                {
                    selectedBitrate = bitrate;
                    break;
                }
            }

            // If it is not possible to pick from configuration list, just select the highest bitrate stream
            if (selectedStream == null)
            {
                selectedStream = formats.OrderBy(x => x.Bitrates.Max()).First();
                selectedBitrate = selectedStream.Bitrates.Max();
            }
            var stream = await _musicService.GetRadioStreamAsync(Radio.Id, selectedStream.Format, selectedBitrate);
            stream.Quality = StreamModel.StreamQuality.High;
            return stream;
        }

        private async Task<StreamModel> LoadBestLowQualityStream(List<StreamFormatModel> formats)
        {
            // Prioritize lowest bitrate, if possible select AAC, then MP3 and lastly OGG
            // Because alphabetically AAC < MP3 < OGG, orderding by format string is sufficient
            var selectedStream = formats.OrderBy(x => x.Bitrates.Min()).ThenBy(x => x.Format.ToLower()).First();
            var selectedBitrate = selectedStream.Bitrates.Min();
            var stream = await _musicService.GetRadioStreamAsync(Radio.Id, selectedStream.Format, selectedBitrate);
            stream.Quality = StreamModel.StreamQuality.Low;
            return stream;
        }

        public async Task LoadStreamAsync()
        {
            var formats = await _musicService.GetAllRadioStreamsAsync(Radio.Id);

            Radio.Streams = new List<StreamModel>();

            // Select Low Quality stream
            var lqStream = await LoadBestLowQualityStream(formats);
            Radio.Streams.Add(lqStream);

            // Select High Quality stream
            var hqStream = await LoadBestHighQualityStream(formats);
            Radio.Streams.Add(hqStream);

            UpdateStreamQuality();
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

            RadioInfo = await _musicService.GetRadioInfo(Radio.Id);

        }

        public async void BackgroundMediaUpdate(object sender, EventArgs args)
        {
            if (Radio == null)
                return;

            var song = await _musicService.GetOnAirAsync(Radio.Id);

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Information = song.CreateMusicInformation();
            });
        }

        public void UpdateStreamQuality()
        {
            if (Radio == null)
                return;

            StreamModel newQualityStream = null;
            switch (SelectedStreamQuality)
            {
                case StreamModel.StreamQuality.High:
                    newQualityStream = Radio.Streams.FirstOrDefault(x => x.Quality == StreamModel.StreamQuality.High);
                    break;
                case StreamModel.StreamQuality.Low:
                    newQualityStream = Radio.Streams.FirstOrDefault(x => x.Quality == StreamModel.StreamQuality.Low);
                    break;
                default:
                    throw new ArgumentException("Unhandled stream quality value");
            }

            if (newQualityStream == null)
                return;

            _playbackService.Stream = newQualityStream;

            var roamingStorage = new RoamingObjectStorageHelper();
            roamingStorage.Save(_roamingQualityKey, newQualityStream.Quality);

            if (PlaybackSession.PlaybackState != MediaPlaybackState.Paused)
            {
                _playbackService.Stop();
                _playbackService.Play();
            }
                
            RaisePropertyChanged(() => Stream);
        }

        public void ToggleTimer()
        {
            if (StopPlaybackTimer != null)
            {
                StopTimer();
            }
            else
            {
                IsTimerOpened = true;
            }
        }

        public void RunTimer()
        {
            IsTimerOpened = false;

            var dueTime = (TimerHours * 60 + TimerMinutes) * 60 * 1000;
            StopPlaybackTimer = new Timer(TimerStopPlayback, null, dueTime, 0);
            
        }

        public void CloseTimerDialog()
        {
            Messenger.Default.Send(new CloseDialogMessage(this, "TimerDialog"));
        }

        public void StopTimer()
        {
            if (StopPlaybackTimer == null)
                return;

            StopPlaybackTimer.Change(Timeout.Infinite, Timeout.Infinite);
            StopPlaybackTimer.Dispose();
            StopPlaybackTimer = null;
        }

        private async void TimerStopPlayback(object state)
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(
                () =>
                {
                    _playbackService.Stop();
                    StopPlaybackTimer.Dispose();
                    StopPlaybackTimer = null;
                });
        }

        public void Dispose()
        {
            _mediaNotify.MediaUpdated -= BackgroundMediaUpdate;
            if (StopPlaybackTimer != null)
                StopTimer();
            PlaybackSession.Dispose();
        }
    }
}