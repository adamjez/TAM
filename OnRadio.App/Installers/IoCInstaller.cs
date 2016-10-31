using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
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
        }
    }
}