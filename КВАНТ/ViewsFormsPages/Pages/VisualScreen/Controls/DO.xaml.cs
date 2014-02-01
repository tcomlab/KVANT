using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using КВАНТ.Logical.ProgrammEngine.array_l_element;

namespace КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls
{
    /// <summary>
    /// Interaction logic for DO.xaml
    /// </summary>
    public partial class DO : UserControl, IControlVisual
    {
        private Logical.ProgrammEngine.array_l_element.DO DOLogic;

        public event MouseButtonEventHandler ModuleOnDown;
        public event MouseButtonEventHandler ModuleOnUp;
        public event EventHandler DeleteElement;
        public Point Position
        {
            get
            {
                return new Point(this.Margin.Left, this.Margin.Top);
            }
            set
            {
                var m = this.Margin;
                m.Top = value.Y;
                m.Left = value.X;
                this.Margin = m;
                VElement.Position = new Point((int)value.X, (int)value.Y);
            }
        }

        public bool SetEdit
        {
            set
            {
                LabelImage.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                MenuItem1.IsChecked = value;
            }
        }

        public VisualElement VElement { get; set; }

        public DO(object element, Point posPoint, bool b)
        {
            InitializeComponent();
            DOLogic = (Logical.ProgrammEngine.array_l_element.DO) element;
            VElement = new VisualElement { X = DOLogic.X, Y = DOLogic.Y };
            DataContext = ((IContentData)element).Content;
            Position = posPoint;

            if (b)
            {
                LabelImage.Visibility = Visibility;
                MenuItem1.IsChecked = true;
            }
        }

        private void MenuItem_Click_Move(object sender, RoutedEventArgs e)
        {
            var a = (MenuItem)sender;
            LabelImage.Visibility = a.IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        private void MenuItem_Click_Edit(object sender, RoutedEventArgs e)
        {
            DOLogic.Dialog();
        }

        private void MenuItem_Click_Remove(object sender, RoutedEventArgs e)
        {
            if (DeleteElement != null)
                DeleteElement(this, null);
        }

        private void LabelImage_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ModuleOnDown != null)
                ModuleOnDown(this, e);
        }

        private void LabelImage_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ModuleOnUp != null)
                ModuleOnUp(this, e);
        }
    }
}
