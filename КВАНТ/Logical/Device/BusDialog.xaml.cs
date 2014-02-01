using System.IO.Ports;
using System.Windows;
using System.Windows.Media;

namespace КВАНТ.Logical.Device
{
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class BusDialog 
    {
        public BusProperties propetry ;

        public BusDialog(BusProperties properties)
        {
            InitializeComponent();
            propetry = properties;
            var a = SerialPort.GetPortNames();
            foreach (string t in a)
                PortList.Items.Add(t);
            DataContext = propetry;
            Closing += BusDialog_Closing;
            ColorPicker1.SelectedColor = (Color)ColorConverter.ConvertFromString((string)propetry.Color);
        }
        //(SolidColorBrush)new BrushConverter().ConvertFromString((string)value);
        void BusDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           propetry.Color = ColorPicker1.SelectedColor.ToString();
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();             
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
