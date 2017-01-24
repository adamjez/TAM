using GalaSoft.MvvmLight.Command;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;
using OnRadio.BL.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Views;
using Microsoft.Toolkit.Uwp;
using OnRadio.App.Messages;
using OnRadio.App.Models;
using OnRadio.App.Views;
using OnRadio.DAL;

namespace OnRadio.App.ViewModels
{
    public class RadioListViewModel : LoadingViewModelBase
    {
        private readonly IMusicService _musicService;
        private readonly ITileManager _tileManager;
        private readonly INavigationService _navigationService;
        private readonly LastRadiosStorage _lastRadiosStorage;

        private ObservableCollection<RadioModel> _allRadioModel;
        private ObservableCollection<RadioModel> _favoriteRadioModel;
        private ObservableCollection<RadioModel> _recentRadioModel;

        private RelayCommand<ItemClickEventArgs> _itemSelectedCommand;
        private RelayCommand _filterListCommand;
        private RelayCommand _aboutNavigateCommand;

        private string _searchString;
        private SortBy _selectedSortBy;
        private int _activeListIndex;

        public RadioListViewModel(IMusicService musicService, ITileManager tileManager, INavigationService navigationService, LastRadiosStorage lastRadiosStorage)
        {
            _musicService = musicService;
            _tileManager = tileManager;
            _navigationService = navigationService;
            _lastRadiosStorage = lastRadiosStorage;
            MessengerInstance.Register<FavoriteChangeMessage>(this, FavoriteChangeHandler);
            MessengerInstance.Register<RecentAddedMessage>(this, RecentChangeHandler);

            ActiveListIndex = (int)PivotItemType.All;
        }

        public List<RadioModel> RadioList { get; set; }

        public ObservableCollection<RadioModel> AllRadioList
        {
            get { return _allRadioModel; }
            set { Set(ref _allRadioModel, value); }
        }

        public ObservableCollection<RadioModel> FavoriteRadioList
        {
            get { return _favoriteRadioModel; }
            set { Set(ref _favoriteRadioModel, value); }
        }

        public ObservableCollection<RadioModel> RecentRadioList
        {
            get { return _recentRadioModel; }
            set { Set(ref _recentRadioModel, value); }
        }

        public string SearchString
        {
            get { return _searchString; }
            set { Set(ref _searchString, value); }
        }

        public int ActiveListIndex
        {
            get { return _activeListIndex; }
            set { Set(ref _activeListIndex, value); }
        }

        public SortBy SelectedSortBy
        {
            get { return _selectedSortBy; }
            set { Set(ref _selectedSortBy, value); }
        }

        public IEnumerable<SortBy> SortBySource => Enum.GetValues(typeof(SortBy)).Cast<SortBy>();

        public RelayCommand<ItemClickEventArgs> ItemSelectedCommand =>
            _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand<ItemClickEventArgs>(ItemSelected));

        public RelayCommand FilterListCommand =>
            _filterListCommand ?? (_filterListCommand = new RelayCommand(async () =>
            {
                Loading = true;
                await Task.Run(FilterList);
                Loading = false;
            }));

        public RelayCommand AboutNavigateCommand =>
            _aboutNavigateCommand ?? (_aboutNavigateCommand = new RelayCommand(() => _navigationService.NavigateTo(nameof(About))));

        public void ItemSelected(ItemClickEventArgs arg)
        {
            // Save pointer to current radio before someone select something different
            if (arg.ClickedItem == null)
            {
                return;
            }

            _navigationService.NavigateTo(nameof(Player), arg.ClickedItem);
        }

        public async Task FilterList()
        {
            if (RadioList == null || RadioList.Count == 0)
            {
                return;
            }

            IEnumerable<RadioModel> radios = RadioList;
            if (!string.IsNullOrEmpty(SearchString))
            {
                if (SearchString.StartsWith("#") && SearchString.Length > 1)
                {
                     var filterStyle = SearchString.Remove(0, 1).ToLower();
                     radios = radios.Where(radio => radio.Styles != null && radio.Styles.Select(style => style.ToLower()).Contains(filterStyle));
                }
                else
                {
                    radios = radios.Where(radio => radio.Title.ToLower().Contains(SearchString.ToLower()));
                }
            }

            radios = SelectedSortBy == SortBy.Popularity
                ? radios.OrderByDescending(radio => radio.Listenters)
                : radios.OrderBy(radio => radio.Title);

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                AllRadioList = new ObservableCollection<RadioModel>(radios);
            });
        }

        protected override async Task LoadData()
        {
            var favorites = LocalDatabaseStorage.GetFavorites();
            var recents = (await _lastRadiosStorage.Get()).ToList();

            if (favorites.Any())
            {
                ActiveListIndex = (int)PivotItemType.Favorite;
            }
            else if (recents.Any())
            {
                ActiveListIndex = (int)PivotItemType.Recent;
            }

            RadioList = await _musicService.GetRadiosAsync();

            AllRadioList = new ObservableCollection<RadioModel>(RadioList);
            FavoriteRadioList = CreateFavoriteRadioList(favorites, RadioList);
            RecentRadioList = CreateRecentRadioList(recents, RadioList);
        }

        private ObservableCollection<RadioModel> CreateFavoriteRadioList(List<FavoriteRadio> favList, List<RadioModel> items)
        {
            var result = new ObservableCollection<RadioModel>(
                items.Where(radio => favList.Select(f => f.RadioId).Contains(radio.Id)));

            foreach (var radio in result)
            {
                radio.IsFavorite = true;
            }

            return result;
        }

        private ObservableCollection<RadioModel> CreateRecentRadioList(IEnumerable<string> recentRadios, List<RadioModel> items)
        {
            var result = new ObservableCollection<RadioModel>();
            foreach (var radioId in recentRadios)
            {
                var selectedRadio = items.FirstOrDefault(radio => radioId == radio.Id);
                if (selectedRadio != null)
                {
                    result.Add(selectedRadio);
                }
            }

            return result;
        }

        public void FavoriteChangeHandler(FavoriteChangeMessage message)
        {
            var selectedRadio = RadioList.FirstOrDefault(r => r.Id == message.RadioId);

            if (selectedRadio == null)
            {
                return;
            }

            if (message.Favorited)
            {
                FavoriteRadioList.Add(selectedRadio);
            }
            else
            {
                FavoriteRadioList.Remove(selectedRadio);
            }
        }

        public void RecentChangeHandler(RecentAddedMessage message)
        {
            var selectedRadio = RadioList.FirstOrDefault(r => r.Id == message.RadioId);

            if (selectedRadio == null)
            {
                return;
            }

            var recent = RecentRadioList.FirstOrDefault(radio => radio.Id == message.RadioId);

            if (recent != null)
            {
                RecentRadioList.Remove(recent);
            }

            RecentRadioList.Insert(0, selectedRadio);
            RaisePropertyChanged(() => RecentRadioList);
        }
    }
}