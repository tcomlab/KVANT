using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    /// <summary>
    /// Interaction logic for KGDialog.xaml
    /// </summary>
    public partial class KGDialog :IModuleDialogInterface
    {
        public ElementData ContentData { get; set; }

        public string Id
        {
            get { return String.Format("ID:{0}", ContentData.X * ContentData.Y); }
        }

        public List<ModuleRemot> AvalibleModules { get; set; }

        private List<object> lsList = new List<object>();

        public KGDialog(ElementData content, List<ModuleRemot> avalibleModules)
        {
            InitializeComponent();
			ContentData = content;
			this.DataContext = content;
            AvalibleModules = avalibleModules;

            lsList.AddRange(Kernel.GetProgram().ProgramModel.GetListOfType(Type.AI));
            lsList.AddRange(Kernel.GetProgram().ProgramModel.GetListOfType(Type.BT));
            lsList.AddRange(Kernel.GetProgram().ProgramModel.GetListOfType(Type.DI));
            lsList.AddRange(Kernel.GetProgram().ProgramModel.GetListOfType(Type.DO));
            lsList.AddRange(Kernel.GetProgram().ProgramModel.GetListOfType(Type.DT));
            lsList.AddRange(Kernel.GetProgram().ProgramModel.GetListOfType(Type.EN));
            lsList.AddRange(Kernel.GetProgram().ProgramModel.GetListOfType(Type.MR));
            lsList.AddRange(Kernel.GetProgram().ProgramModel.GetListOfType(Type.RT));
            lsList.AddRange(Kernel.GetProgram().ProgramModel.GetListOfType(Type.TI));
            
            if (lsList.Count > 0)
            {
                foreach (IContentData t in lsList)
                    ComboBox1.Items.Add(t.Content.Name);
            }
            else
                ComboBox1.Items.Add("<Нет>");
            ComboBox1.SelectedIndex = 0;

            if (ContentData.MiscObject[2] == null) ContentData.MiscObject[2] = Function.Nc;

            var sel = (Function)ContentData.MiscObject[2];
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
            var so = ContentData.MiscObject[0];
            var si = ComboBox1.SelectedIndex;
            var obj = lsList[si];
            ContentData.MiscObject[1] = ((IContentData)obj).Content;
            DialogResult = true;
            Close();
        }

        private void OnClickCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                var el = (RadioButton)sender;
                if (((string)el.Tag) == "0")
                {
                    ContentData.MiscObject[2] = Function.Nc;
                }
                else
                {
                    ContentData.MiscObject[2] = Function.No;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
