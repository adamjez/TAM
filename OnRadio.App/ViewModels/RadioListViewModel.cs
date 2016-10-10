using GalaSoft.MvvmLight.Command;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;
using OnRadio.BL.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OnRadio.App.ViewModels
{
    public class RadioListViewModel : LoadingViewModelBase
    {
        private readonly IMusicService _musicService;
        private readonly PlaybackService _playbackService;

        private ObservableCollection<RadioItem> _radioList;

        private RadioItem selectedRadioItem;

        private RelayCommand _itemSelectedCommand;

        public RadioListViewModel(IMusicService musicService, PlaybackService playbackService)
        {
            _musicService = musicService;
            _playbackService = playbackService;
        }

        public ObservableCollection<RadioItem> RadioList
        {
            get { return _radioList; }
            set { Set(ref _radioList, value); }
        }

        public RadioItem SelectedRadioItem
        {
            get { return selectedRadioItem; }
            set { Set(ref selectedRadioItem, value); }
        }

        public RelayCommand ItemSelectedCommand =>
            _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand(async () => await ItemSelected()));

        public async Task ItemSelected()
        {
            var stream = await _musicService.GetRadioStreamUrlAsync(SelectedRadioItem);

            _playbackService.PlayFromUrl(stream.StreamUrl);
        }

        protected override async Task LoadData()
        {
            List<RadioItem> items = await _musicService.GetRadiosAsync();
            RadioList = new ObservableCollection<RadioItem>(items);
        }
    }
}