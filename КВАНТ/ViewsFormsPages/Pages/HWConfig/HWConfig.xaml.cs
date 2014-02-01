using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using КВАНТ.Logical.Device;
using КВАНТ.Logical.Engine;
using КВАНТ.Logical.ProgrammEngine;
using КВАНТ.ViewsFormsPages.Pages.HWConfig.Controls;

using Type = КВАНТ.Logical.ProgrammEngine.array_l_element.Type;

namespace КВАНТ.ViewsFormsPages.Pages.HWConfig
{
    public class DiagnosticData
    {
        public string Name { set; get; }
        public Type type { set; get; }
        public int adres { set; get; }

        public object[] data { set; get; }

        public DiagnosticData()
        {
            Name = "---";
            type = Type.NN;
            adres = 0;
            data = new object[16];
            for (int i = 0; i < 16; i++)
            {
                data[i] = "---";}
        }
    }


    public partial class HWConfig : UserControl
    {
        //private Project _currentProject;
        private object _tmpMoveBus;

        public DiagnosticData DiagData;

        //---------------------------------------------------------------
        #region Операции с окном
        public HWConfig()
        {
            InitializeComponent();
            Unloaded += HWConfig_Unloaded;
            Loaded += HWConfig_Loaded;
            DiagData = new DiagnosticData(); 
            DiagnosticPanel.DataContext = DiagData;
        }

        void HWConfig_Loaded(object sender, RoutedEventArgs e)
        {
            //_currentProject = Kernel.GetProgram();
            BuldField();

            BusControl.RemovedCmd += BusRemove;
            BusControl.MouseDwPic += BusMouseDown;
            BusControl.MouseUpPic += BusMouseUp;
            BusControl.ModuleAdd += ModuleAdd;

            Module.ModuleOnDown += ModuleOnDown;
            Module.ModuleOnUp += ModuleOnUp;
            Module.RemoveCMD += ModuleRemove;

            /*foreach (var allModule in Kernel.GetProgram().Bus)
            {
                allModule.StopBus();
            }*/
        }

        void HWConfig_Unloaded(object sender, RoutedEventArgs e)
        {
            var bc = GetBusControsBy();

            for (int i = 0; i < bc.Count; i++)
            {
                for (int a = 0; a < Kernel.TtProgram.Bus.Count; a++)
                {
                    if (Kernel.TtProgram.Bus[a].BusID == bc[i].GeBus.BusID)
                    {
                        Kernel.TtProgram.Bus[a].HeighVisual = (int)bc[i].Margin.Top;
                    }
                }
            }

            var allModules = GetModulesControls();
            foreach (var allModule in allModules)
            {
                var a = Kernel.TtProgram.Modules.IndexOf(allModule._mrRemot);
                Kernel.TtProgram.Modules[a].CPoint = new Point((int)allModule.Margin.Left, (int)allModule.Margin.Top);
            }

           
            ClearField();
            /*
            foreach (var allModule in Kernel.GetProgram().Bus)
            {
                allModule.StopBus();
            }
            //Kernel.SetProgramm(_currentProject);
            //Bus.Project = _currentProject;
            foreach (var allModule in Kernel.tt_program.Bus)
            {
                allModule.RunBus();
            }
             * */
        }
        #endregion
        //--------------------------------------------------------------- 
        #region Работа с модулями
        private void ModuleAdd(object sender, EventArgs e)
        {
            var a = (RoutedEventArgs)e;
            a.Handled = true;
            var bcontrol = sender as BusControl;
            if (bcontrol != null)
            {
                var bsBus = bcontrol.GeBus;
                int busID = bsBus.BusID;

                var posit = Mouse.GetPosition(SchemeField);

                if (posit.X < 0) return;
                if (posit.Y < 0) return; // TODO Костыль который решает проблему создания кучи рандомных модулей

                var moduleVisula = new Module(new ModuleRemot() { ModuleId = GenerateUnicaleIDModule()}, new Point((int)posit.X, (int)posit.Y), true);
                if (moduleVisula.Cancel) 
                    return;
                var mRemot = moduleVisula._mrRemot;
                mRemot.BusId = busID;
            
                var ln = new Line
                {
                    StrokeThickness = 8,
                    Stroke = (SolidColorBrush) new BrushConverter().ConvertFromString(bsBus.BusProperties.Color)
                };

                ln.X1 = ln.X2 = moduleVisula.Margin.Left + 50;
                ln.Y1 = bcontrol.Margin.Top + 30;
                ln.Y2 = moduleVisula.Margin.Top;
                moduleVisula.LineBusDraw = ln;

                SchemeField.Children.Add(ln);
                Kernel.TtProgram.Modules.Add(mRemot);
                SchemeField.Children.Add(moduleVisula);
            }
        }

        private void ModuleRemove(object sender, EventArgs e)
        {
            if (sender is Module)
            {
                var element = sender as Module;
                SchemeField.Children.Remove(element.LineBusDraw);
                Kernel.TtProgram.Modules.Remove(element._mrRemot);
                SchemeField.Children.Remove(element);
            }
        }

        private void ModuleOnUp(object sender, EventArgs e)
        {
            if (sender is Module)
            {
                _tmpMoveBus = null;
            }
        }

        private void ModuleOnDown(object sender, EventArgs e)
        {
            if (sender is Module)
            {
                _tmpMoveBus = sender as Module;
            }
        }
        #endregion
        //--------------------------------------------------------------- 
        #region Работа с шинами
        private void InsertBus(object sender, RoutedEventArgs e)
        {
            if (Kernel.TtProgram.Bus.Count >= 8)
            {
                MessageBox.Show("Нельзя создать более 8 шин");
                return;
            }
            var bs = new Bus();
            if (bs.BusDialogSetting() == false) return;bs.BusID = GenerateUnicaleIDBus();
            var bc = new BusControl(bs);
            Kernel.TtProgram.Bus.Add(bs);
            SchemeField.Children.Add(bc);
         
        }

