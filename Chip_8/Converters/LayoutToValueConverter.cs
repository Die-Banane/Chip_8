using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Chip_8.Data;

namespace Chip_8.Converters;

public class LayoutToValueConverter : IValueConverter
{
    private readonly List<KeyValuePair<string, KeyPadLayouts>> _layouts =
    [
        new("Qwerty", KeyPadLayouts.Qwerty),
        new("Qwertz", KeyPadLayouts.Qwertz),
        new("Azerty", KeyPadLayouts.Azerty)
    ];
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is KeyPadLayouts layout)
            return _layouts.First(x => x.Value == layout);
        
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is KeyValuePair<string, KeyPadLayouts> pair)
            return pair.Value;

        return null;
    }
}