using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using КВАНТ.Logical.ProgrammEngine.array_l_element;
using BTLogic = КВАНТ.Logical.ProgrammEngine.array_l_element.BT;

namespace КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls
{
    /// <summary>
    /// Interaction logic for BT.xaml
    /// </summary>
    public partial class BT : UserControl, IControlVisual
    {
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

        private BTLogic hirself;

        public BT(object element, Point posPoint, bool b)
        {
            hirself = (BTLogic)element;
            InitializeComponent();
            TextBlock1.Text = hirself.Content.Text[0];
            Button1.Fill = Brushes.Gainsboro;
            hirself.ReciveDataFromControl(null, BTLogic.ButtonState.NoPressed);
            VElement = new VisualElement { X = hirself.X, Y = hirself.Y };
            Position = posPoint;
            if (b)
            {
                LabelImage.Visibility = Visibility;
                MenuItem1.IsChecked = true;
            }
            Button1.Fill = hirself.Content.GetColor(0);
        }

        private void Button1_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var st = (Function)hirself.Content.MiscObject[0];
            if (st == Function.Nc || st == Function.No)
            {
                TextBlock1.Text = hirself.Content.Text[0];
                hirself.ReciveDataFromControl(sender, BTLogic.ButtonState.NoPressed);
                Button1.Fill = hirself.Content.GetColor(0);
            }
        }

        private void Button1_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            
            hirself.ReciveDataFromControl(sender, BTLogic.ButtonState.Pressed);
            var st = (Function)hirself.Content.MiscObject[0];
            if (st == Function.Nc || st == Function.No)
            {
                Button1.Fill = hirself.Content.GetColor(1);
                TextBlock1.Text = hirself.Content.Text[1];}
            else
            {
                Button1.Fill = hirself.Content.GetColor(Convert.ToInt32(!hirself.ElementExecuted()));
                TextBlock1.Text = hirself.Content.Text[Convert.ToInt32(!hirself.ElementExecuted())];
            }
        }


        private void MenuItem_Click_Move(object sender, RoutedEventArgs e)
        {
            var a = (MenuItem)sender;
            LabelImage.Visibility = a.IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        private void MenuItem_Click_Edit(object sender, RoutedEventArgs e)
        {
            hirself.Dialog();
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

        private void Button1_OnMouseLeave(object sender, MouseEventArgs e)
        {
            
        }

    }
}
