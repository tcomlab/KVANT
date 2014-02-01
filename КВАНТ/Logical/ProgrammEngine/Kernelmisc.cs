

using КВАНТ.Logical.ProgrammEngine.array_l_element;

namespace КВАНТ.Logical.ProgrammEngine
{
    public enum KernelStatus
    {
        Stop,
        Start,
        Pause,
        Reset,
        Unknow
    }

    /// <summary>
    /// Интерфейс взаимодействия с ядром
    /// </summary>
    internal interface IKernel
    {
        /// <summary> Сигнал включения цепи когда на всех модулях ок = ture</summary>
        void EnableLineSignal(bool state);

        /// <summary> Выход в ядро - блок сработал</summary>
        bool ElementExecuted();

        /// <summary> Тип элемента программы</summary>
        Type Type { get; }
    }
}