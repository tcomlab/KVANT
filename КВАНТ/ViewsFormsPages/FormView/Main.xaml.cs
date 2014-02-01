using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using КВАНТ.Logical.ProgrammEngine;
using КВАНТ.ViewsFormsPages.Pages.HWConfig;
using КВАНТ.ViewsFormsPages.Pages.LogicProgram;
using КВАНТ.ViewsFormsPages.Pages.SaveOpen;
using КВАНТ.ViewsFormsPages.Pages.VisualScreen;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;

namespace КВАНТ.ViewsFormsPages.FormView
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static EventHandler StopSystem;

        public static Grid RootFieldGrid;

        private UserControl _current = new UserControl();

        public MainWindow()
        {
            InitializeComponent();
            var Sm = new HelloWindow();
            Dispatcher.Invoke(Sm.Show);
            RootFieldGrid = RootGrid;
            this.Closing += MainWindow_Closed;
            _satart_sys();
            RootFieldGrid.Children.Add(_current);
            System.Threading.Thread.Sleep(3000);
            Dispatcher.Invoke(Sm.Close);
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            var Sm = new StopMessage();
            Dispatcher.Invoke(Sm.Show);
            
            if (StopSystem != null) StopSystem(this, EventArgs.Empty);
            System.Threading.Thread.Sleep(1000);
            Kernel.GetProgram().SaveProject(Set.Default.Path);
            System.Threading.Thread.Sleep(1000);
            Dispatcher.Invoke(Sm.Close);
        }

        private void _satart_sys()
        {
            if (Pages.SaveOpen.Set.Default.Path == "")
            {
                // Если проект не найден
                //NewProgram(null, null);
                ButtonConfig.IsEnabled = false;
                ButtonProgram.IsEnabled = false;
                ButtonVisual.IsEnabled = false;
            }
            else
            { // если проект найден
                Kernel.SetProgramm(new Project());
                Kernel.GetProgram().OpenProject(Set.Default.Path);
            }
        }

        private void LoadConfig(object sender, RoutedEventArgs e)
        {
            RootFieldGrid.Children.Remove(_current);
            _current = new HWConfig();
            RootFieldGrid.Children.Add(_current);
            ButtonConfig.IsEnabled = false;
            ButtonProgram.IsEnabled = true;
            ButtonVisual.IsEnabled = true;
        }

        private void LoadVisual(object sender, RoutedEventArgs e)
        {
            RootFieldGrid.Children.Remove(_current);
            _current = new VisualProgramm();
            RootFieldGrid.Children.Add(_current);
            ButtonConfig.IsEnabled = true;
            ButtonProgram.IsEnabled = true;
            ButtonVisual.IsEnabled = false;
        }

        private void LoadProgram(object sender, RoutedEventArgs e)
        {
            RootFieldGrid.Children.Remove(_current);
            _current = new LogicalProgram();
            RootFieldGrid.Children.Add(_current);
            ButtonConfig.IsEnabled = true;
            ButtonProgram.IsEnabled = false;
            ButtonVisual.IsEnabled = true;
        }


        private void OpenProgram(object sender, RoutedEventArgs e)
        {
            Kernel.GetProgram().OpenProject();
        }

        private void SaveProgram(object sender, RoutedEventArgs e)
        {
            Kernel.GetProgram().SaveProject(Set.Default.Path);
        }

        private void NewProgram(object sender, RoutedEventArgs e)
        {
            Kernel.GetProgram().CreateProject();
        }
    }
}
