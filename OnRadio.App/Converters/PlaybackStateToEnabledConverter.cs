using System;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace OnRadio.App.Converters
{
    public class PlaybackStateToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is MediaPlaybackState)) return null;
            var state = (MediaPlaybackState)value;

            return state == MediaPlaybackState.Playing ||
                   state == MediaPlaybackState.Buffering ||
                   state == MediaPlaybackState.Opening;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}