using System;
using System.Collections.Generic;
using System.Windows;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    /// <summary>
    /// Interaction logic for DTDialog.xaml
    /// </summary>
    public partial class DTDialog : IModuleDialogInterface
    {
        public ElementData ContentData { get; set; }
        public List<ModuleRemot> AvalibleModules { get; set; }

        public string Id
        {
            get { return String.Format("ID:{0}", ContentData.X * ContentData.Y); }
        }

        public DTDialog(ElementData content, List<ModuleRemot> avalibleModules)
        {
            InitializeComponent();
			ContentData = content;
			this.DataContext = content;
            AvalibleModules = avalibleModules;
            foreach (ModuleRemot t in avalibleModules)
                    ComboBox1.Items.Add(String.Format("{0}<{1}>", t.Name, t.Adress));
            ComboBox1.SelectedIndex = 0;
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

        private void OnClickCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnClickSave(object sender, RoutedEventArgs e)
        {
            var tmp = AvalibleModules[ComboBox1.SelectedIndex];
            ContentData.DeviceBinding.ModuleId = tmp.ModuleId;
            ContentData.DeviceBinding.Adress = tmp.Adress;
            DialogResult = true;
            Close();
        }
    }
}
