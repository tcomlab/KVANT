using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    /// <summary>
    /// Interaction logic for AIDialog.xaml
    /// </summary>
    /// 
    /// 
    /// 
    public class ButtonList : List<Button> { }
    
    public partial class AiDialog : IModuleDialogInterface
    {
        public ElementData ContentData { get; set; }

        public string Id
        {
            get { return String.Format("ID:{0}", ContentData.X * ContentData.Y); }
        }

        public List<ModuleRemot> AvalibleModules { get; set; }

        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
        }

        private static bool IsTextNumeric(string str)
        {
            var reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        public AiDialog(ElementData content, List<ModuleRemot> avalibleModules)
        {
            InitializeComponent();
            ContentData = content;
            this.DataContext = content;
            AvalibleModules = avalibleModules;
            foreach (var t in avalibleModules)
                DeviceNameBox.Items.Add(String.Format("{0}<{1}>", t.Name, t.Adress));
            DeviceNameBox.SelectedIndex = 0;
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
