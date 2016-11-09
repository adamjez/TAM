﻿using System;
using Windows.Media.Playback;
using Windows.UI.Xaml.Data;
using OnRadio.BL.Models;

namespace OnRadio.App.Converters
{
    public class FavoriteIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            const string isFavorite = "ms-appx:/Icons/star.png";
            const string notFavorite = "ms-appx:/Icons/star_blank.png";

            if (!(value is bool)) return null;
            var state = (bool)value;

            return state ? isFavorite : notFavorite;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}