using Autofac;
using Bezysoftware.Navigation;
using Bezysoftware.Navigation.Lookup;
using Bezysoftware.Navigation.Platform;
using Bezysoftware.Navigation.StatePersistence;

namespace OnRadio.App.Installers
{
    public class NavigationServiceInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NavigationService>()
                .As<INavigationService>();
            builder.RegisterType<ViewLocator>()
                .As<IViewLocator>();
            builder.RegisterType<ViewModelServiceLocator>()
                .As<IViewModelLocator>();
            builder.RegisterType<StatePersistor>()
                .As<IStatePersistor>();
            builder.RegisterType<PlatformNavigator>()
                .As<IPlatformNavigator>();
            builder.RegisterType<CurrentWindowFrameProvider>()
                .As<IApplicationFrameProvider>();
            builder.RegisterType<GlobalAssemblyResolver>()
                .As<IAssemblyResolver>();
            builder.RegisterType<AdaptiveNavigationInterceptor>()
                .As<INavigationInterceptor>();

        }
    }
}