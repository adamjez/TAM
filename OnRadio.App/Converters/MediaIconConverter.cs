using System;
using Windows.Media.Playback;
using Windows.UI.Xaml.Data;

namespace OnRadio.App.Converters
{
    public class MediaIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var media = (string) parameter;
            var disabled = ((string) value) == "";

            string icon;
            switch (media)
            {
                case "twitter":
                    icon = "twitter";
                    break;
                case "facebook":
                    icon = "facebook";
                    break;
                case "youtube":
                    icon = "youtube";
                    break;
                case "gplus":
                    icon = "gplus";
                    break;
                default:
                    icon = "";
                    break;
            }

            if (disabled)
                icon += "_dis";           

            return $"ms-appx:/Icons/{icon}.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
