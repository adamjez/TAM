using GalaSoft.MvvmLight.Command;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Models;
using OnRadio.BL.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Windows.ApplicationModel.VoiceCommands;
using GalaSoft.MvvmLight.Views;
using OnRadio.App.Messages;
using OnRadio.App.Services;
using OnRadio.App.Views;
using OnRadio.DAL;

namespace OnRadio.App.ViewModels
{
    public class RadioListViewModel : LoadingViewModelBase
    {
        private readonly IMusicService _musicService;
        private readonly ITileManager _tileManager;
        private readonly INavigationService _navigationService;

        private ObservableCollection<RadioModel> _radioList;

        private ObservableCollection<RadioModel> _allRadioList;

        private ObservableCollection<RadioModel> _favoriteRadioList;

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
            MessengerInstance.Register<FavoriteChangeMessage>(this, FavoriteChangeHandler);
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

        public ObservableCollection<RadioModel> FavoriteRadioList
        {
            get { return _favoriteRadioList; }
            set { Set(ref _favoriteRadioList, value); }
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
            List<RadioModel> items = await _musicService.GetRadiosAsync();
            AllRadioList = new ObservableCollection<RadioModel>(items);
            RadioList = AllRadioList; // Pro filtrovani
            FavoriteRadioList = new ObservableCollection<RadioModel>(LoadFavorites());
        }

        private IEnumerable<RadioModel> LoadFavorites()
        {
            List<FavoriteRadio> favList = LocalDatabaseStorage.GetFavorites();
            return AllRadioList
                .Where(radio => favList.Select(f => f.RadioId).Contains(radio.Id))
                .ToList();
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


        /// <summary>
        /// Whenever a trip is modified, we trigger an update of the voice command Phrase list. This allows
        /// voice commands such as "Adventure Works Show trip to {destination} to be up to date with saved
        /// Trips.
        /// </summary>
        public async Task UpdateRadioPhraseList(string commandSetKey)
        {
            try
            {
                // Update the destination phrase list, so that Cortana voice commands can use destinations added by users.
                // When saving a trip, the UI navigates automatically back to this page, so the phrase list will be
                // updated automatically.
                VoiceCommandDefinition commandDefinitions;

                if (VoiceCommandDefinitionManager.InstalledCommandDefinitions.TryGetValue(commandSetKey, out commandDefinitions))
                {
                    List<string> destinations = new List<string>();
                    foreach (var radio in AllRadioList)
                    {
                        destinations.Add(radio.Title);
                    }

                    await commandDefinitions.SetPhraseListAsync("radio", destinations);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Updating Phrase list for VCDs: " + ex);
            }
        }
    }
}