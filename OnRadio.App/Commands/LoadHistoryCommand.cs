using System;
using System.Windows.Input;
using OnRadio.App.ViewModels;
using OnRadio.BL.Interfaces;

namespace OnRadio.App.Commands
{
    public class LoadHistoryCommand : ICommand
    {
        private bool _isLoading;

        private IMusicService _musicService;

        private PlayerViewModel _playerViewModel;

        public LoadHistoryCommand(IMusicService musicService, PlayerViewModel playerViewModel)
        {
            _musicService = musicService;
            _playerViewModel = playerViewModel;
        }

        public bool CanExecute(object parameter)
        {
            int index = (int) parameter;
            return index == 1 && !IsLoading;
        }

        public async void Execute(object parameter)
        {
            IsLoading = true;

            _playerViewModel.History = await _musicService.GetOnAirHistoryAsync(_playerViewModel.Radio.Id);

            IsLoading = false;
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                _playerViewModel.IsHistoryLoading = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}