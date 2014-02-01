using System.Windows;
using System.Windows.Controls;
using КВАНТ.Logical.ProgrammEngine;
using System.Windows.Forms;

namespace КВАНТ.ViewsFormsPages.Pages.SaveOpen
{
    /// <summary>
    /// Interaction logic for SaveLoad.xaml
    /// </summary>
    public partial class SaveLoad : Page
    {
        public SaveLoad()
        {
            InitializeComponent();
            this.Unloaded += SaveLoad_Unloaded;
            FileNameTextBox.Text = Set.Default.Path;
        }

        void SaveLoad_Unloaded(object sender, RoutedEventArgs e)
        {
            Set.Default.Save();
        }

        private void ClickButton(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.FileName = Set.Default.Path;
            DialogResult result = dialog.ShowDialog();

            // Display OpenFileDialog by calling ShowDialog method
            
            // Get the selected file name and display in a TextBox
            if (result == DialogResult.OK)
            {
                // Open document
                string filename = dialog.FileName;
                FileNameTextBox.Text = filename;
                Set.Default.Path = filename;
                Set.Default.Save();
            }
        }

        private void ClickSave(object sender, RoutedEventArgs e)
        {
            Kernel.GetProgram().SaveProject(Set.Default.Path);
        }

        private void ClickOpen(object sender, RoutedEventArgs e)
        {
            Kernel.GetProgram().OpenProject(Set.Default.Path);
        }

        private void ClickGreate(object sender, RoutedEventArgs e)
        {
           // Kernel.GetProgram().CreateProject();
        }
    }
}
