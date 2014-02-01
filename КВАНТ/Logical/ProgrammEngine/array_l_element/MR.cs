namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    public class MR : LogicTemplate
    {
        public MR()
        {
            Content.Type = Type.MR;
            base.Dialog();
        }

        public MR(ElementData data)
        {
            Content = data;
        }

        internal override void DataContextChange()
        {

        }

        // Выполняется когда активна цепь
        public override void EnableLineSignal(bool state)
        {
            _allEnable = state;
            // TODO: Нужно сюда вставить трейс события к визуальному контролу
        }

        // Выполняется каждую интерацию ядра
        public override bool ElementExecuted()
        {
            var a = (Function)Content.MiscObject[1];
            var usl = (Uslovie)Content.MiscObject[0];

            switch (usl)
            {
                case Uslovie.Bolshe:
                    if (Content.Val[0] < Content.DoubleData[0]) Content.BlockExec1Bool = true;
                    else Content.BlockExec1Bool = false;
                    break;
                case Uslovie.Menshe:
                    if (Content.Val[0] > Content.DoubleData[0]) Content.BlockExec1Bool = true;
                    else Content.BlockExec1Bool = false;
                    break;
                case Uslovie.Ravno:
                    if (Content.Val[0] == (int)Content.DoubleData[0]) Content.BlockExec1Bool = true;
                    else Content.BlockExec1Bool = false;
                    break;
            }

            if (a == Function.Nc)
            {
                return Content.BlockExec1Bool;
            }
            else
            {
                return !Content.BlockExec1Bool;
            }
            
        }
    }
}
