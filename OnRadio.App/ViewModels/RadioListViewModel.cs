using GalaSoft.MvvmLight.Command;
using OnRadio.App.Models;
using OnRadio.App.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OnRadio.App.ViewModels
{
    public class RadioListViewModel : LoadingViewModelBase
    {
        private readonly IMusicService _musicService;

        private ObservableCollection<RadioItem> _radioList;

        public RadioListViewModel(IMusicService musicService)
        {
            _musicService = musicService;
        }

        public ObservableCollection<RadioItem> RadioList
        {
            get { return _radioList; }
            set { Set(ref _radioList, value); }
        }

        protected override async Task LoadData()
        {
            List<RadioItem> items = await _musicService.GetRadiosAsync();
            RadioList = new ObservableCollection<RadioItem>(items);
        }
    }
}