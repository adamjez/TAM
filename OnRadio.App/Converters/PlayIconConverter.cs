using System;
using Windows.Media.Playback;
using Windows.UI.Xaml.Data;

namespace OnRadio.App.Converters
{
    public class PlayIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //const string playIcon = "ms-appx:/Icons/play.png";
            //const string pauseIcon = "ms-appx:/Icons/pause.png";

            const string playIcon = "\uE102";
            const string pauseIcon = "\uE15B";

            if (value is MediaPlaybackState)
            {
                var state = (MediaPlaybackState)value;

                if (state == MediaPlaybackState.Playing)
                {
                    return pauseIcon;
                }
                else
                {
                    return playIcon;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
