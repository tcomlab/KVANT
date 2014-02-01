using System;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    class TI : LogicTemplate
    {
        public TI()
        {
            Content.Type = Type.TI;
            base.Dialog();
            Kernel._100msTick += MsTick;
        }
        public TI(ElementData data)
        {
            Content = data;
            Kernel._100msTick += MsTick;
        }


        private int prescaler = 0;
        private void MsTick(object sender, EventArgs eventArgs)
        {
            if (prescaler++ == 10)
            {
                prescaler = 0;
                if (_allEnable) // Ядро активировано начинаем отсчёт
                {
                    if (Content.CurrentValueSInt == 0)
                    {
                        Content.BlockExec1Bool = true;
                    }
                    else
                    {
                        Content.CurrentValueSInt--;
                        Content.BlockExec1Bool = false;}
                }
                else
                {
                    Content.CurrentValueSInt = Content.Val[0];
                    Content.BlockExec1Bool = false;
                }

            }
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
            return true;
        }
    }
}
