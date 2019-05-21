using Entitas.MatchLine;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Performance.Converters
{
    public class GridPositionToMarginConverter : IValueConverter
    {
        public int Offset { get; set; }

        public int FixedMargin { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GridPosition position)
            {
                if (position.x % 2 == 0)
                {
                    return new Thickness(FixedMargin, FixedMargin - Offset, FixedMargin, FixedMargin + Offset);
                }
                else
                {
                    return new Thickness(FixedMargin, FixedMargin + Offset, FixedMargin, FixedMargin - Offset);

                }
            }
            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}

