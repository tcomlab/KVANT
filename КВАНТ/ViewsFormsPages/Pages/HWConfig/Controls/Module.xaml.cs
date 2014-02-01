using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using КВАНТ.Logical.Engine;
using System.Drawing;
using Type = КВАНТ.Logical.ProgrammEngine.array_l_element.Type;

namespace КВАНТ.ViewsFormsPages.Pages.HWConfig.Controls
{
    /// <summary>
    /// Interaction logic for Module.xaml
    /// </summary>
    public partial class Module : UserControl
    {
        ///BeginStoryboard beginsb;
        Storyboard flsb;
        public ModuleRemot _mrRemot;
       
        public static event EventHandler RemoveCMD;
        public static event EventHandler ModuleOnDown;
        public static event EventHandler ModuleOnUp;
        public bool Cancel = false;
        public Line LineBusDraw { set; get; }

        public Point Position
        {
            set
            {
                var m = this.Margin;
                m.Left = value.X;
                m.Top = value.Y;
                this.Margin = m;
            }
            get
            {
                var m = this.Margin;
                return new Point((int) m.Left, (int) m.Top);
            }
        }

        public Module()
        {
            InitializeComponent();
            flsb = (Storyboard)FindResource("FlashOn");

            _mrRemot = new ModuleRemot(0, Type.AI);
            DataContext = _mrRemot;

            _mrRemot.ConnectFault += mrRemot_ConnectFault;
            _mrRemot.ConnectNormal += mrRemot_ConnectNormal;

            LabelImage.MouseDown += LabelImage_MouseDown; //ModuleOnDown;
            LabelImage.MouseUp += LabelImage_MouseUp; //ModuleOnUp;

        }
   
        public Module(ModuleRemot remot,Point point, bool dialog)
        {
            InitializeComponent();

            flsb = (Storyboard)FindResource("FlashOn");

            _mrRemot = remot;
            DataContext = _mrRemot;

            remot.ConnectFault += mrRemot_ConnectFault;
            remot.ConnectNormal += mrRemot_ConnectNormal;

            LabelImage.MouseDown += LabelImage_MouseDown; //ModuleOnDown;
            LabelImage.MouseUp += LabelImage_MouseUp; //ModuleOnUp;

            if (dialog)
            {
                remot.ModuleSettingDialog();
                if (remot.Cancel)
                {
                    Cancel = true;
                    return;
                }
            }
            Cancel = false;
            Position = point;
        }
        
        void mrRemot_ConnectNormal(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke((Action) (() => flsb.Stop()));
        }

        void mrRemot_ConnectFault(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() => flsb.Begin()));
        }

        void LabelImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ModuleOnUp != null) ModuleOnUp(this, e);
        }

        void LabelImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ModuleOnDown != null) ModuleOnDown(this, e);
        }

        private void MenuItem_Click_Move(object sender, RoutedEventArgs e)
        {
            var a = (MenuItem)sender;
            LabelImage.Visibility = a.IsChecked ? Visibility.Visible : Visibility.Hidden;
        }

        private void MenuItem_Click_Edit(object sender, RoutedEventArgs e)
        {
            _mrRemot.ModuleSettingDialog(); 
        }

        private void MenuItem_Click_Remove(object sender, RoutedEventArgs e)
        {
            if (RemoveCMD != null) RemoveCMD(this, e); 
        }

        private void MenuItem_Click_Diagnostic(object sender, RoutedEventArgs e)
        {
            new Diagnostic(_mrRemot).Show();
        }
    }
}
