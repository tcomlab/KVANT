using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using КВАНТ.Logical.Device;


namespace КВАНТ.ViewsFormsPages.Pages.HWConfig.Controls
{
    /// <summary>
    /// Логика взаимодействия для Bus.xaml
    /// </summary>
    public partial class BusControl : UserControl
    {
        private Bus _bus;

        public static EventHandler RemovedCmd;
        public static EventHandler MouseUpPic;
        public static EventHandler MouseDwPic;
        public static EventHandler ModuleAdd;

        public Bus GeBus { get { return _bus;} }

        public BusControl(Bus bus)
        {
            InitializeComponent();
            _bus = bus;
            DataContext = bus.BusProperties;
        }

        public BusControl()
        {
            
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            _bus.BusDialogSetting();
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            if (RemovedCmd != null) RemovedCmd(this, e);
        }

        private void IsMoved(object sender, RoutedEventArgs e)
        {
            var a = (MenuItem)sender;
            LabelImage.Visibility = a.IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        private void UIElement_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MouseUpPic != null) MouseUpPic(this, e);
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MouseDwPic != null) MouseDwPic(this, e);
        }

        // Команда добавления модуля
        private void AddModule(object sender, RoutedEventArgs e)
        {
            if (ModuleAdd != null) 
                ModuleAdd(this, e);
        }

    }
}
