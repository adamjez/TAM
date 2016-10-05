using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OnRadio.App.Services;
using OnRadio.App.ViewModels;

namespace OnRadio.App.Common
{
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IMusicService, PlayCzMusicService>();
            }
            else
            {
                SimpleIoc.Default.Register<IMusicService, PlayCzMusicService>();
            }

            SimpleIoc.Default.Register<RadioListViewModel>();
        }

        public RadioListViewModel RadioList => ServiceLocator.Current.GetInstance<RadioListViewModel>();
    }
}