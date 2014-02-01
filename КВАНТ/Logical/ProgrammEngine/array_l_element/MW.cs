namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    public class MW : LogicTemplate
    {
        public MW()
        {
            Content.Type = Type.MW;
            base.Dialog();
        }

        private ElementData _parent;

        public MW(ElementData data)
        {
            Content = data;
        }

        internal override void DataContextChange()
        {
            if (Content.MiscObject[1] == null) return;
            _parent = (ElementData) Content.MiscObject[1]; // Указывает на читающую ячейку
        }

        // Выполняется когда активна цепь
        public override void EnableLineSignal(bool state)
        {
            if (_allEnable == state) return;
            _allEnable = state;
            if (state) 
                _parent.Val[0] = (int)Content.DoubleData[0];
            // TODO: Нужно сюда вставить трейс события к визуальному контролу
        }

        // Выполняется каждую интерацию ядра
        public override bool ElementExecuted()
        {
            return true;
        }
    }
}
