using System;
using Windows.UI.Xaml.Data;

namespace OnRadio.App.Converters
{
    public class PlayIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            const string playIcon = "ms-appx:/Icons/play.png";
            const string pauseIcon = "ms-appx:/Icons/pause.png";
            return (bool) value ? pauseIcon : playIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
