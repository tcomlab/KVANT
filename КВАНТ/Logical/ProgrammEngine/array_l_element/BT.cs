using System.Diagnostics;


namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    public enum Function { Nc, No, TNc, TNo }

    public class BT : LogicTemplate
    {
        //------------------------------------------------
        public enum ButtonState  {  NoPressed,Pressed  }
        public ButtonState BState { set; get; }
        //------------------------------------------------
        public BT()
        {
            Content.Type = Type.BT;
            Dialog();
        }

        public BT(ElementData data)
        {
            Content = data;
        }

        private bool togle = false;
        public override void ReciveDataFromControl(object sender, object e)
        {
            BState = (BT.ButtonState)e;
            Trace.WriteLine(BState);
            var st = (Function)Content.MiscObject[0];

            if (togle)
            {
                togle = false;
            }
            else togle = true;

            if (st == Function.TNc || st == Function.TNo)
            {
                if (st == Function.TNc) _exec_ok = !togle;
                if (st == Function.TNo) _exec_ok = togle;
            }
            else
            {
                switch (BState)
                {
                    case BT.ButtonState.NoPressed:
                        if (st == Function.No) _exec_ok = true;
                        if (st == Function.Nc) _exec_ok = false;
                        break;

                    case BT.ButtonState.Pressed:
                        if (st == Function.No) _exec_ok = false;
                        if (st == Function.Nc) _exec_ok = true;
                        break;
                }
            }

            Content.BlockExec1Bool = _exec_ok;
        }
    }
}
