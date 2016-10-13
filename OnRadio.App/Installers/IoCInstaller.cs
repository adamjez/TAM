using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using OnRadio.BL.Interfaces;
using OnRadio.BL.Services;
using OnRadio.PlayCz;

namespace OnRadio.App.Installers
{
    public class IoCInstaller
    {
        public static void Install()
        {
            var service = new PlaybackService();
            SimpleIoc.Default.Register(() => service);
            SimpleIoc.Default.Register(() => service.Player);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IMusicService, PlayCzMusicService>();
            }
            else
            {
                SimpleIoc.Default.Register<IMusicService, PlayCzMusicService>();
            }
        }
    }
}