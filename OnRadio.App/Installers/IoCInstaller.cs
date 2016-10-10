using GalaSoft.MvvmLight.Ioc;
using OnRadio.BL.Services;

namespace OnRadio.App.Installers
{
    public class IoCInstaller
    {
        public static void Install()
        {
            var service = new PlaybackService();
            SimpleIoc.Default.Register(() => service);
            SimpleIoc.Default.Register(() => service.Player);
        }
    }
}