using System.Windows.Controls;
using System.Windows.Media;
using Type = КВАНТ.Logical.ProgrammEngine.array_l_element.Type;

namespace КВАНТ.ViewsFormsPages.Pages.VisualScreen.Controls
{
    

    /// <summary>
    /// Interaction logic for Menuelement.xaml
    /// </summary>
    public partial class Menuelement : UserControl
    {
        public Brush BackgroundColor { set { Rectangle1.Fill = value; } get { return Rectangle1.Fill; } }

        public Menuelement()
        {
            InitializeComponent();
        }

        public string textBlockLabel
        {
            set { TextBlockLabel.Text = value; }
            get { return TextBlockLabel.Text; }
        }

        private Type block_t = Type.NN;

        public Type BlockType
        {
            set { block_t = value; }
            get { return block_t; }
        }

    }
}
