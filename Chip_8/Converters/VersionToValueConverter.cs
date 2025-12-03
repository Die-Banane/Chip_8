using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Chip_8.config;

namespace Chip_8.Converters;

public class VersionToValueConverter : IValueConverter
{
    private readonly List<KeyValuePair<string, Chip8Versions>> _versions = new()
    {
        new("Chip 8", Chip8Versions.Chip8),
        new("Super-Chip", Chip8Versions.SuperChip),
        new("XO-Chip", Chip8Versions.XoChip)
    };
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Chip8Versions version)
            return _versions.First(x => x.Value == version);
        
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is KeyValuePair<string, Chip8Versions> pair)
            return pair.Value;

        return null;
    }
}