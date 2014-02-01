using System;
using System.ComponentModel;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    public enum Type
    {
        [Description("DI")]
        DI,
        [Description("DO")]
        DO,
        [Description("EN")]
        EN,
        [Description("DT")]
        DT,
        [Description("AI")]
        AI,
        [Description("BT")]
        BT,
        [Description("TI")]
        TI,
        [Description("RT")]
        RT,
        [Description("II")]
        II,
        [Description("NN")]
        NN,
        [Description("MR")]
        MR,
        [Description("MW")]
        MW,
        [Description("KG")]
        KG
    }                                           

    public interface ILogicEngine
    {
        Type Type {get;}
        int X { get; }
        int Y { get; }
        int ID { get; }
        string Name { get; }
    }
    public interface ILogicProgramBlock // Связь с блоком логической программы
    {
        void Dialog();
        event EventHandler EnableBlock;
        bool IsTrue { get; }
    }

    public interface IVisualProgram // Используется при обмене с визуальным контролом 
    {
        string Name { get; }
        void ReciveDataFromControl(object sender, object e);      // >>>> Принимаем данные от контрола
        event EventHandler SendDataToControl;                     // >>>> Отправляем данные контролу
    }

    public interface IContentData
    {
        ElementData Content { set; get; }
    }

}
