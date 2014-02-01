using System;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    sealed class DI : LogicTemplate
    {
        //-----------------------------------------------------
        public DI()
        {
            Content.Type = Type.DI;
            Dialog();
        }

        public DI(ElementData data)
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

        void _localModule_DataChenge(object sender, EventArgs e)
        {
            try
            {
                if (_localModule.Data == null) return;
                Content.CurrentValueBool = ((bool[])_localModule.Data)[Content.DeviceBinding.Chanel - 1];
                Content.BlockExec1Bool = Content.CurrentValueBool;
            }
            catch (Exception)
            {
            }
            
        }

        // Выполняется когда активна цепь
        public override void EnableLineSignal(bool state)
        {
            _allEnable = state;
        }

        // Выполняется каждую интерацию ядра
        public override bool ElementExecuted()
        {
            return Content.CurrentValueBool;
        }
        //-------------------------------------------------------
    }
}
