using System;
using System.Windows;
using System.Windows.Input;
using КВАНТ.Logical.ProgrammEngine.array_l_element;

namespace КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls
{
    interface IControlVisual
    {
       event MouseButtonEventHandler ModuleOnDown;
       event MouseButtonEventHandler ModuleOnUp;
       event EventHandler DeleteElement;
       Point Position { set; get; }
       VisualElement VElement { set; get; }
       bool SetEdit { set; }
    }
}
