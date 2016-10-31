using Autofac;
using GalaSoft.MvvmLight.Views;
using OnRadio.App.Views;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Services;
using OnRadio.PlayCz;
using Module = Autofac.Module;

namespace OnRadio.App.Installers
{
    public class IoCInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var service = new PlaybackService();

            builder.RegisterInstance(service);
            builder.RegisterInstance(service.Player);

            builder.RegisterType<HttpClient>()
                .Named<IHttpClient>("httpClient");

            builder.RegisterDecorator<IHttpClient>(
                (c, inner) => new CachedHttpClientDecorator(inner), "httpClient");

            builder.RegisterType<PlayCzMusicService>()
                .As<IMusicService>();


            var navigation = new NavigationService();
            navigation.Configure(nameof(Player), typeof(Player));
            builder.RegisterInstance(navigation)
               .As<INavigationService>();
        }
    }
}