using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Microsoft.Toolkit.Uwp.UI.Converters;

namespace OnRadio.App.Converters
{
    public class EmptyCollectionVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the value to be returned when the collection is neither null nor empty
        /// </summary>
        public object NotEmptyValue { get; set; }

        /// <summary>
        /// Gets or sets the value to be returned when the collection is either null or empty
        /// </summary>
        public object EmptyValue { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.Toolkit.Uwp.UI.Converters.CollectionVisibilityConverter" /> class.
        /// </summary>
        public EmptyCollectionVisibilityConverter()
        {
            this.NotEmptyValue = Visibility.Collapsed;
            this.EmptyValue = Visibility.Visible;
        }

        /// <summary>Convert a boolean value to an other object.</summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference.</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int count = (int)value;

            return count > 0 ? NotEmptyValue : EmptyValue;
        }

        /// <summary>Not implemented</summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The type of the target property, as a type reference (System.Type for Microsoft .NET, a TypeName helper struct for Visual C++ component extensions (C++/CX)).</param>
        /// <param name="parameter">An optional parameter to be used to invert the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}