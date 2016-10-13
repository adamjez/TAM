using GalaSoft.MvvmLight.Command;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;
using OnRadio.BL.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnRadio.App.ViewModels
{
    public class RadioListViewModel : LoadingViewModelBase
    {
        private readonly IMusicService _musicService;
        private readonly PlaybackService _playbackService;

        private ObservableCollection<RadioModel> _radioList;

        private RadioModel _selectedRadioItem;

        private RelayCommand _itemSelectedCommand;

        private RelayCommand _sortByPopularityCommand;

        private RelayCommand _sortAlphabeticallyCommand;

        public RadioListViewModel(IMusicService musicService, PlaybackService playbackService)
        {
            _musicService = musicService;
            _playbackService = playbackService;
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

            var stream = await _musicService.GetRadioStreamUrlAsync(currentRadio.Id);

            _playbackService.Play(stream.First());
            _playbackService.SetMusicInformation(currentRadio.CreateMusicInformation());

            if (currentRadio.OnAir)
            {
                var song = await _musicService.GetOnAir(currentRadio.Id);
                _playbackService.SetMusicInformation(song.CreateMusicInformation());
            }
        }

        public void SortByPopularity()
        {
            RadioList = new ObservableCollection<RadioModel>(
                RadioList.OrderByDescending(radio => radio.Listenters));
        }

        public void SortAlphabetically()
        {
            RadioList = new ObservableCollection<RadioModel>(
                RadioList.OrderByDescending(radio => radio.Title));
        }

        protected override async Task LoadData()
        {
            List<RadioModel> items = await _musicService.GetRadiosAsync();
            RadioList = new ObservableCollection<RadioModel>(items);
        }
    }
}