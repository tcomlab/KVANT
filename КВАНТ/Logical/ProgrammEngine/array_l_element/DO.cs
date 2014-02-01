using System;
using КВАНТ.Logical.Engine;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    class DO : LogicTemplate
    {
        public DO()
        {
            Content.Type = Type.DO;
            Dialog();
        }

        public DO(ElementData data)
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
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        // Выполняется когда активна цепь
        public override void EnableLineSignal(bool state)
        {
            try
            {
                ((bool[])_localModule.Data)[Content.DeviceBinding.Chanel] = state;
                Content.BlockExec1Bool = state;
                Content.CurrentValueBool = state;
            }
            catch (Exception)
            {
   
            }
            
        }

        // Выполняется каждую интерацию ядра
        public override bool ElementExecuted()
        {
            return true;
        }
    }
}
