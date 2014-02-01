namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    public class Ii : LogicTemplate
    {
        public Ii()
        {
            Content.Type = Type.II;
            base.Dialog();
            _exec_ok = true;
        }

        public bool State
        {
            get { return _allEnable; }
        }
  
        public Ii(ElementData data)
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
            Content.BlockExec1Bool = state;
        }

        // Выполняется каждую интерацию ядра
        public override bool ElementExecuted()
        {
            return true;
        }
    }
}