        private void BusRemove(object sender, EventArgs e)
        {
            if (sender is BusControl)
            {
                var element = sender as BusControl;
                var BusID = element.GeBus.BusID;
                element.GeBus.Stop = true;
                Kernel.TtProgram.Bus.Remove(element.GeBus);
                SchemeField.Children.Remove(element);
                // TODO Добавить сюда удалить всех связанных модулей которые пренадлежат шины
                var busModules = GetModulesByBusID(BusID);
                foreach (var busModule in busModules)
                {
                    Kernel.TtProgram.Modules.Remove(busModule._mrRemot);
                    SchemeField.Children.Remove(busModule.LineBusDraw);
                    SchemeField.Children.Remove(busModule);
                }
            }
        }

        private void BusMouseUp(object sender, EventArgs e)
        {
            if (sender is BusControl)
            {
                _tmpMoveBus = null;
            }
        }

        private void BusMouseDown(object sender, EventArgs e)
        {
            if (sender is BusControl)
            {
                _tmpMoveBus = sender as BusControl;
            }
        }
        #endregion
        //--------------------------------------------------------------- 
        #region Работа с визуальным полем
        private void BuldField()
        {
            foreach (var bs in Kernel.TtProgram.Bus)
            {
                var bc = new BusControl(bs);
                var mrcp = bc.Margin;
                mrcp.Top = bs.HeighVisual;
                bc.Margin = mrcp;
                SchemeField.Children.Add(bc);
            }

            for (int i=0;i< Kernel.TtProgram.Modules.Count;i++)
            {
                var modules = Kernel.TtProgram.Modules[i];

                var errCoordinate = modules.CPoint;

                if (errCoordinate.X < 0) errCoordinate.X = 100;
                if (errCoordinate.Y < 0) errCoordinate.Y = 100;

                modules.CPoint = errCoordinate;

                var newVe = new Module(modules, modules.CPoint, false);

                var bcontrol = GetBysessByBusID(modules.BusId);

                var ln = new Line
                {
                    StrokeThickness = 8,
                    Stroke =
                        (SolidColorBrush) new BrushConverter().ConvertFromString(bcontrol.GeBus.BusProperties.Color)
                };

                ln.X1 = ln.X2 = newVe.Margin.Left + 50;
                ln.Y1 = bcontrol.Margin.Top + 30;
                ln.Y2 = newVe.Margin.Top;
                newVe.LineBusDraw = ln;
                SchemeField.Children.Add(ln);

                SchemeField.Children.Add(newVe);
            }
        }

        private void ClearField()
        {
             SchemeField.Children.Clear();
        }

        private void FieldMovie(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var posMouse = Mouse.GetPosition(SchemeField);
            XLabel.Text = Convert.ToString(posMouse.X);
            YLabel.Text = Convert.ToString(posMouse.Y);

            if (_tmpMoveBus is BusControl)
            {
                var ta = _tmpMoveBus as BusControl;
                var posMod = ta.Margin;
                posMod.Top = posMouse.Y;
                ta.Margin = posMod;
                List<Module> moveGroupe = GetModulesByBusID(ta.GeBus.BusID);
                foreach (Module t in moveGroupe)
                {
                    var move = t.LineBusDraw;
                    move.Y1 = posMouse.Y + 30;
                }
            }

            if (_tmpMoveBus is Module)
            {
                var ta = _tmpMoveBus as Module;
                var posMod = ta.Margin;
                posMod.Top = posMouse.Y-3;
                posMod.Left = posMouse.X-3;
                ta.Margin = posMod;

                int a = Kernel.TtProgram.Modules.IndexOf(ta._mrRemot);
                var line = ta.LineBusDraw;
                line.X1 = line.X2 = ta.Margin.Left+50;
                line.Y2 = ta.Margin.Top;
            }
        }
        #endregion
        //---------------------------------------------------------------
        #region Дополнительные утилиты

        private int GenerateUnicaleIDBus()
        {
            jump:
            var irandom = (int) new Random().Next(0, 8);
            foreach (var bus in Kernel.TtProgram.Bus)
            {
                if (bus.BusID == irandom) goto jump;
            }
            return irandom;
        }

        private int GenerateUnicaleIDModule()
        {
        jump:
            var irandom = (int)new Random().Next(1, 254);
        foreach (var module in Kernel.TtProgram.Modules)
            {
                if (module.ModuleId == irandom) goto jump;
            }
            return irandom;
        }

        private List<Module> GetModulesByBusID(int BusID)
        {
            var modules = SchemeField.Children.OfType<Module>().Select(element => element as Module).ToList();
            return modules.Where(module => module._mrRemot.BusId == BusID).ToList();
        }

        private BusControl GetBysessByBusID(int BusID)
        {
            var buses = SchemeField.Children.OfType<BusControl>().Select(element => element as BusControl).ToList();
            foreach (var busControl in buses)
            {
                if (busControl.GeBus.BusID == BusID) return busControl;
            }
            return null;
        }

        private List<Module> GetModulesControls()
        {
            return SchemeField.Children.OfType<Module>().Select(element => element as Module).ToList();
        }

        private List<BusControl> GetBusControsBy()
        {
            return  SchemeField.Children.OfType<BusControl>().Select(element => element as BusControl).ToList();       
        }

        #endregion
        //---------------------------------------------------------------
    }
}