using System;
using Windows.Media.Playback;
using Windows.UI.Xaml.Data;

namespace OnRadio.App.Converters
{
    public class FavoriteIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            const string addFavoriteIcon = "\uEB51"; // Empty Heart
            const string removeFavoriteIcon = "\uEB52"; // Full Heart

            var isFavorite = value as bool? ?? false;
            return isFavorite ? removeFavoriteIcon : addFavoriteIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
