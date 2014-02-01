using System;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    public enum Uslovie
    {
        Bolshe, Ravno, Menshe
    }

    public sealed class Ai : LogicTemplate
    {
        public Ai()
        {
            Content.Type = Type.AI;
            Dialog();
        }

        public Ai(ElementData data)
        {
            Content = data;
        }

        private ModuleRemot _localModule;

        internal override void DataContextChange()
        {
            if (Kernel.TtProgram == null) return;
            try
            {
                foreach (var t in Kernel.TtProgram.Modules)
                {
                    if (Content.DeviceBinding.ModuleId == t.ModuleId)
                    {
                        _localModule = t;
                        _localModule.DataChenge += _localModule_DataChenge;
                        return;
                    }
                }
                
            }
            catch (Exception)
            {

            }

        }

        bool _state = false;
        private void _localModule_DataChenge(object sender, EventArgs e)
        {
            if (_localModule.Data == null) return;
            Content.CurrentValueDouble = ((short[])_localModule.Data)[Content.DeviceBinding.Chanel];

            var usl = (Uslovie)Content.MiscObject[0];
            switch (usl)
            {
                case Uslovie.Bolshe:
                    if (Content.CurrentValueDouble < Content.Val[0]) _state = true; else _state = false;
                    break;
                case Uslovie.Ravno:
                    if (Content.CurrentValueDouble == Content.Val[0]) _state = true; else _state = false;
                    break;
                case Uslovie.Menshe:
                    if (Content.CurrentValueDouble > Content.Val[0]) _state = true; else _state = false;
                    break;
            }
            Content.BlockExec1Bool = _state;
        }

        // Выполняется когда активна цепь
        public override void EnableLineSignal(bool state)
        {
            _allEnable = state;
            Content.BlockExec2Bool = state;
        }

        // Выполняется каждую интерацию ядра
        public override bool ElementExecuted()
        {
            return _state;
        }
    }
}
