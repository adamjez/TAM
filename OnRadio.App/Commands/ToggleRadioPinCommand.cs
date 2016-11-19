using System;
using System.Windows.Input;
using OnRadio.BL.Models;
using OnRadio.BL.Services;

namespace OnRadio.App.Commands
{
    public class ToggleRadioPinCommand : ICommand
    {
        private readonly ITileManager _tileManager;
        private bool _canPinRadio = true;

        public ToggleRadioPinCommand(ITileManager tileManager)
        {
            _tileManager = tileManager;
        }

        public bool CanExecute(object parameter)
        {
            return CanPinRadio;
        }

        public async void Execute(object parameter)
        {
            RadioModel radio = parameter as RadioModel;
            if (radio == null)
                return;

            CanPinRadio = false;
            radio.IsPinned = _tileManager.Exists(radio);
            if (radio.IsPinned)
            {
                await _tileManager.RemoveTileAsync(radio);
            }
            else
            {
                await _tileManager.CreateTileAsync(radio);
            }
            radio.IsPinned = !radio.IsPinned;

            CanPinRadio = true;
        }

        public bool CanPinRadio
        {
            get { return _canPinRadio; }
            set
            {
                _canPinRadio = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}