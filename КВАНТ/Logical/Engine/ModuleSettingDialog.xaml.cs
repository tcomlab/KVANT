using System.Windows;

namespace КВАНТ.Logical.Engine
{
    /// <summary>
    /// Interaction logic for ModuleSettingDialog.xaml
    /// </summary>
    public partial class ModuleSettingDialog{
        public ModuleRemot MRemote { set; get; }

        public ModuleSettingDialog(ModuleRemot mrRemot)
        {
            InitializeComponent();
            MRemote = mrRemot;
            DataContext = MRemote;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
