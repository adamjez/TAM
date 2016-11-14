using System;
using Windows.Media.Playback;
using Windows.UI.Xaml.Data;

namespace OnRadio.App.Converters
{
    public class RadioPinnedIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            const string pinIcon = "\uE718"; // Pin
            const string unpinIcon = "\uE77A"; // Unpin

            var isPinned = value as bool? ?? false;
            return isPinned ? unpinIcon : pinIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
