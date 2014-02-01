using System;
using System.ComponentModel;
using System.Threading;
using System.Xml.Serialization;
using eslelectronic;
using КВАНТ.Logical.Engine;
using КВАНТ.Logical.ProgrammEngine;
using КВАНТ.ViewsFormsPages.FormView;
using Type = КВАНТ.Logical.ProgrammEngine.array_l_element.Type;

namespace КВАНТ.Logical.Device
{
    // Здесь полность работаем с модулями ввода вывода
    public class Bus
    {
        public enum BusState
        {
            Stop,Run,Error,Dispatch
        }

        public static Project Project { set; get; }
        public  bool Stop { set; get; }
        private BusState _busState = BusState.Stop;
        private ModBus _mbData;
        private BusProperties _busProperties;

        public BusState State
        {
            get
            {
                return _busState;
            }
        }
        public Bus(BusProperties busProperties)
        {
            _busProperties = busProperties;
            MainWindow.StopSystem += stop_sygnal;
            Kernel._100msTick += MsTick;
            _slowModuleIntervaltimer = 0;
        }

        public Bus()
        {
            _busProperties = new BusProperties();
            MainWindow.StopSystem += stop_sygnal;
            Kernel._100msTick += MsTick;
            _slowModuleIntervaltimer = 0;
            new Thread(Process1){IsBackground = true}.Start();
        }

        public bool BusDialogSetting()
        {
            // TODO Тут нужно похимичить
            _busProperties.BusSettingDialog();
            return true;
        }

        private int _slowModuleIntervaltimer;

        private void MsTick(object sender, EventArgs eventArgs)
        {
            _slowModuleIntervaltimer++;
        }

        private void stop_sygnal(object sender, EventArgs e)
        {
            Stop = true;
            Kernel._100msTick -= MsTick;
        }

        [XmlElement("busProperties")]
        public BusProperties BusProperties { set { _busProperties = value; } get { return _busProperties; } }

        [XmlElement("BusID")]
        public int BusID { set; get; }

        [XmlElement("HeighVisual")]
        public int HeighVisual { set; get; }

   
        void Process1()
        {
            Thread.Sleep(10);
            Stop = false;
            while (!Stop)
            {
                if (_mbData == null)
                {
                    _mbData = new ModBus(_busProperties.Baudrate);
                    Thread.Sleep(1000);
                    continue;
                }

                if (_mbData.CurrentPort != _busProperties.Port)
                {
                    _mbData.ClosePort();
                    Thread.Sleep(1000);
                    _mbData.OpenPort(_busProperties.Port);
                    Thread.Sleep(1000);
                }

                if (!_mbData.PortIsOpened)
                {
                    try
                    {
                        _mbData.ClosePort();
                        _mbData.OpenPort(_busProperties.Port);
                    }
                    catch (Exception)
                    { }
                    Thread.Sleep(1000);
                    continue;
                }
                // Читаем модули
                if (Project == null) continue;

                for (var i = 0; i < Project.Modules.Count; i++)
                {
                    if (Project.Modules[i].BusId != BusID) continue;
                    try
                    {        
                        MbusError er;
                        switch (Project.Modules[i].Type)
                        {
                            case Type.AI: // Читаем аналоговый модуль
                                var bb = new short[16];
                                er = _mbData.ReadInputRegisters(ref bb, Project.Modules[i].Adress, 0, 8);
                                if (er.Code == MbusError.Errorcode.DATA_OK)
                                {
                                    Project.Modules[i].Data = bb;
                                    Project.Modules[i].Err = Errore.Connected;
                                }
                                else Project.Modules[i].Err = Errore.NoAnswer;
                         
                                break;

                            case Type.EN: // Читаем модуль Енкодера 
                                var dt = new short[8];
                                er = _mbData.ReadInputRegisters(ref dt, Project.Modules[i].Adress, 0, 2);
                                if (er.Code == MbusError.Errorcode.DATA_OK)
                                {
                                    Project.Modules[i].Data = (Int16)(dt[1] << 16) | (Int16)dt[0];
                                    Project.Modules[i].Err = Errore.Connected;
                                }
                                else Project.Modules[i].Err = Errore.NoAnswer;
                               
                                break;

                            case Type.DO: // Читаем модуль выходов
                                var bits1 = new bool[16];
                                er = _mbData.ReadCoil(ref bits1, Project.Modules[i].Adress, 0, 16);
                                if (Project.Modules[i].Data == null)
                                    Project.Modules[i].Data = new bool[16];
                                var bools = Project.Modules[i].Data as bool[];
                                if (bools == null) continue;
                                
                                for (var re = 0; re < 16; re++)
                                {
                                    if (bools[re] == bits1[re]) continue;
                                    er = _mbData.WriteSingleCoil(bools[re], Project.Modules[i].Adress, re);
                                }
                                switch (er.Code)
                                {
                                    case MbusError.Errorcode.DATA_OK:
                                        Project.Modules[i].Err = Errore.Connected;
                                        break;
                                    default:
                                        Project.Modules[i].Err = Errore.NoAnswer;
                                        break;
                                }
                               
                                break;

                            case Type.DI: // Читаем модуль входов
                                var bits21 = new bool[16];
                                er = _mbData.ReadInputDiscreteRegister(ref bits21, Project.Modules[i].Adress, 0, 16);
                                if (er.Code == MbusError.Errorcode.DATA_OK)
                                {
                                    Project.Modules[i].Data = bits21;
                                    Project.Modules[i].Err = Errore.Connected;
                                }
                                else Project.Modules[i].Err = Errore.NoAnswer;
                         
                                break;

                            case Type.DT: // Читаем модуль дэсок
                                if (_slowModuleIntervaltimer > 5)
                                {
                                    var temperature = new float[16];
                                    var dat = new short[64];
                                    er = _mbData.ReadInputRegisters(ref dat, Project.Modules[i].Adress, 0, 32);
                                    if (er.Code == MbusError.Errorcode.DATA_OK)
                                    {
                                        byte sh = 0;
                                        for (int im = 0; im < 16; im++)
                                        {
                                            var ad = new byte[4];
                                            ad[0] = (byte) (dat[0 + sh] & 0x00FF);
                                            ad[1] = (byte) (dat[0 + sh] >> 8);
                                            ad[2] = (byte) (dat[1 + sh] & 0x00FF);
                                            ad[3] = (byte) (dat[1 + sh] >> 8);
                                            temperature[im] = BitConverter.ToSingle(ad, 0);
                                            sh += 2;
                                        }

                                        Project.Modules[i].Data = temperature;
                                        Project.Modules[i].Err = Errore.Connected;
                                    }
                                    else Project.Modules[i].Err = Errore.NoAnswer;
                                    _slowModuleIntervaltimer = 0;
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message + " | Comunication");
                        _busState = BusState.Error;
                    }
                }
            }
            if (_mbData != null) _mbData.ClosePort();
            Console.WriteLine("End process bus destroy");
            _busState = BusState.Dispatch;
        }

        
    }
}