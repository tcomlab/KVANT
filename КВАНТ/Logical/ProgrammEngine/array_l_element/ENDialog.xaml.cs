﻿using System;
using System.Collections.Generic;
using System.Windows;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    /// <summary>
    /// Interaction logic for ENDialog.xaml
    /// </summary>
    public partial class ENDialog : IModuleDialogInterface
    {
        public ElementData ContentData { get; set; }
        public List<ModuleRemot> AvalibleModules { get; set; }
        public string Id
        {
            get { return String.Format("ID:{0}", ContentData.X * ContentData.Y); }
        }

        public ENDialog(ElementData content, List<ModuleRemot> avalibleModules)
        {
            InitializeComponent();
            if (content == null) content = new ElementData();
            ContentData = content;
			this.DataContext = content;
            AvalibleModules = avalibleModules;
            foreach (ModuleRemot t in AvalibleModules)
                    DeviceNameBox.Items.Add(String.Format("{0}<{1}>", t.Name, t.Adress));
            DeviceNameBox.SelectedIndex = 0;
            Closing += ENDialog_Closing;
        }

        void ENDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
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
