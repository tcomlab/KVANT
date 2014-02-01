using System;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    [Serializable]
    public class Dummy : LogicTemplate
    {
        public Dummy()
        {
            _exec_ok = true;
            Content.Name = "--------";
            Content.Type =Type.NN;
        }

    }
}
