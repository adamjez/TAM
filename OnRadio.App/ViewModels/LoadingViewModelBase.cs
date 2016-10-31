using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace OnRadio.App.ViewModels
{
    public abstract class LoadingViewModelBase : ViewModelBase
    {
        private bool loaded = false;

        public bool Loaded
        {
            get { return loaded; }
            private set
            {
                if (value == loaded) return;
                loaded = value;
                RaisePropertyChanged();
            }
        }

        internal async void StartLoadData()
        {
            await LoadData();
            Loaded = true;
        }

        protected virtual Task LoadData()
        {
            return Task.FromResult(true);
        }

        public virtual void Initialize(object argument)
        {

        }
    }
}