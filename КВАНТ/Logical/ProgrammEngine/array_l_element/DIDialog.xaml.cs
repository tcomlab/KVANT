using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    /// <summary>
    /// Interaction logic for DIDialog.xaml
    /// </summary>
    public partial class DIDialog : IModuleDialogInterface
    {
        public ElementData ContentData { get; set; }

        public List<ModuleRemot> AvalibleModules { get; set; }

        public string Id
        {
            get { return String.Format("ID:{0}", ContentData.X * ContentData.Y); }
        }

        public DIDialog(ElementData content, List<ModuleRemot> avalibleModules)
        {
            if (content == null) content = new ElementData();
            InitializeComponent();
            ContentData = content;
            DataContext = ContentData;
            AvalibleModules = avalibleModules;
            foreach (ModuleRemot t in AvalibleModules)
                DeviceNameBox.Items.Add(String.Format("{0}<{1}>", t.Name, t.Adress));
            DeviceNameBox.SelectedIndex = 0;
            if (ContentData.MiscObject[0] == null) ContentData.MiscObject[0] = Function.Nc;
            var sel = (Function)ContentData.MiscObject[0];
            switch (sel)
            {
                case Function.Nc:
                    Ch1.IsChecked = true;
                    break;
                case Function.No:
                    Ch2.IsChecked = true;
                    break;
                case Function.TNc:
                    Ch3.IsChecked = true;
                    break;
                case Function.TNo:
                    Ch4.IsChecked = true;
                    break;
            }}

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var a = (string)((RadioButton)sender).Tag;
            switch (a)
            {
                case "0":
                    ContentData.MiscObject[0] = Function.Nc;
                    break;
                case "1":
                    ContentData.MiscObject[0] = Function.No;
                    break;
                case "2":
                    ContentData.MiscObject[0] = Function.TNc;
                    break;
                case "3":
                    ContentData.MiscObject[0] = Function.TNo;
                    break;
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
            var tmp = AvalibleModules[DeviceNameBox.SelectedIndex];
            ContentData.DeviceBinding.ModuleId = tmp.ModuleId;
            ContentData.DeviceBinding.Adress = tmp.Adress;
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
