using Autofac;
using OnRadio.App.ViewModels;

namespace OnRadio.App.Installers
{
    public class ViewModelInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PlayerViewModel>().SingleInstance();
            builder.RegisterType<RadioListViewModel>();
        }
    }
}