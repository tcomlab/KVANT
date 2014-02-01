using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using КВАНТ.Logical.Engine;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    public class EargToVisual : EventArgs
    {
        public object Data { set; get; }

        public EargToVisual(object data)
        {
            Data = data;
        }

        public EargToVisual()
        {
            Data = null;
        }
    }

    public class LogicTemplate : ILogicEngine, IKernel, ILogicProgramBlock, IVisualProgram, IContentData
    {
        public int X { get { return Content.X; } set { Content.X = value; } }
        public int Y { get { return Content.Y; } set { Content.Y = value; } }
        public int ID { get { return X * Y; } }
        public Type Type { get { return Content.Type; } }
        public string Name { get { return _content.Name; } }
        //-----------------------------------------------
        internal ElementData _content;

        internal bool _exec_ok = false;
        internal bool _allEnable = false;

        public ElementData Content
        {
            set
            {
                _content = value;
                DataContextChange();
            }
            get
            {
                return _content;
            }
        }

        internal virtual void DataContextChange()
        {

        }

        // IKernel --------------------------------------
        public virtual void EnableLineSignal(bool state)
        {
            _allEnable = state;
            Content.BlockExec2Bool = state;
        }

        public virtual bool ElementExecuted()
        {
            return _exec_ok;
        }

        //-----------------------------------------------
        // Создание нового елемента
        public LogicTemplate()
        {
            Content = new ElementData() { Type = Type.NN };
        }

        // Востановление из сохраненного документа
        public LogicTemplate(ElementData contData)
        {
            Content = contData;
        }

        //---------------------------------------------------------
        // Отсылаем евент графическому представлению программы
        public event EventHandler EnableBlock;
        public bool IsTrue { get; private set; }

        protected virtual void OnEnableBlock(Brush color)
        {
            EventHandler handler = EnableBlock;
            if (handler != null) handler(this, new EArg() { ColorBrushes = color });
        }

        //---------------------------------------------------------
        // Событейно ориентировочный интерфейс обменна данными
        public event EventHandler SendDataToControl;

        internal virtual void OnSendDataToControl(EargToVisual eventArgs)
        {
            EventHandler handler = SendDataToControl;
            if (handler != null) handler(this, eventArgs);
        }


        // Принимаем данные от графического элемента
        public virtual void ReciveDataFromControl(object sender, object e)
        {

        }

        //---------------------------------------------------
        public virtual void Dialog()
        {
            var dialog = new Window();
            var modules = new List<ModuleRemot>();
            switch (Type)
            {
                case Type.DI:
                    modules = Kernel.GetProgram().Modules.Where(moduleRemot => moduleRemot.Type == Type).ToList();
                    if (modules.Count == 0)
                    {
                        MessageBox.Show("Нет сконфигурированных модулей", "Внимание", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }dialog = new DIDialog(Content, modules);
                    break;
                case Type.DO:
                    modules = Kernel.GetProgram().Modules.Where(moduleRemot => moduleRemot.Type == Type).ToList();
                    if (modules.Count == 0)
                    {
                        MessageBox.Show("Нет сконфигурированных модулей", "Внимание", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                    dialog = new DODialog(Content, modules);
                    break;
                case Type.EN:
                    modules = Kernel.GetProgram().Modules.Where(moduleRemot => moduleRemot.Type == Type).ToList();
                    if (modules.Count == 0)
                    {
                        MessageBox.Show("Нет сконфигурированных модулей", "Внимание", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                    dialog = new ENDialog(Content, modules);
                    break;
                case Type.DT:
                    modules = Kernel.GetProgram().Modules.Where(moduleRemot => moduleRemot.Type == Type).ToList();
                    if (modules.Count == 0)
                    {
                        MessageBox.Show("Нет сконфигурированных модулей", "Внимание", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                    dialog = new DTDialog(Content, modules);
                    break;
                case Type.AI:
                    modules = Kernel.GetProgram().Modules.Where(moduleRemot => moduleRemot.Type == Type).ToList();
                    if (modules.Count == 0)
                    {
                        MessageBox.Show("Нет сконфигурированных модулей", "Внимание", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                    dialog = new AiDialog(Content, modules);
                    break;
                case Type.BT:
                    dialog = new BTDialog(Content, modules);
                    break;
                case Type.TI:
                    dialog = new TIDialog(Content, modules);
                    break;
                case Type.RT:
                    dialog = new RtDialog(Content, modules);
                    break;
                case Type.II:
                    dialog = new IiDialog(Content, modules);
                    break;
                case Type.NN:
                    //dialog = new NNDialog(Content, modules); 
                    break;
                case Type.MR:
                    dialog = new MRDialog(Content, modules);
                    break;
                case Type.MW:
                    dialog = new MWDialog(Content, modules);
                    break;
                case Type.KG:
                    dialog = new KGDialog(Content, modules);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            dialog.ShowDialog();

            if (dialog.DialogResult == true)
            {
                var tt = ((IModuleDialogInterface)dialog).ContentData;
                Content = tt;
                IsTrue = true;
            }
            else
            {
                IsTrue = false;
            }
        }
    }
}
