using System;
using Windows.UI.Xaml.Data;

namespace OnRadio.App.Converters
{
    public class TimeToRelativeTimeConventer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime songTime = (DateTime) value;
            const int second = 1;
            const int minute = 60 * second;
            const int hour = 60 * minute;

            var ts = new TimeSpan(DateTime.Now.Ticks - songTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 0)
                return "právě hraje";

            if (delta < 2 * minute)
                return "před minutou";

            if (delta < 90 * minute)
                return "před " + ts.Minutes + " minutami";

            if (delta < 2*hour)
                return "před hodinou";

            return "před " + ts.Hours + " hodinami";


        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}