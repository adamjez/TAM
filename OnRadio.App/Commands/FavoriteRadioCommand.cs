using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using OnRadio.App.Messages;
using OnRadio.BL.Models;
using OnRadio.DAL;

namespace OnRadio.App.Commands
{
    public class FavoriteRadioCommand : ICommand
    {
        private bool _canFavoriteRadio = true;

        public bool CanExecute(object parameter)
        {
            return CanFavoriteRadio;
        }

        public void Execute(object parameter)
        {
            RadioModel radio = parameter as RadioModel;
            if (radio == null)
                return;

            CanFavoriteRadio = false;
            if (radio.IsFavorite)
            {
                LocalDatabaseStorage.DeleteFavorite(radio.Id);
            }
            else
            {
                LocalDatabaseStorage.InsertFavorite(radio.Id);
            }

            radio.IsFavorite = !radio.IsFavorite;
            Messenger.Default.Send(new FavoriteChangeMessage(this, radio.Id, radio.IsFavorite));

            CanFavoriteRadio = true;
        }

        public bool CanFavoriteRadio
        {
            get { return _canFavoriteRadio; }
            set
            {
                _canFavoriteRadio = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}