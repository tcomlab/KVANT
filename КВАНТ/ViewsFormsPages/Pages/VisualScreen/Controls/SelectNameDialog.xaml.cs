using System;
using System.Collections.Generic;
using System.Windows;
using КВАНТ.Logical.ProgrammEngine.array_l_element;

namespace КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls
{
    /// <summary>
    /// Interaction logic for SelectNameDialog.xaml
    /// </summary>
    public partial class SelectNameDialog
    {
        public int NameIndex { set; get; }

        public SelectNameDialog(List<object> data)
        {
            InitializeComponent();
            foreach (var t in data)
            {
                ListBoxNames.Items.Add(((ILogicEngine)t).Name);
            }
            this.Closing += SelectNameDialog_Closing;
        }

        void SelectNameDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NameIndex = ListBoxNames.SelectedIndex;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {if (ListBoxNames.Text == "")
                DialogResult = false;
            else
                DialogResult = true;
            Close();
        }
    }
}
