using Autofac;
using Autofac.Builder;
using Microsoft.Practices.ServiceLocation;
using OnRadio.App.ViewModels;

namespace OnRadio.App.Common
{
    public class ViewModelLocator
    {
        public RadioListViewModel RadioList => App.AutofacContainer.Resolve<RadioListViewModel>();
        public PlayerViewModel Player => App.AutofacContainer.Resolve<PlayerViewModel>();
    }

}