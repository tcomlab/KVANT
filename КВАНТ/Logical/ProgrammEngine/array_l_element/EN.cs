using System;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    internal class EN : LogicTemplate
    {
        public EN()
        {
            Content.Type = Type.EN;
            Dialog();
        }

        public EN(ElementData data)
        {
            Content = data;
        }

        private ModuleRemot _localModule;

        internal override void DataContextChange()
        {
            if (Kernel.TtProgram == null) return;
            try
            {
                for (int i = 0; i < Kernel.TtProgram.Modules.Count; i++)
                {
                    if (Content.DeviceBinding.ModuleId == Kernel.TtProgram.Modules[i].ModuleId)
                    {
                        _localModule = Kernel.TtProgram.Modules[i];
                        _localModule.DataChenge += _localModule_DataChenge;
                        return;
                    }
                }        
            }
            catch (Exception)
            {

            }
            
        }


        private bool _state = false;
        void _localModule_DataChenge(object sender, EventArgs e)
        {
            if (_localModule.Data == null) return;
            Content.CurrentValueSInt = (Int32)_localModule.Data;
            var usl = (Uslovie)Content.MiscObject[0];

            switch (usl)
            {
                case Uslovie.Bolshe:
                    if (Content.CurrentValueSInt < Content.DoubleData[0]) _state = true;
                    else _state = false;
                    break;
                case Uslovie.Menshe:
                    if (Content.CurrentValueSInt > Content.DoubleData[0]) _state = true;
                    else _state = false;
                    break;
            }
            Content.BlockExec1Bool = _state;
            OnSendDataToControl(new EargToVisual(_state));
        }

        // Выполняется когда активна цепь
        public override void EnableLineSignal(bool state)
        {
            _allEnable = state;
        }

        // Выполняется каждую интерацию ядра
        public override bool ElementExecuted()
        {return _state;
        }
    }
}
