using System;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    class DT : LogicTemplate
    {
        public DT()
        {
            Content.Type = Type.DT;
            Dialog();
        }

        public DT(ElementData data)
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
                        _localModule.DataChenge += _localModule_DataChenge;return;
                    }
                }
                
            }
            catch (Exception)
            {

            }
        
        }

        private bool state = false;
        private void _localModule_DataChenge(object sender, EventArgs e)
        {
            Content.CurrentValueDouble = ((float[])_localModule.Data)[Content.DeviceBinding.Chanel];

            var usl = (Uslovie)Content.MiscObject[0];

            switch (usl)
            {
                case Uslovie.Bolshe:
                    if (Content.CurrentValueDouble < Content.Val[0]) state = true; else state = false;
                    break;
                case Uslovie.Ravno:
                    if (Content.CurrentValueDouble == Content.Val[0]) state = true; else state = false;
                    break;
                case Uslovie.Menshe:
                    if (Content.CurrentValueDouble > Content.Val[0]) state = true; else state = false;
                    break;
            }
            Content.BlockExec1Bool = state;
        }

        // Выполняется когда активна цепь
        public override void EnableLineSignal(bool state)
        {
            _allEnable = state;
        }

        // Выполняется каждую интерацию ядра
        public override bool ElementExecuted()
        {
            return state;
        }
    }
}
