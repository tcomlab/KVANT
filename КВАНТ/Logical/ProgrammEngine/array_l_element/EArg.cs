using System;
using System.Windows.Media;


namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    public class EArg : EventArgs
    {
        public bool State { set; get; }
        public Brush ColorBrushes { set; get; }
        public string Text { set; get; }
        public int Value1 { set; get; }
        public int Value2 { set; get; }
        public EArg()
        {
            State = false;
            ColorBrushes = Brushes.Transparent;
            Text = "";
            Value1 = 0;
            Value2 = 0;
        }
    }
}
