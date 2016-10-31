using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Services;
using OnRadio.PlayCz;

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
        }
    }
}