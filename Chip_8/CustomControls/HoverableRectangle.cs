using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Chip_8.CustomControls;

[PseudoClasses(":pointerover", ":click")]
public class HoverableRectangle : Rectangle
{
    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        
        PseudoClasses.Set(":pointerover", true);
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        
        PseudoClasses.Set(":pointerover", false);
        PseudoClasses.Set(":click", false);
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);
        
        PseudoClasses.Set(":pointerover", false);
        PseudoClasses.Set(":click", false);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        
        PseudoClasses.Set(":click", true);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        
        PseudoClasses.Set(":click", false);
    }
}