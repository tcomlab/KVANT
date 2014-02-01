using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using КВАНТ.Logical.ProgrammEngine.array_l_element;

namespace КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls
{
    /// <summary>
    /// Interaction logic for AI.xaml
    /// </summary>
    public partial class AI : UserControl, IControlVisual
    {
        private ElementData _content;
        
        private Logical.ProgrammEngine.array_l_element.Ai AILogic;

        public event MouseButtonEventHandler ModuleOnDown;
        public event MouseButtonEventHandler ModuleOnUp;
        public event EventHandler DeleteElement;
        public Point Position
        {
            get{
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

        public AI(object element, Point posPoint, bool b)
        {
            InitializeComponent();
            
            _content = ((IContentData)element).Content;
            DataContext = _content;
            AILogic = (Logical.ProgrammEngine.array_l_element.Ai)element;
            VElement = new VisualElement{ X = AILogic.X, Y = AILogic.Y};
            Position = posPoint;

            if ((Uslovie)AILogic.Content.MiscObject[0] == Uslovie.Bolshe)
                Button1.Content = "<";
            else
                Button1.Content = ">";

            Helper1.SetBinding(Helper.CaptionTextProperty, new Binding("Name") { Source = ((IContentData)element).Content });
            Helper1.SetBinding(Helper.EnableBlinkProperty, new Binding("BlockExec1Bool") { Source = ((IContentData)element).Content });

            if (b)
            {
                LabelImage.Visibility = Visibility;
                MenuItem1.IsChecked = true;
            }
        }


        // Пользователь захотел поменять знак
        private void ClickButton(object sender, RoutedEventArgs e)
        {
            var usl = (Uslovie)AILogic.Content.MiscObject[0];
            if (usl == Uslovie.Bolshe)
            {
                AILogic.Content.MiscObject[0] = Uslovie.Menshe;
                Button1.Content = ">";
            }
            else
            {
                AILogic.Content.MiscObject[0] = Uslovie.Bolshe;
                Button1.Content = "<";
            }
        }

        private void MenuItem_Click_Move(object sender, RoutedEventArgs e)
        {
            var a = (MenuItem)sender;
            LabelImage.Visibility = a.IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        private void MenuItem_Click_Edit(object sender, RoutedEventArgs e)
        {
            AILogic.Dialog();
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
