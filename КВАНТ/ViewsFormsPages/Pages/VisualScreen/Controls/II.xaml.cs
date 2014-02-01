
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using КВАНТ.Logical.ProgrammEngine.array_l_element;

namespace КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls
{
    /// <summary>
    /// Interaction logic for II.xaml
    /// </summary>
    /// 
    /// 
    public partial class II : UserControl, IControlVisual
    {
        private Ii Ii;

        public bool SetEdit
        {
            set
            {
                LabelImage.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                MenuItem1.IsChecked = value;
            }
        }

        public II(object data, Point posPoint, bool b)
        {
            Ii = (Ii)data;
            InitializeComponent();
            DataContext = ((IContentData)data).Content;
            ((IContentData)data).Content.PropertyChanged += Content_PropertyChanged;
            VElement = new VisualElement { X = Ii.X, Y = Ii.Y };
            Position = posPoint;

            if (b)
            {
                LabelImage.Visibility = Visibility;
                MenuItem1.IsChecked = true;
            }
            Rectangle1.Fill = Ii.Content.GetColor(0);
        }

        void Content_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "BlockExec1Bool")
            {
                if (Ii.Content.BlockExec1Bool)
                {
                    Dispatcher.Invoke(delegate
                    {
                        Rectangle1.Fill = Ii.Content.GetColor(0);
                    });
                }
                else
                {
                    Dispatcher.Invoke(delegate
                    {
                        Rectangle1.Fill = Ii.Content.GetColor(1);
                    });
                }

            }
        }

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

        public VisualElement VElement { get; set; }

        private void MenuItem_Click_Move(object sender, RoutedEventArgs e)
        {
            var a = (MenuItem)sender;
            LabelImage.Visibility = a.IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        private void MenuItem_Click_Edit(object sender, RoutedEventArgs e)
        {
            Ii.Dialog();
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
