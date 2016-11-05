using GalaSoft.MvvmLight.Command;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;
using OnRadio.BL.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using OnRadio.App.Services;
using OnRadio.App.Views;

namespace OnRadio.App.ViewModels
{
    public class RadioListViewModel : LoadingViewModelBase
    {
        private readonly IMusicService _musicService;
        private readonly ITileManager _tileManager;
        private readonly INavigationService _navigationService;

        private ObservableCollection<RadioModel> _radioList;

        private ObservableCollection<RadioModel> _allRadioList;

        private RadioModel _selectedRadioItem;

        private RelayCommand _itemSelectedCommand;

        private RelayCommand _sortByPopularityCommand;

        private RelayCommand _sortAlphabeticallyCommand;

        private RelayCommand _filterListCommand;

        private string _searchString;

        public RadioListViewModel(IMusicService musicService, ITileManager tileManager, INavigationService navigationService)
        {
            _musicService = musicService;
            _tileManager = tileManager;
            _navigationService = navigationService;
        }

        public ObservableCollection<RadioModel> RadioList
        {
            get { return _radioList; }
            set { Set(ref _radioList, value); }
        }

        public ObservableCollection<RadioModel> AllRadioList
        {
            get { return _allRadioList; }
            set { Set(ref _allRadioList, value); }
        }


        public RadioModel SelectedRadioItem
        {
            get { return _selectedRadioItem; }
            set { Set(ref _selectedRadioItem, value); }
        }

        public string SearchString
        {
            get { return _searchString; }
            set { Set(ref _searchString, value); Console.WriteLine(_searchString); }
        }

        public RelayCommand ItemSelectedCommand =>
            _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand(ItemSelected));

        public RelayCommand SortByPopularityCommand =>
            _sortByPopularityCommand ?? (_sortByPopularityCommand = new RelayCommand(SortByPopularity));

        public RelayCommand SortAlphabeticallyCommand =>
            _sortAlphabeticallyCommand ?? (_sortAlphabeticallyCommand = new RelayCommand(SortAlphabetically));

        public RelayCommand FilterListCommand =>
            _filterListCommand ?? (_filterListCommand = new RelayCommand(FilterList));

        public void ItemSelected()
        {
            // Save pointer to current radio before someone select something different
            var currentRadio = SelectedRadioItem;
            if (currentRadio == null)
            {
                return;
            }
            //if (_tileManager.Exists(currentRadio))
            //{
            //    await _tileManager.RemoveTileAsync(currentRadio);

            //}
            //else
            //{
            //    await _tileManager.CreateTileAsync(currentRadio);

            //}
            _navigationService.NavigateTo(nameof(Player), currentRadio);
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

        public void FilterList()
        {
            RadioList = new ObservableCollection<RadioModel>(
                AllRadioList.Where(radio => radio.Title.ToLower().Contains(SearchString.ToLower()))
            );
        }

        protected override async Task LoadData()
        {
            if (Loaded)
            {
                return;
            }

            List<RadioModel> items = await _musicService.GetRadiosAsync();
            AllRadioList = new ObservableCollection<RadioModel>(items);
            RadioList = AllRadioList; // Pro filtrovani
        }
       
    }
}