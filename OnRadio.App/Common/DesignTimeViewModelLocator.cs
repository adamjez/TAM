using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OnRadio.App.ViewModels;
using OnRadio.BL.Models;

namespace OnRadio.App.Common
{
    public class DesignTimeViewModelLocator
    {

        public RadioListViewModel RadioList => new DesignRadioListViewModel();

        public class DesignRadioListViewModel : RadioListViewModel
        {
            public DesignRadioListViewModel()
                : base(null, null, null)
            {

            }

            protected override Task LoadData()
            {
                RadioList = new ObservableCollection<RadioModel>(
                    new[]
                    {
                        new RadioModel()
                        {
                            Title = "Radio Beat"
                        },
                        new RadioModel()
                        {
                            Title = "Evropa 2"
                        }
                    });
                return Task.FromResult(true);
            }
        }

        public PlayerViewModel Player => new DesignPlayerViewModel();

        public class DesignPlayerViewModel : PlayerViewModel
        {
            public DesignPlayerViewModel()
                : base(null, null, null)
            {

            }

            protected override Task LoadData()
            {
                Radio = new RadioModel()
                {
                    Title = "Radio Beat"
                };
                return Task.FromResult(true);
            }
        }
    }
}