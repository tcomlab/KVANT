using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using КВАНТ.Logical.ProgrammEngine.array_l_element;

namespace КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls
{
    /// <summary>
    /// Interaction logic for RT.xaml
    /// </summary>
    public partial class RT : UserControl, IControlVisual
    {
        private Logical.ProgrammEngine.array_l_element.RT RTLogic;

        public RT(object element, Point posPoint, bool b)
        {
            InitializeComponent();
            RTLogic = (Logical.ProgrammEngine.array_l_element.RT) element;
            VElement = new VisualElement { X = RTLogic.X, Y = RTLogic.Y };
            Position = posPoint;

            Helper1.SetBinding(Helper.CaptionTextProperty, new Binding("Name") { Source = ((IContentData)element).Content });
            Helper1.SetBinding(Helper.EnableBlinkProperty, new Binding("BlockExec1Bool") { Source = ((IContentData)element).Content });

            if ((int)RTLogic.Content.MiscObject[0] == 0)
                Button1.Content = "<";
            else
                Button1.Content = ">";

          new Thread(TimeRefresh){IsBackground = true}.Start();

          if (b)
          {
              LabelImage.Visibility = Visibility;
              MenuItem1.IsChecked = true;
          }
        }

        private void TimeRefresh()
        {
            while (true)
            {
                Thread.Sleep(1000);
                Dispatcher.Invoke(delegate
                {
                    Display1.Text = DateTime.Now.TimeOfDay.ToString();
                    Display2.Text = ((DateTime) RTLogic.Content.MiscObject[1]).TimeOfDay.ToString();
                });

            }
        }


        private void ClickButton(object sender, RoutedEventArgs e)
        {
            var usl = (int)RTLogic.Content.MiscObject[0];
            if (usl == 0)
            {
                RTLogic.Content.MiscObject[0] = 1;
                Button1.Content = ">";
            }
            else
            {
                RTLogic.Content.MiscObject[0] = 0;
                Button1.Content = "<";
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
            RTLogic.Dialog();
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

        public bool SetEdit
        {
            set
            {
                LabelImage.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                MenuItem1.IsChecked = value;
            }
        }
    }
}
