using System;
using Windows.UI.Xaml.Data;

namespace OnRadio.App.Converters
{
    public class StringToUriConverter : IValueConverter
    {
        public string Prefix { get; set; } = "";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var url = value as string;

            if (!string.IsNullOrEmpty(url))
            {
                return new Uri(Prefix + url);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class EmailToUriConverter : StringToUriConverter
    {
        public EmailToUriConverter()
        {
            Prefix = "mailto:";
        }
    }

    public class TelToUriConverter : StringToUriConverter
    {
        public TelToUriConverter()
        {
            Prefix = "tel:";
        }
    }
}