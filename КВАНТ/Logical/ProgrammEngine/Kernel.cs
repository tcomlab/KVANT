using System;
using System.Threading;
using КВАНТ.ViewsFormsPages.FormView;
using Type = КВАНТ.Logical.ProgrammEngine.array_l_element.Type;

namespace КВАНТ.Logical.ProgrammEngine
{
    public static class Kernel
    {
        public static Project TtProgram { get; private set; }

        private static int _kernelReactionTime;
        private static bool Stop { set; get; }

        private static int[] _activeLines;

        public static EventHandler _100msTick;

        public static void SetProgramm(Project prog)
        {
            TtProgram = prog;
            _activeLines = clculate_element_in_row();
        }

        public static Project GetProgram()
        {
            return TtProgram;
        }

        public static int KernelReactionTime
        {
            set { _kernelReactionTime = value; }
            get { return _kernelReactionTime; }
        }

        private static KernelStatus _kernelStatus;
        public static KernelStatus KernelStatus
        {
            set { _kernelStatus = value; }
            get { return _kernelStatus; }
        }

        static Kernel()
        {
            MainWindow.StopSystem += StopSignal;
            Stop = false;
            _kernelReactionTime = 10;
            new Thread(Tick) { IsBackground = true }.Start();
            new Thread(KernelProcess) { IsBackground = true }.Start();
        }

        private static void StopSignal(object sender, EventArgs e)
        {
            Stop = true;
        }


        private static void Tick()
        {
            while (!Stop)
            {
                if (_100msTick != null)
                _100msTick(null, null);
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Считаем колическтво активных елементов в массиве
        /// </summary>
        /// <returns></returns>
        private static int[] clculate_element_in_row()
        {
            if (TtProgram == null) return null;
            if (TtProgram.ProgramModel == null) return null;
            var element = new int[TtProgram.ProgramModel.Y];
            for (var y = 0; y < TtProgram.ProgramModel.Y; y++)
            {
                for (var x = 0; x < TtProgram.ProgramModel.X; x++)
                {
                    if (TtProgram.ProgramModel.Element[x, y] == null) continue;
                    if (((IKernel)TtProgram.ProgramModel.Element[x, y]).Type != Type.NN)
                        element[y]++;
                }
            }
            return element;
        }

        private static void reset_blocks()
        {
            if (TtProgram == null) return;
            if (TtProgram.ProgramModel == null) return;
            for (var y = 0; y < TtProgram.ProgramModel.Y; y++)
            {
                for (var x = 0; x < TtProgram.ProgramModel.X; x++)
                {
                    if (TtProgram.ProgramModel.Element[x, y] == null) continue;
                    var element = (IKernel)TtProgram.ProgramModel.Element[x, y];
                    if (element == null) continue;
                    element.EnableLineSignal(false);
                }
            }
        }

        private static void KernelProcess(object obj)
        {
            var lines = new int[10];
            int linesCount = 0;
            while (!Stop)
            {
                try
                {
                    //----------Process controller ------------------------------------------------
                    switch (_kernelStatus)
                    {
                        case KernelStatus.Stop:
                            // TODO: Сделать вызов этот запускается только при старте
                            _activeLines = clculate_element_in_row();
                            reset_blocks();
                            Thread.Sleep(2000);
                            continue;
                        case KernelStatus.Start:
                            if (TtProgram.ProgramModel == null)
                            {
                                Thread.Sleep(1000);
                                continue;
                            }
                            break;
                        case KernelStatus.Pause:
                            Thread.Sleep(1000);
                            continue;
                        case KernelStatus.Reset:
                            // -- Reset context
                            KernelStatus = KernelStatus.Stop;
                            continue;
                        case KernelStatus.Unknow:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    //----------Process controller -------------------------------------------------
                    try
                    {
                        if (TtProgram.ProgramModel == null)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        lines[linesCount] = 0;

                        for (var x = 0; x < TtProgram.ProgramModel.X; x++)
                        {
                            if (TtProgram.ProgramModel.Element[x, linesCount] == null) continue;
                            var element = (IKernel)TtProgram.ProgramModel.Element[x, linesCount];
                            if (element == null) continue;
                            if (element.Type == Type.NN) continue;
                            if (element.ElementExecuted()) lines[linesCount]++;
                        }

                        if (lines[linesCount] == _activeLines[linesCount] && lines[linesCount] > 0)
                        {
                            for (var x = 0; x < TtProgram.ProgramModel.X; x++)
                            {
                                if (TtProgram.ProgramModel.Element[x, linesCount] == null) continue;
                                var element = (IKernel)TtProgram.ProgramModel.Element[x, linesCount];
                                if (element == null) continue;
                                element.EnableLineSignal(true);
                            }
                        }
                        else
                        {
                            for (var x = 0; x < TtProgram.ProgramModel.X; x++)
                            {
                                if (TtProgram.ProgramModel.Element[x, linesCount] == null) continue;
                                var element = (IKernel)TtProgram.ProgramModel.Element[x, linesCount];
                                if (element == null) continue;
                                element.EnableLineSignal(false);
                            }
                        }

                        if (linesCount++ == TtProgram.ProgramModel.Y - 1)
                            linesCount = 0;
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                    //---------------------------------------
                    Thread.Sleep(_kernelReactionTime);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {

                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
