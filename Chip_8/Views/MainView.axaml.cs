using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Chip_8.ViewModels;

namespace Chip_8.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        
        FileDrop.PointerPressed += OpenFile;
        
        DragDrop.AddDragOverHandler(FileDrop, OnDragOver);
        DragDrop.AddDropHandler(FileDrop, OnDrop);
        DragDrop.AddDragEnterHandler(FileDrop, OnDragEnter);
        DragDrop.AddDragLeaveHandler(FileDrop, OnDragLeave);
    }
    
    private void OpenFile(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
            vm.SetUpInterpreterCommand.Execute(null);
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        ScreenIndicator.IsVisible = false;
        
        if (DataContext is MainViewModel vm)
        {
            if (e.DataTransfer.TryGetFile() is not { } file) return;
            
            var path = file.Path.AbsolutePath;
            
            if (path.EndsWith(".ch8") || path.EndsWith(".bin"))
            {
                vm.SetUpInterpreterCommand.Execute(path);
            }
        }
    }
    
    private void OnDragEnter(object? sender, DragEventArgs e) => ScreenIndicator.IsVisible = true;
    
    private void OnDragLeave(object? sender, DragEventArgs e) => ScreenIndicator.IsVisible = false;
    
    private void OnDragOver(object? sender, DragEventArgs e) => e.DragEffects = DragDropEffects.Move;
}