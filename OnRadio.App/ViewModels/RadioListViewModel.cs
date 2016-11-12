using GalaSoft.MvvmLight.Command;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;
using OnRadio.BL.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.VoiceCommands;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Views;
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

        private ObservableCollection<GroupRadioList> _groupRadioList;
        private RelayCommand<ItemClickEventArgs> _itemSelectedCommand;
        private RelayCommand _sortByPopularityCommand;
        private RelayCommand _sortAlphabeticallyCommand;
        private RelayCommand _filterListCommand;

        private string _searchString;

        public RadioListViewModel(IMusicService musicService, ITileManager tileManager, INavigationService navigationService)
        {
            _musicService = musicService;
            _tileManager = tileManager;
            _navigationService = navigationService;
            MessengerInstance.Register<FavoriteChangeMessage>(this, FavoriteChangeHandler);
        }

        public List<RadioModel> RadioList { get; set; }

        public ObservableCollection<GroupRadioList> GroupRadioList
        {
            get { return _groupRadioList; }
            set { Set(ref _groupRadioList, value); }
        }

        public string SearchString
        {
            get { return _searchString; }
            set { Set(ref _searchString, value); }
        }

        public RelayCommand<ItemClickEventArgs> ItemSelectedCommand =>
            _itemSelectedCommand ?? (_itemSelectedCommand = new RelayCommand<ItemClickEventArgs>(ItemSelected));

        public RelayCommand SortByPopularityCommand =>
            _sortByPopularityCommand ?? (_sortByPopularityCommand = new RelayCommand(SortByPopularity));

        public RelayCommand SortAlphabeticallyCommand =>
            _sortAlphabeticallyCommand ?? (_sortAlphabeticallyCommand = new RelayCommand(SortAlphabetically));

        public RelayCommand FilterListCommand =>
            _filterListCommand ?? (_filterListCommand = new RelayCommand(FilterList));        

        public void ItemSelected(ItemClickEventArgs arg)
        {
            // Save pointer to current radio before someone select something different
            if (arg.ClickedItem == null)
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
            _navigationService.NavigateTo(nameof(Player), arg.ClickedItem);
        }

        public void SortByPopularity()
        {
            //RadioList = new ObservableCollection<RadioModel>(
            //    RadioList.OrderByDescending(radio => radio.Listenters));
        }

        public void SortAlphabetically()
        {
            //RadioList = new ObservableCollection<RadioModel>(
            //    RadioList.OrderBy(radio => radio.Title));
        }

        public void FilterList()
        {
            //RadioList = new ObservableCollection<RadioModel>(
            //    AllRadioList.Where(radio => radio.Title.ToLower().Contains(SearchString.ToLower()))
            //);
        }

        protected override async Task LoadData()
        {
            RadioList = await _musicService.GetRadiosAsync();

            AddFavoriteFlag(RadioList);

            GroupRadioList = new ObservableCollection<GroupRadioList>(CreateGroupedList(RadioList));
        }

        private void AddFavoriteFlag(List<RadioModel> items)
        {
            List<FavoriteRadio> favList = LocalDatabaseStorage.GetFavorites();
            foreach(var item in items.Where(radio => favList.Select(f => f.RadioId).Contains(radio.Id)))
            {
                item.IsFavorite = true;
            }
        }

        public List<GroupRadioList> CreateGroupedList(List<RadioModel> radios)
        {
            var groupList = new List<GroupRadioList>();

            var query = radios.GroupBy(radio => radio.IsFavorite)
                .OrderByDescending(g => g.Key)
                .Select(g => new {GroupName = g.Key, Items = g});

            foreach (var group in query)
            {
                GroupRadioList info = new GroupRadioList
                {
                    Type = group.GroupName ? GroupType.Favorited : GroupType.Others
                };


                foreach (var item in group.Items)
                {
                    info.Add(item);
                }

                groupList.Add(info);
            }

            return groupList;

        }

        public void FavoriteChangeHandler(FavoriteChangeMessage message)
        {
            var selectedRadio = RadioList.FirstOrDefault(r => r.Id == message.RadioId);

            if (selectedRadio == null)
            {
                return;
            }

            var favoriteList = GroupRadioList.FirstOrDefault(x => x.Type == GroupType.Favorited);
            var otherList = GroupRadioList.FirstOrDefault(x => x.Type == GroupType.Others);
            if (message.Favorited)
            {
                if (favoriteList == null)
                {
                    favoriteList = new GroupRadioList
                    {
                        Type = GroupType.Favorited
                    };
                    GroupRadioList.Insert(0, favoriteList);
                }

                favoriteList.Add(selectedRadio);
                otherList.Remove(selectedRadio);
            }
            else
            {
                favoriteList?.Remove(selectedRadio);
                otherList.Add(selectedRadio);
            }
        }
    }
}