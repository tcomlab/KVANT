using System;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    internal class RT : LogicTemplate
    {
        public RT()
        {
            Content.Type = Type.RT;
            base.Dialog();
        }

        public RT(ElementData data)
        {
            Content = data;
        }

        // Выполняется когда активна цепь
        public override void EnableLineSignal(bool state)
        {
            _allEnable = state;
            Content.MiscObject[2] = DateTime.Now.TimeOfDay;
        }

        // Выполняется каждую интерацию ядра
        public override bool ElementExecuted()
        {
            var znak = (int) Content.MiscObject[0];
            var pTime = ((DateTime)Content.MiscObject[1]).TimeOfDay;
            var current = DateTime.Now.TimeOfDay;
            
            var res = current.CompareTo(pTime);

            if (znak == 0)
            {
                if (res == 1)
                {
                    Content.BlockExec1Bool = true;
                    return true;
                }
                else
                {
                    Content.BlockExec1Bool = false;
                    return false;
                }
            }
            else
            {
                if (res == -1)
                {
                    Content.BlockExec1Bool = true;
                    return true;
                }
                else
                {
                    Content.BlockExec1Bool = false;
                    return false;
                }
            }
        }
    }
}
