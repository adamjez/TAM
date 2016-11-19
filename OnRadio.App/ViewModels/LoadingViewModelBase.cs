using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace OnRadio.App.ViewModels
{
    public abstract class LoadingViewModelBase : ViewModelBase
    {
        private bool loaded = false;
        private bool _loading;

        public bool Loaded
        {
            get { return loaded; }
            set
            {
                if (value == loaded) return;
                loaded = value;
                RaisePropertyChanged();
            }
        }

        public bool Loading
        {
            get { return _loading; }
            set { Set(ref _loading, value); }
        }

        internal async void StartLoadData()
        {
            if (!Loaded)
            {
                Loading = true;
                await LoadData();
                Loading = false;

                Loaded = true;
            }
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