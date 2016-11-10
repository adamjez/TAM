using System;
using Windows.Media.Playback;
using Windows.UI.Xaml.Data;
using OnRadio.BL.Models;

namespace OnRadio.App.Converters
{
    public class PlaybackQualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            const string lowQuality = "Lq";
            const string highQuality = "Hq";

            var quality = value as StreamModel.StreamQuality? ?? StreamModel.StreamQuality.High;
            switch (quality)
            {
                case StreamModel.StreamQuality.High:
                    return highQuality;
                case StreamModel.StreamQuality.Low:
                    return lowQuality;
                default:
                    throw new ArgumentException("Unhandled stream quality value");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
