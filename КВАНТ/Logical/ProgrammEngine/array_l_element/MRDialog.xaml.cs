using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    /// <summary>
    /// Interaction logic for MRDialog.xaml
    /// </summary>
    public partial class MRDialog : IModuleDialogInterface
    {
        public ElementData ContentData { get; set; }

        public string Id
        {
            get { return String.Format("ID:{0}", ContentData.X * ContentData.Y); }
        }

        public List<ModuleRemot> AvalibleModules { get; set; }

        public MRDialog(ElementData content, List<ModuleRemot> avalibleModules)
        {
            InitializeComponent();
			ContentData = content;
            avalibleModules = AvalibleModules;
			this.DataContext = content;
            if (ContentData.MiscObject[1] == null) ContentData.MiscObject[1] = Function.Nc;

            var sel = (Function)ContentData.MiscObject[1];
            switch (sel)
            {
                case Function.Nc:
                    Ch1.IsChecked = true;
                    break;
                case Function.No:
                    Ch2.IsChecked = true;
                    break;
            }
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                var el = (RadioButton)sender;
                if (((string)el.Tag) == "0")
                {
                    ContentData.MiscObject[1] = Function.Nc;
                }
                else
                {
                    ContentData.MiscObject[1] = Function.No;
                }
            }
            catch (Exception)
            {
            }
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

        private void OnClickSave(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnClickCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
