using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using КВАНТ.Logical.ProgrammEngine;
using КВАНТ.Logical.ProgrammEngine.array_l_element;
using КВАНТ.ViewsFormsPages.FormView;
using КВАНТ.ViewsFormsPages.Pages.LogicProgram;
using КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls;
using AI = КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls.AI;
using BT = КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls.BT;
using DI = КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls.DI;
using DO = КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls.DO;
using DT = КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls.DT;
using EN = КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls.EN;
using II = КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls.II;
using RT = КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls.RT;
using TI = КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls.TI;
using Type = КВАНТ.Logical.ProgrammEngine.array_l_element.Type;

namespace КВАНТ.ViewsFormsPages.Pages.VisualScreen
{
    /// <summary>
    /// Interaction logic for VisualProgramm.xaml
    /// </summary>
    public partial class VisualProgramm : UserControl
    {
        private bool builded = false;
        public VisualProgramm()
        {
            InitializeComponent();
            this.Unloaded += VisualProgramm_Unloaded;
            if (!builded) BuldVisual();
            MainWindow.StopSystem += StopSygnal;
        }

        private void StopSygnal(object sender, EventArgs e)
        {
            CreateView();
        }

        void CreateView()
        {
            Kernel.GetProgram().Visual.Clear();
            for (int i = 0; i < RootStackPanel.Children.Count; i++)
            {
                var arg = (IControlVisual)RootStackPanel.Children[i];
                Kernel.GetProgram().Visual.Add(arg.VElement);
            }
        }

        void BuldVisual()
        {
            builded = true;
            RootStackPanel.Children.Clear();
            foreach (var t in Kernel.GetProgram().Visual)
            {
                var element = Kernel.GetProgram().ProgramModel.Element[t.X, t.Y];
                UserControl controlElement = null;
                var ieng = (ILogicEngine) element;
                var p = new Point(t.Position.X, t.Position.Y);

                switch (ieng.Type)
                {
                    case Type.DI: controlElement = new DI(element, p, false);
                        break;
                    case Type.DO: controlElement = new DO(element, p, false);
                        break;
                    case Type.EN: controlElement = new EN(element, p, false);
                        break;
                    case Type.DT: controlElement = new DT(element, p, false);
                        break;
                    case Type.AI: controlElement = new AI(element, p, false);
                        break;
                    case Type.BT: controlElement = new BT(element, p, false);
                        break;
                    case Type.TI: controlElement = new TI(element, p, false);
                        break;
                    case Type.RT: controlElement = new RT(element, p, false);
                        break;
                    case Type.II: controlElement = new II(element, p, false);
                        break;
                    case Type.NN:
                        return;
                }

                if (controlElement == null) return;
                var ev = (IControlVisual)controlElement;
                ev.ModuleOnDown += dt_MouseLeftButtonDown;
                ev.ModuleOnUp += dt_MouseLeftButtonUp;
                ev.DeleteElement += ev_DeleteElement;
                controlElement.VerticalAlignment = VerticalAlignment.Top;
                controlElement.HorizontalAlignment = HorizontalAlignment.Left;
                RootStackPanel.Children.Add(controlElement);
            }

            //Kernel.GetProgram().Visual.Clear();
        }


        void VisualProgramm_Unloaded(object sender, RoutedEventArgs e)
        {
            CreateView();
            var vr = (LogicalProgram)Frame1.Content;
            if (vr == null) return;
            
            vr.SaveProgram();
        }

        private void RootStackPanel_OnDrop(object sender, DragEventArgs e)
        {
            var dd = (Menuelement)e.Data.GetData("DragSource");
            // Выводим диалог выбора типа привязки
            var arr = Kernel.GetProgram().ProgramModel.GetListOfType(dd.BlockType);
            if (arr.Count == 0) return;
            var dialog = new SelectNameDialog(arr);
            dialog.ShowDialog();
            if (dialog.DialogResult == false)
            {
                return;
            }
            var element = arr[dialog.NameIndex];
            UserControl controlElement = null;
            var r = new Random();
            int xp = r.Next(0, 200); //for ints
            int yp = r.Next(0, 200); //for ints
            switch (dd.BlockType)
            {
                case Type.DI: controlElement = new DI(element, new Point(xp, yp),true);
                    break;
                case Type.DO: controlElement = new DO(element, new Point(xp, yp), true);
                    break;
                case Type.EN: controlElement = new EN(element, new Point(xp, yp), true);
                    break;
                case Type.DT: controlElement = new DT(element, new Point(xp, yp), true);
                    break;
                case Type.AI: controlElement = new AI(element, new Point(xp, yp), true);
                    break;
                case Type.BT: controlElement = new BT(element, new Point(xp, yp), true);
                    break;
                case Type.TI: controlElement = new TI(element, new Point(xp, yp), true);
                    break;
                case Type.RT: controlElement = new RT(element, new Point(xp, yp), true);
                    break;
                case Type.II: controlElement = new II(element, new Point(xp, yp), true);
                    break;
                case Type.NN:
                    break;
            }

            if (controlElement == null) return;
            var ev = (IControlVisual)controlElement;
            ev.ModuleOnDown += dt_MouseLeftButtonDown;
            ev.ModuleOnUp += dt_MouseLeftButtonUp;
            ev.DeleteElement += ev_DeleteElement;
            controlElement.VerticalAlignment = VerticalAlignment.Top;
            controlElement.HorizontalAlignment = HorizontalAlignment.Left;
            RootStackPanel.Children.Add(controlElement);
        }


        void ev_DeleteElement(object sender, EventArgs e)
        {
            var eel = (UIElement) sender;
            var ev = (IControlVisual)sender;
            ev.ModuleOnDown -= dt_MouseLeftButtonDown;
            ev.ModuleOnUp -= dt_MouseLeftButtonUp;
            ev.DeleteElement -= ev_DeleteElement;
            RootStackPanel.Children.Remove(eel);
        }

        private UIElement _el;

        private void dt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _el = null;
        }

        void dt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _el = (UIElement)sender;
        }
        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var dragSource = (Menuelement) sender;
            var data = dragSource.BlockType;
            var dataObj = new DataObject(data);
            dataObj.SetData("DragSource", dragSource);
            DragDrop.DoDragDrop(dragSource, dataObj, DragDropEffects.Copy);
        }

        private void RootStackPanel_OnMouseMove(object sender, MouseEventArgs e)
        {
            var spanel = (Grid) sender;
            var pos = this.PointToScreen(Mouse.GetPosition(this)); //position relative to screen
            var pos2 = Mouse.GetPosition(spanel);
            //PositionLabel.Content = String.Format("X:{0} - Y:{1}", pos2.X, pos2.Y);
            if (_el == null) return;
            //LabelZones.Content = String.Format("X:{0} - Y:{1}", pos2.X, pos2.Y);
            var viel = (IControlVisual)_el;
            var p = (Control)_el;
            var m = p.Margin;
            m.Left = pos2.X;
            m.Top = pos2.Y;
            p.Margin = m;
            ((IControlVisual)_el).VElement.Position = new Point((int) pos2.X, (int) pos2.Y);
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e) // Закрываем
        {
            var vr = (LogicalProgram)Frame1.Content;
            vr.SaveProgram();
        }

        private void EditAll(object sender, RoutedEventArgs e)
        {
            foreach (var element in RootStackPanel.Children)
            {
                if (element is IControlVisual)
                {
                    var rc = element as IControlVisual;
                    var state = ((CheckBox) sender).IsChecked;
                    if (state != null) rc.SetEdit = state.Value;
                }
            }
        }
    }
}
