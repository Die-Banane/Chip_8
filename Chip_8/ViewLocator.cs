using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Chip_8.ViewModels;

namespace Chip_8;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;
        
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);
        
        if (type is null)
            return null;
        
        var control = (Control)Activator.CreateInstance(type)!;
        control.DataContext = data;
        
        return control;
    }

    public bool Match(object? data) => data is ViewModelBase;
}