using GalaSoft.MvvmLight.Ioc;
using OnRadio.BL.BackgroundAudio;
using OnRadio.BL.Interfaces;

namespace OnRadio.App.Installers
{
    public class IoCInstaller
    {
        public static void Install()
        {
            SimpleIoc.Default.Register<IBackgroundAudio>(() => new BackgroundAudio());
        }
    }
}