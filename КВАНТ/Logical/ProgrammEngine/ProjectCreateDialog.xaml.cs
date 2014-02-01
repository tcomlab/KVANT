using System.Windows;

namespace КВАНТ.Logical.ProgrammEngine
{
    /// <summary>
    /// Interaction logic for ProjectCreateDialog.xaml
    /// </summary>
    public partial class ProjectCreateDialog 
    {
        public int X { set; get; }
        public int Y { set; get; }
        public ProjectCreateDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OkButton(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
