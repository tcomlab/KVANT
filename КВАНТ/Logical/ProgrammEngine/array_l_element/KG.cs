using System;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    public class KG : LogicTemplate
    {
        public KG()
        {
            Content.Type = Type.KG;
            base.Dialog();
        }

        public KG(ElementData data)
        {
            Content = data;
        }

        ElementData _parent;

        internal override void DataContextChange()
        {
            try
            {
                var spect = Content.MiscObject[1];
                if (spect == null) return;
                _parent = (ElementData) spect;
            }
            catch (Exception)
            {
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
            if (_parent == null) return false;
            var contact = (Function) Content.MiscObject[2];
            if (contact == Function.Nc)
                Content.BlockExec1Bool = _parent.BlockExec1Bool;
            else
                Content.BlockExec1Bool = !_parent.BlockExec1Bool;
            return _parent.BlockExec1Bool;
        }
    }
}
