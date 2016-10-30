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
                .As<IHttpClient>();

            builder.RegisterType<PlayCzMusicService>()
                .As<IMusicService>();
        }
    }
}