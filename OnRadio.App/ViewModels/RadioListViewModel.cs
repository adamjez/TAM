using GalaSoft.MvvmLight.Command;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;
using OnRadio.BL.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bezysoftware.Navigation;

namespace OnRadio.App.ViewModels
{
    public class RadioListViewModel : LoadingViewModelBase
    {
        private readonly IMusicService _musicService;
        private readonly PlaybackService _playbackService;
        private readonly INavigationService _navigatioinService;

        private ObservableCollection<RadioModel> _radioList;

        private RadioModel _selectedRadioItem;

        private RelayCommand _itemSelectedCommand;

        private RelayCommand _sortByPopularityCommand;

        private RelayCommand _sortAlphabeticallyCommand;

        public RadioListViewModel(IMusicService musicService, PlaybackService playbackService, INavigationService navigatioinService)
        {
            _musicService = musicService;
            _playbackService = playbackService;
            _navigatioinService = navigatioinService;
        }

        public ObservableCollection<RadioModel> RadioList
        {
            get { return _radioList; }
            set { Set(ref _radioList, value); }
        }

        public RadioModel SelectedRadioItem
        {
            get { return _selectedRadioItem; }
            set { Set(ref _selectedRadioItem, value); }
        }


        public RelayCommand ItemSelectedCommand =>
            _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand(async () => await ItemSelected()));

        public RelayCommand SortByPopularityCommand =>
            _sortByPopularityCommand ?? (_sortByPopularityCommand = new RelayCommand(SortByPopularity));

        public RelayCommand SortAlphabeticallyCommand =>
            _sortAlphabeticallyCommand ?? (_sortAlphabeticallyCommand = new RelayCommand(SortAlphabetically));

        public async Task ItemSelected()
        {
            // Save pointer to current radio before someone select something different
            var currentRadio = SelectedRadioItem;
            if (currentRadio == null)
            {
                return;
            }

            await _navigatioinService.NavigateAsync<PlayerViewModel, RadioModel>(currentRadio);

            //var result1 = await _musicService.GetStyles();

            //var result2 = await _musicService.GetOnAirHistoryAsync(currentRadio.Id);

            //var streams = await _musicService.GetAllRadioStreamsAsync(currentRadio.Id);

            //var selectedStream = streams.First();
            //var selectedBitrate = selectedStream.Bitrates.First();
            //var stream = await _musicService.GetRadioStreamAsync(currentRadio.Id, selectedStream.Format, selectedBitrate);

            //_playbackService.Play(stream);
            //_playbackService.SetMusicInformation(currentRadio.CreateMusicInformation());

            //if (currentRadio.OnAir)
            //{
            //    var song = await _musicService.GetOnAirAsync(currentRadio.Id);
            //    _playbackService.SetMusicInformation(song.CreateMusicInformation());
            //}
        }

        public void SortByPopularity()
        {
            RadioList = new ObservableCollection<RadioModel>(
                RadioList.OrderByDescending(radio => radio.Listenters));
        }

        public void SortAlphabetically()
        {
            RadioList = new ObservableCollection<RadioModel>(
                RadioList.OrderBy(radio => radio.Title));
        }

        protected override async Task LoadData()
        {
            List<RadioModel> items = await _musicService.GetRadiosAsync();
            RadioList = new ObservableCollection<RadioModel>(items);
        }
    }
}