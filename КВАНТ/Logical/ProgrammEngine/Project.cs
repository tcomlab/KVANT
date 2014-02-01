using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using КВАНТ.Logical.Device;
using КВАНТ.Logical.Engine;
using КВАНТ.Logical.ProgrammEngine.array_l_element;
using КВАНТ.ViewsFormsPages.Pages.SaveOpen;


namespace КВАНТ.Logical.ProgrammEngine
{
    [Serializable]
    public class SaveModel
    {
        [XmlElement("Modules")]
        public List<ModuleRemot> Modules { set; get; }

        [XmlElement("Visual")]
        public List<VisualElement> Visual { set; get; }

        [XmlElement("Bus")]
        public List<Bus> Bus { set; get; }

        [XmlElement("ProgramModel")]
        public ElementData[] Program { set; get; }

        [XmlElement("X")]
        public int X { set; get; }

        [XmlElement("Y")]
        public int Y { set; get; }

        public SaveModel()
        {
            
        }
    }

    public class Project
    {
        private ProgramArray _programModel;

        public ProgramArray ProgramModel
        {
            set { _programModel = value; }
            get { return _programModel; }
        }

        private List<ModuleRemot> _modules = new List<ModuleRemot>();
        public List<ModuleRemot> Modules
        {
            set { _modules = value; }
            get { return _modules; }
        }

        private List<Bus> _bus = new List<Bus>();
        public List<Bus> Bus
        {
            set { _bus = value; }
            get { return _bus; }
        }

        private List<VisualElement> _visual = new List<VisualElement>();
        public List<VisualElement> Visual
        {
            set {  _visual = value; }
            get {  return _visual; }
        }

        public Project(int x,int y)
        {
            _programModel = new ProgramArray(x,y);
        }

        public Project()
        {
   
        }
   
        public void SaveProject(string filePath)
        {
            try
            {
                var smodel = new SaveModel
                {
                    Modules = _modules,
                    Bus = _bus,
                    Visual = _visual,
                    Program = _programModel.save_model(),
                    X = _programModel.X,
                    Y = _programModel.Y
                };
                var serializer = new XmlSerializer(typeof (SaveModel));
                TextWriter writer = File.CreateText(filePath);
                serializer.Serialize(writer, smodel);
                
                writer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void OpenProject()
        {
            string filePath = @"C:\KVANT\Project\";
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "kvant_project_n"; // Default file name
            dlg.DefaultExt = ".proj"; // Default file extension
            dlg.Filter = "KVANT Project file (.proj)|*.proj"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                filePath = dlg.FileName;
            }


            try
            {
                var serializer = new XmlSerializer(typeof(SaveModel));
                var reader1 = new StreamReader(filePath);
                var restored = (SaveModel)serializer.Deserialize(reader1);
                reader1.Close();
                _bus.AddRange(restored.Bus);
                _modules.AddRange(restored.Modules);
                _visual.AddRange(restored.Visual);
                Kernel.SetProgramm(this);
                _programModel = new ProgramArray();
                _programModel.load_model(restored.Program, restored.X, restored.Y);
                Kernel.SetProgramm(this);

                Device.Bus.Project = this;
               /*
                foreach (var allModule in Kernel.GetProgram().Bus)
                {
                    if (allModule.State != Device.Bus.BusState.Run) allModule.RunBus();
                }*/
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка открытия проекта");
            }
            
        }

        public void OpenProject(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(SaveModel));
                var reader1 = new StreamReader(filePath);
                var restored = (SaveModel)serializer.Deserialize(reader1);
                reader1.Close();
                _bus.AddRange(restored.Bus);
                _modules.AddRange(restored.Modules);
                _visual.AddRange(restored.Visual);
                Kernel.SetProgramm(this);
                _programModel = new ProgramArray();
                _programModel.load_model(restored.Program, restored.X, restored.Y);
                Kernel.SetProgramm(this);

                Device.Bus.Project = this;
                /*
                foreach (var allModule in Kernel.GetProgram().Bus)
                {
                    if (allModule.State != Device.Bus.BusState.Run) allModule.RunBus();
                }*/
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка открытия проекта");
            }

        }

        public void CreateProject()
        {
            string filePath = @"C:\KVANT\Project\";
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "kvant_project_n"; // Default file name
            dlg.DefaultExt = ".proj"; // Default file extension
            dlg.Filter = "KVANT Project file (.proj)|*.proj"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                filePath = dlg.FileName;
            }
            Set.Default.Path = filePath;
            Set.Default.Save();
            var pcd = new ProjectCreateDialog {X = 0, Y = 0};
            pcd.ShowDialog();
            if (pcd.DialogResult == false) return;
            _programModel = new ProgramArray(pcd.X, pcd.Y);
            //_programModel = new ProgramArray(10,10);
            Kernel.SetProgramm(this);
            SaveProject(filePath);
        }
    }
}
