using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using КВАНТ.Logical.ProgrammEngine;
using КВАНТ.Logical.ProgrammEngine.array_l_element;
using КВАНТ.ViewsFormsPages.Pages.LogicProgram.Controls;
using Type = КВАНТ.Logical.ProgrammEngine.array_l_element.Type;

namespace КВАНТ.ViewsFormsPages.Pages.LogicProgram
{/// <summary>
    /// Interaction logic for LogicalProgram.xaml
    /// </summary>
    public partial class LogicalProgram : UserControl
    {
        private Project _project;

        public LogicalProgram()
        {
            InitializeComponent();
            this.Unloaded += LogicalProgram_Unloaded;
            this.Loaded += LogicalProgram_Loaded;
        }

        void LogicalProgram_Loaded(object sender, RoutedEventArgs e)
        {
            ElementGrid.Children.Clear(); // TODO !!
            _project = Kernel.GetProgram();
            build_array();
        }

        void LogicalProgram_Unloaded(object sender, RoutedEventArgs e)
        {
            SaveProgram();
        }

        public void SaveProgram()
        {
            if (_project == null) return;
            Kernel.SetProgramm(_project);
            unload_event();
        }

        private void unload_event()
        {
            if (_project == null) return;

            for (int i = 0; i < ElementGrid.Children.Count; i++)
            {
                var el = (ProgramBlock)ElementGrid.Children[i];
                if (el == null) continue;
                el.Drop -= PbOnDrop;
                el.MouseDown -= pb_MouseDown;
            }

        }

        private void build_array()
        {
            if (_project == null) return;

            for (int y = 0; y < _project.ProgramModel.Y; y++)
            {
                ElementGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Pixel) });
                for (int x = 0; x < _project.ProgramModel.X; x++)
                {
                    ElementGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100, GridUnitType.Pixel) });
                    var pb = new ProgramBlock(_project.ProgramModel.Element[x, y], x, y);
                    pb.DeleteSelf += pb_DeleteSelf;
                    pb.Drop += PbOnDrop;
                    pb.MouseDown += pb_MouseDown;
                    pb.Margin = new Thickness(5);
                    ElementGrid.Children.Add(pb);
                    Grid.SetColumn(pb, x);
                    Grid.SetRow(pb, y);

                }
            }
        }

        void pb_DeleteSelf(object sender, EventArgs e)
        {
            var db = (ProgramBlock)sender;
            var x = db.X;
            var y = db.Y;
            db.DeleteSelf -= pb_DeleteSelf;
            db.Drop -= PbOnDrop;
            db.MouseDown -= pb_MouseDown;
            ElementGrid.Children.Remove(db);
            _project.ProgramModel.Element[x, y] = new Dummy();

            var pb = new ProgramBlock(_project.ProgramModel.Element[y, x], y, x);
            pb.DeleteSelf += pb_DeleteSelf;
            pb.Drop += PbOnDrop;
            pb.MouseDown += pb_MouseDown;
            pb.Margin = new Thickness(5);
            ElementGrid.Children.Add(pb);
            Grid.SetColumn(pb, x);
            Grid.SetRow(pb, y);

        }


        void pb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed) return;
            try // TODO костыль номер раз
            {
                var dragSource = (ProgramBlock)sender;
                if (dragSource.Type == Type.NN) return;
                var data = dragSource.Type;
                var dataObj = new DataObject(data);
                dataObj.SetData("DragSource", dragSource);
                DragDrop.DoDragDrop(dragSource, dataObj, DragDropEffects.Copy);
            }
            catch (Exception)
            {
            }
        }

        private void PbOnDrop(object sender, DragEventArgs dragEventArgs)
        {
            try
            {
                var dragtypeblock = (ProgramBlock)dragEventArgs.Data.GetData("DragSource"); // Узнаем какого типа пользователь хочет сосдать блок
                int x = ((ProgramBlock)sender).X;
                int y = ((ProgramBlock)sender).Y;
                
                object template = null;
                switch (dragtypeblock.Type)//TODO разобратся почему здесь подмешивает левый тип
                {
                    case Type.DI: template = new DI(); break;
                    case Type.DO: template = new DO(); break;
                    case Type.EN: template = new EN(); break;
                    case Type.DT: template = new DT(); break;
                    case Type.AI: template = new Ai(); break;
                    case Type.BT: template = new BT(); break;
                    case Type.TI: template = new TI(); break;
                    case Type.RT: template = new RT(); break;
                    case Type.II: template = new Ii(); break;
                    case Type.NN: break;
                    case Type.MR: template = new MR(); break;
                    case Type.MW: template = new MW(); break;
                    case Type.KG: template = new KG(); break;
                    default: throw new ArgumentOutOfRangeException();
                }

                var logicProgramBlock = (ILogicProgramBlock)template;
                var res = logicProgramBlock != null && logicProgramBlock.IsTrue;
                if (res == false) return;
                if (template == null) return;
                var current = new ProgramBlock(template, x, y);
                _project.ProgramModel.Element[x, y] = template;
                current.Margin = new Thickness(5);
                ElementGrid.Children.Remove((ProgramBlock)sender);
                ElementGrid.Children.Add(current);
                Grid.SetColumn(current, x);
                Grid.SetRow(current, y);
            }
            catch (Exception)
            {

            }

        }

        private void click_drop(object sender, MouseButtonEventArgs e)
        {
            var dragSource = (ProgramBlock)sender;
            var data = dragSource.Type;
            var dataObj = new DataObject(data);
            dataObj.SetData("DragSource", dragSource);
            try
            {
                DragDrop.DoDragDrop(dragSource, dataObj, DragDropEffects.Copy);
            }
            catch (Exception)
            {
            }

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var tag = (String)((RadioButton)sender).Tag;
            switch (tag)
            {
                case "Stop":
                    Kernel.KernelStatus = KernelStatus.Stop;
                    break;
                case "Run": Kernel.KernelStatus = KernelStatus.Start;
                    break;
                case "Pause": Kernel.KernelStatus = KernelStatus.Pause;
                    break;
            }
        }

        public class SaveHelper
        {
            public int x { set; get; }
            public int y { set; get; }
            public SaveHelper()
            {
                x = 0;
                y = 0;
            }

            public SaveHelper(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

    }
}
