using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls
{
    /// <summary>
    /// Логика взаимодействия для helper.xaml
    /// </summary>
    public partial class Helper
    {
        public class ControlData:INotifyPropertyChanged
        {
            public string CaptionText { set { _captionText = value; OnPropertyChanged("CaptionText"); } get { return _captionText; } }
            private string _captionText;

            public bool Blink { set { _blink = value; OnPropertyChanged("Blink"); } get { return _blink; } }
            private bool _blink;

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ControlData data = new ControlData();


        //--------------------------------------------------------------------------------------------------
        public static readonly DependencyProperty EnableBlinkProperty = DependencyProperty.Register(
            "EnableBlink", typeof(bool), typeof(Helper), new PropertyMetadata(BlinkProperCalback));
        private static  void BlinkProperCalback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var helper = (Helper)d;
            helper.data.Blink = (bool)e.NewValue;
        }

        public bool EnableBlink
        {
            get { return (bool)GetValue(EnableBlinkProperty); }
            set { SetValue(EnableBlinkProperty, value); }
        }
        //--------------------------------------------------------------------------------------------------


        public static readonly DependencyProperty CaptionTextProperty = DependencyProperty.Register(
            "CaptionText", typeof(string), typeof(Helper), new PropertyMetadata(CaptionProperCalback));
        private  static void CaptionProperCalback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var helper = (Helper) d;
            helper.data.CaptionText = (string) e.NewValue;
        }
        public string CaptionText
        {
            get { return (string)GetValue(CaptionTextProperty); }
            set { SetValue(CaptionTextProperty, value); }
        }
        //--------------------------------------------------------------------------------------------------
       
        public Helper()
        {
            InitializeComponent();
            DataContext = data;
        }

      
    }
}
