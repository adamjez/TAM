using System;
using System.Diagnostics;
using System.Windows.Input;
using OnRadio.App.ViewModels;
using OnRadio.BL.Interfaces;

namespace OnRadio.App.Commands
{
    public class LoadHistoryCommand : ICommand
    {
        private bool _canLoad = true;

        private IMusicService _musicService;

        private PlayerViewModel _playerViewModel;

        public LoadHistoryCommand(IMusicService musicService, PlayerViewModel playerViewModel)
        {
            _musicService = musicService;
            _playerViewModel = playerViewModel;
        }

        public bool CanExecute(object parameter)
        {
            int Index = (int) parameter;
            return Index == 2 && CanLoad;
        }

        public async void Execute(object parameter)
        {
            CanLoad = false;
            _playerViewModel.IsHistoryLoading = true;

            var hist = await _musicService.GetOnAirHistoryAsync(_playerViewModel.Radio.Id);

            _playerViewModel.History = hist;
            _playerViewModel.IsHistoryLoading = false;

            CanLoad = true;

        }

        public bool CanLoad
        {
            get { return _canLoad; }
            set
            {
                _canLoad = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}