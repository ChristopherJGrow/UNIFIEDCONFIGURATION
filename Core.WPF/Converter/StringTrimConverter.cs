using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Config.Core.WPF.Converter;

/// <summary>
/// Converter to ensure values dont have leading or trailing spaces
/// </summary>
public class StringTrimConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var retval=System.Convert.ToString(value)?.Trim() ?? string.Empty;
        return retval;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var retval=System.Convert.ToString(value)?.Trim() ?? string.Empty;
        return retval;
    }
}

