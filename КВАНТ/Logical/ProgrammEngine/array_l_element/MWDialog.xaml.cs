using System;
using System.Collections.Generic;
using System.Windows;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    /// <summary>
    /// Interaction logic for MWDialog.xaml
    /// </summary>
    public partial class MWDialog : IModuleDialogInterface
    {
        public ElementData ContentData { get; set; }
        public List<ModuleRemot> AvalibleModules { get; set; }

        public string Id
        {
            get { return String.Format("ID:{0}", ContentData.X * ContentData.Y); }
        }

        List<object> arg = new List<object>();

        public MWDialog(ElementData content, List<ModuleRemot> avalibleModules)
        {
            InitializeComponent();
            ContentData = content;
            this.DataContext = content;
            AvalibleModules = avalibleModules;
            var proj = Kernel.GetProgram();
            arg = proj.ProgramModel.GetListOfType(Type.MR);
            if (arg.Count > 0)
            {
                foreach (MR t in arg)
                    ComboBox1.Items.Add(t.Name);
            }
            else
                ComboBox1.Items.Add("<Нет>");

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
            try
            {
                object[] a = ContentData.MiscObject;
                var arp = arg[ComboBox1.SelectedIndex];
                ContentData.MiscObject = new Object[2];
                ContentData.MiscObject[0] = a[0];
                ContentData.MiscObject[1] = ((IContentData)arp).Content;
                //ContentData.Name = ((IContentData)arp).Content.Name;
            }
            catch
            {
            }
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
