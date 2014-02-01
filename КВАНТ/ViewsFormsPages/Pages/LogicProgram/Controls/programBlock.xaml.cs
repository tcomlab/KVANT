using System;
using System.Windows;
using System.Windows.Controls;
using КВАНТ.Logical.ProgrammEngine.array_l_element;
using Brush = System.Windows.Media.Brush;

namespace КВАНТ.ViewsFormsPages.Pages.LogicProgram.Controls
{
    /// <summary>
    /// Interaction logic for programBlock.xaml
    /// </summary>
    public partial class ProgramBlock : UserControl
    {
        private readonly ILogicProgramBlock _container;
        private readonly ElementData _content;
        public int X { get { return _content.X; } }
        public int Y { get { return _content.Y; } }
        internal КВАНТ.Logical.ProgrammEngine.array_l_element.Type Type { set { _content.Type = value; } get {return _content.Type; } }

        public event EventHandler DeleteSelf;

        protected virtual void OnDeleteSelf()
        {
            EventHandler handler = DeleteSelf;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public ProgramBlock(object content,int x, int y)
        {
            InitializeComponent(); 
            AllowDrop = true;
            _content = ((IContentData)content).Content;
            _container = (ILogicProgramBlock)content;
            DataContext = _content;
            _content.X = x;
            _content.Y = y;
        }

        public ProgramBlock()
        {
            InitializeComponent();
            AllowDrop = true;
            _content = new ElementData();
            _container = null;
            DataContext = _content;
        }

        internal String StickText
        {
            set { LabelStick.Content = value; }
            get { return (string)LabelStick.Content; }
        }

        internal String BlockText
        {
            set { MainText.Text = value; }
            get { return MainText.Text; }
        }

        internal Brush FillColor
        {
            set { RectangleMaine.Fill = value; }
            get { return RectangleMaine.Fill; }
        }
        
        private void Setting(object sender, RoutedEventArgs e)
        {
            if (_container == null) return;
            if (_container is Dummy) return;
            _container.Dialog();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (_container == null) return;
            if (_container is Dummy) return;
            OnDeleteSelf();
        }
    }

}
