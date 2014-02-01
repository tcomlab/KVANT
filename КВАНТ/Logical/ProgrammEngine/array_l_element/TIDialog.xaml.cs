using System;
using System.Collections.Generic;
using System.Windows;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    /// <summary>
    /// Interaction logic for TIDialog.xaml
    /// </summary>
    public partial class TIDialog : IModuleDialogInterface
    {
        public ElementData ContentData { get; set; }

        public List<ModuleRemot> AvalibleModules { get; set; }

        public string Id
        {
            get { return String.Format("ID:{0}", ContentData.X * ContentData.Y); }
        }

        public TIDialog(ElementData content, List<ModuleRemot> avalibleModules)
        {
            InitializeComponent();
            AvalibleModules = avalibleModules;
            ContentData = content;
            this.DataContext = content;
        }

        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }

        private static bool IsTextNumeric(string str)
        {
            var reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
