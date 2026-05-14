using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using CRMApp.Models;

namespace CRMApp.Converters
{
    // Конвертирует ClientType в цвет бейджа
    [ValueConversion(typeof(ClientType), typeof(Brush))]
    public class ClientTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ClientType type)
            {
                return type == ClientType.VIP
                    ? new SolidColorBrush(Color.FromRgb(255, 215, 0))    // Gold
                    : new SolidColorBrush(Color.FromRgb(200, 230, 201)); // Light green
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    // Конвертирует ClientType в текст на русском
    [ValueConversion(typeof(ClientType), typeof(string))]
    public class ClientTypeToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is ClientType type
                ? type == ClientType.VIP ? "★ VIP" : "Обычный"
                : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
