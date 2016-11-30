using System;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;
using OnRadio.App.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OnRadio.App.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About : MvvmPage
    {
        public About()
        {
            this.InitializeComponent();

            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            AppName.Text = $"On-Radio";
            Version.Text = $"Verze {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void OpenPlayCzWeb(object sender, RoutedEventArgs e)
        {
            const string playCzWebUrl = @"http://www.play.cz/";
            var uri = new Uri(playCzWebUrl);

            await Launcher.LaunchUriAsync(uri);
        }

        private async void OpenRateApp(object sender, RoutedEventArgs e)
        {
            var uri = new Uri(string.Format("ms-windows-store:REVIEW?PFN={0}", Package.Current.Id.FamilyName));
            await Launcher.LaunchUriAsync(uri);
        }

        private async void SendFeedback(object sender, RoutedEventArgs e)
        {
            const string emailAddress = "mailto:?to=on-radio@outlook.com&subject=Nazor na aplikaci";
            var uri = new Uri(emailAddress);

            await Launcher.LaunchUriAsync(uri);
        }
    }
}
