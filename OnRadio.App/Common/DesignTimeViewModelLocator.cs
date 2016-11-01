﻿using System.Collections.ObjectModel;
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
                            Title = "Radio Beat",
                            LogoUrl = "http://www.radio1.cz/media/images/design/radio1-logo.png"
                        },
                        new RadioModel()
                        {
                            Title = "Evropa 2",
                            LogoUrl = "http://www.radio1.cz/media/images/design/radio1-logo.png"
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
                    Title = "Radio Beat",
                    LogoUrl = "http://api.play.cz/static/radio_logo/playuk40.png"
                };
                Information = new MusicInformation()
                {
                    Artist = "Red Hot Chilli Peppers",
                    Title = "Very Long Song Title That Needs To Be Shortened",
                    ThumbnailUrl = "http://is4.mzstatic.com/image/thumb/Music49/v4/62/43/31/624331f1-024c-aefb-f3b6-aee95f3097f5/source/600x600bb.jpg"
                };
                return Task.FromResult(true);
            }
        }
    }
}