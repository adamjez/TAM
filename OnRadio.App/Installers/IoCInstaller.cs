using System;
using Windows.Storage;
using Autofac;
using GalaSoft.MvvmLight.Views;
using Microsoft.Toolkit.Uwp.UI;
using OnRadio.App.Services;
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

            builder.RegisterType<HttpClient>()
                .Named<IHttpClient>("httpClient");

            builder.RegisterDecorator<IHttpClient>(
                (c, inner) => new CachedHttpClientDecorator(inner), "httpClient");

            builder.RegisterType<PlayCzMusicService>()
                .As<IMusicService>();

            builder.RegisterType<MediaNotify>()
                .SingleInstance();


            var navigation = new NavigationService();
            navigation.Configure(nameof(Player), typeof(Player));
            navigation.Configure(nameof(RadioList), typeof(RadioList));
            builder.RegisterInstance(navigation)
               .As<INavigationService>();



            // Image Cache
            //var cache = new ImageCache {CacheDuration = TimeSpan.FromDays(7)};
            //cache.InitializeAsync(ApplicationData.Current.LocalCacheFolder, "tmp").GetAwaiter().GetResult();
            //builder.RegisterInstance(cache);

            builder.RegisterType<TileManager>()
                .As<ITileManager>();

            builder.RegisterType<ImageManager>()
               .As<IImageManager>();
        }
    }
}