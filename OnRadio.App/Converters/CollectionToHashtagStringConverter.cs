using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace OnRadio.App.Converters
{
    public class CollectionToHashtagStringConverter : IValueConverter
    {
        public string Prefix { get; set; } = "";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var collection = value as IEnumerable<string>;

            if (collection != null)
            {
                return string.Join(" ", collection.Take(6).Select(x => '#' + x));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}