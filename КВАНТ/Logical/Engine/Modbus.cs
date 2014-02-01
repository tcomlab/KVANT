using System;
using System.IO.Ports;

namespace eslelectronic
{
    #region Клас описывающий ошибки Modbus
    /// <summary>
    /// Клас описывающий ошибки протокола
    /// </summary>
    [Serializable]
    public class MbusError
    {
        public enum Errorcode
        {
            /// <summary>
            /// Удалённое устройство не отвечает
            /// </summary>
            NOT_ANSWER, 
            /// <summary>
            /// Принятый код функции не может быть обработан на подчиненном.
            /// </summary>
            ILLEGAL_FUNCTION,
            /// <summary>
            /// Адрес данных указанный в запросе не доступен данному подчиненному. 
            /// </summary>
            ILLEGAL_DATA_ADDRESS,
            /// <summary>
            /// Величина содержащаяся в поле данных запроса является не допустимой величиной для подчиненного.
            /// </summary>
            ILLEGAL_DATA_VALUE,  
            /// <summary>
            ///  Невосстанавливаемая ошибка имела место пока подчиненный пытался выполнить затребованное действие.
            /// </summary>
            SLAVE_DEVICE_FAILURE,
            /// <summary>
            /// Подчиненный принял запрос и обрабатывает его, но это требует много времени. Этот ответ предохраняет главного от генерации ошибки таймаута. Главный может выдать команду Poll Program Complete для обнаружения завершения обработки команды.
            /// </summary>
            ACKNOWLEDGE,   
            /// <summary>
            /// Подчиненный занят обработкой команды. Главный должен повторить сообщение позже, когда подчиненный освободится. 
            /// </summary>
            SLAVE_DEVICE_BUSY, 
            /// <summary>
            /// Подчиненный не может выполнить программную функцию, принятую в запросе. Этот код возвращается для неудачного программного запроса, использующего функции с номерами 13 или 14. Главный должен запросить диагностическую информацию или информацию обошибках с подчиненного. 
            /// </summary>
            NEGATIVE_ACKNOWLEDGE, 
            /// <summary>
            /// Подчиненный пытается читать расширенную память, но обнаружил ошибку паритета. Главный может повторить запрос, но обычно в таких случаях требуется ремонт. 
            /// </summary>
            MEMORY_PARITY_ERROR,  
            /// <summary>
            /// Данные в порядке
            /// </summary>
            DATA_OK  
        };

        public MbusError()
        {
            Code = Errorcode.NOT_ANSWER;
        }

        public MbusError(Errorcode code)
        {
            this.Code = code;
        }

        public Errorcode Code { set; get; }
    }

    #endregion

    public class ModBus
    {
        private readonly SerialPort _dataPort;

        private enum CMD { ReadCoil, ReadDiscreteInput, ReadHoldingRegister, ReadInputRegister, WriteSingleCoil, WriteSingleRegister };

        #region Инициализация порта и его открытие/закрытие

        public ModBus(int boudrate)
        {
            _dataPort = new SerialPort
            {
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One,
                BaudRate = boudrate,
                Handshake = Handshake.None,
                ReadTimeout = 1000,
                WriteTimeout = 1000
            };

            _dataPort.ErrorReceived += _dataPort_ErrorReceived;
        }

        void _dataPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            
        }

        public bool OpenPort(string portName)
        {
            try
            {
                _dataPort.PortName = portName;
                _dataPort.Open();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void ClosePort()
        {
            _dataPort.Close();
        }

        public bool PortIsOpened
        {
            get
            {
                return _dataPort.IsOpen;
            }
        }

        public string CurrentPort {
            get { return _dataPort.PortName; }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="slaveId"></param>
        /// <param name="adress"></param>
        /// <param name="quantity"></param>
        /// <returns>Количество ожидаемых байт в ответе</returns>
        private int send_req(CMD cmd, byte slaveId, Int16 adress, Int16 quantity)
        {
            var req = new byte[] { slaveId, (byte)(cmd + 1), (byte)((adress & 0xFF00) >> 8), (byte)(adress & 0x00FF), (byte)((quantity & 0xFF00) >> 8), (byte)(quantity & 0x00FF), 0x00, 0x00 };
            var crc1 = new CRCCalculate().CRCmodbus(req, 6);
            req[6] = (byte)((crc1 & 0xFF00) >> 8);
            req[7] = (byte)((crc1 & 0xFF));
            try
            {
                _dataPort.Write(req, 0, req.Length);
            }
            catch
            { return 0; }

            switch (cmd)
            {
                case CMD.ReadCoil:
                    return quantity / 8 + 3 + 2 + 0;
                case CMD.ReadDiscreteInput:
                    return quantity / 8 + 3 + 2 + 0;
                case CMD.ReadHoldingRegister:
                    return quantity * 2 + 3 + 2 + 1;
                case CMD.ReadInputRegister:
                    return quantity * 2 + 3 + 2 + 0;
                case CMD.WriteSingleCoil:
                    return quantity / 8 + 3 + 2 + 0;
                case CMD.WriteSingleRegister:
                    return quantity / 8 + 3 + 2 + 0;
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="waintingDataLengh"></param>
        /// <param name="slaveId"></param>
        /// <returns></returns>
        private byte[] parse_answer(int waintingDataLengh, byte slaveId)
        {
            var rxData = new byte[256];
            var c = 0;
            try
            {
                while (waintingDataLengh != c)
                {
                    rxData[c++] = (byte) (_dataPort.ReadByte());
                   
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            if (c == 0) return null;

            if (c == 5)
            {
                // Сообщение об ошибке
            }

            if (c == waintingDataLengh)
            {
                // Хороший переходник
            }
            else
            {
                // Плохой переходник
                c = waintingDataLengh;
            }

            var crc = 0;
            try
            { crc = new CRCCalculate().CRCmodbus(rxData, c - 2); }
            catch { return null; }
            var crc2 = ((rxData[c - 2] << 8) | (rxData[c - 1]));
            if (crc != crc2)
                return null;
            if (slaveId != rxData[0])
                return null;
            return rxData;
        }

        /// <summary>
        /// Чтение состояния выходов 0х01
        /// </summary>
        /// <param name="bits">ссылка на читаемые битв</param>
        /// <param name="slaveId">адрес подченённого устройства</param>
        /// <param name="adress">адрес бита</param>
        /// <param name="quantity">количество читаемых бит</param>
        /// <returns> состояние чтения</returns>
        /// 
        public MbusError ReadCoil(ref bool[] bits, byte slaveId, Int16 adress, Int16 quantity)
        {
            var data = parse_answer(send_req(CMD.ReadCoil, slaveId, adress, quantity), slaveId);
            if (data == null) return new MbusError() { Code = MbusError.Errorcode.NOT_ANSWER };
            if (data[0x01] == 0x84) return new MbusError() { Code = (MbusError.Errorcode)(data[0x02]) };
            var massPointer = 0;
            for (var i = 1; i <= data[2]; i++)
            {
                for (int n = 0; n < 8; n++)
                {
                    bits[massPointer++] = Convert.ToBoolean((data[2 + i] >> n) & 0x01);
                }
            }
            return new MbusError() { Code = MbusError.Errorcode.DATA_OK };
        }

        /// <summary>
        /// Чтение состояния дискретных входов 0х02
        /// </summary>
        /// <param name="bits">ссылка на читаемые битв</param>
        /// <param name="slaveId">адрес подченённого устройства</param>
        /// <param name="adress">адрес бита</param>
        /// <param name="quantity">количество читаемых бит</param>
        /// <returns> состояние чтения</returns>
        /// 
        public MbusError ReadInputDiscreteRegister(ref bool[] bits, byte slaveId, Int16 adress, Int16 quantity)
        {
            var data = parse_answer(send_req(CMD.ReadDiscreteInput, slaveId, adress, quantity), slaveId);
            if (data == null) return new MbusError(MbusError.Errorcode.NOT_ANSWER);
            if (data[0x01] == 0x84) return new MbusError((MbusError.Errorcode)(data[0x02]));
            var massPointer = 0;
            for (int i = 1; i <= data[2]; i++)
            {
                for (int n = 0; n < 8; n++)
                    bits[massPointer++] = Convert.ToBoolean((data[2 + i] >> n) & 0x01);
            }
            return new MbusError(MbusError.Errorcode.DATA_OK);
        }

        /// <summary>
        /// Чтение регистров устройства Read Holding Registers 0x03
        /// </summary>
        /// <param name="datacomplite"></param>
        /// <param name="slaveId"></param>
        /// <param name="adress"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public MbusError ReadHoldingRegisters(ref byte[] datacomplite, byte slaveId, Int16 adress, Int16 quantity)
        {
            var data = parse_answer(send_req(CMD.ReadHoldingRegister, slaveId, adress, quantity), slaveId);
            if (data == null) return new MbusError(MbusError.Errorcode.NOT_ANSWER);
            if (data[0x01] == 0x84) return new MbusError((MbusError.Errorcode) (data[0x02]));
            for (int i = 0; i < data[2]; i++) datacomplite[i] = data[i + 3];
            return new MbusError(MbusError.Errorcode.DATA_OK); 
        }

        public MbusError ReadHoldingRegisters(ref short[] datacomplite, byte slaveId, Int16 adress, Int16 quantity)
        {
            var data = parse_answer(send_req(CMD.ReadHoldingRegister, slaveId, adress, quantity), slaveId);
            if (data == null) return new MbusError(MbusError.Errorcode.NOT_ANSWER);
            if (data[0x01] == 0x84) return new MbusError((MbusError.Errorcode) (data[0x02]));
            var ca = 3;
            for (var i = 0; i < data[2]; i++)
            {
                datacomplite[i] = (short)((data[ca] << 8) | data[ca + 1]);
                ca += 2;
            }
            return new MbusError(MbusError.Errorcode.DATA_OK); 
        }

        /// <summary>
        /// Чтение регистров устройства Read Input Registers 0x04
        /// </summary>
        /// <param name="dataOut"></param>
        /// <param name="slaveId"></param>
        /// <param name="adress"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public MbusError ReadInputRegisters(ref byte[] dataOut, byte slaveId, Int16 adress, Int16 quantity)
        {
            var data = parse_answer(send_req(CMD.ReadInputRegister, slaveId, adress, quantity), slaveId);
            if (data == null) return new MbusError(MbusError.Errorcode.NOT_ANSWER);
            if (data[0x01] == 0x84) return new MbusError((MbusError.Errorcode) (data[0x02]));
            for (var i = 0; i < data[2]; i++) dataOut[i] = data[i + 3];
            return new MbusError(MbusError.Errorcode.DATA_OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataOut"></param>
        /// <param name="slaveId"></param>
        /// <param name="adress"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public MbusError ReadInputRegisters(ref short[] dataOut, byte slaveId, Int16 adress, Int16 quantity)
        {
            var data = parse_answer(send_req(CMD.ReadInputRegister, slaveId, adress, quantity), slaveId);
            if (data == null) return new MbusError(MbusError.Errorcode.NOT_ANSWER);
            if (data[0x01] == 0x84) return new MbusError((MbusError.Errorcode) (data[0x02]));
            var ca = 3;
            for (var i = 0; i < data[2]; i++)
            {
                dataOut[i] = (short)((data[ca] << 8) | data[ca + 1]);
                ca += 2;
            }
            return new MbusError(MbusError.Errorcode.DATA_OK);
        }


        /// <summary>
        /// Запись состояния выходов 0х05 WriteSingleCoil
        /// </summary>
        /// <param name="bit">вкл/выкл выход</param>
        /// <param name="bitState"></param>
        /// <param name="slaveId">адрес подченённого устройства</param>
        /// <param name="adress">адрес бита</param>
        /// <returns> состояние чтения</returns>
        public MbusError WriteSingleCoil(bool bitState, byte slaveId, int adress)
        {
            var req = new byte[] { slaveId, 0x05, (byte)((adress & 0xFF00) >> 8), (byte)(adress & 0x00FF), 0x00, 0x00, 0x00, 0x00 };
            if (bitState) req[4] = 0xFF; else req[4] = 0x00;
            var rxData = new byte[64];
            var crc1 = new CRCCalculate().CRCmodbus(req, 6);
            req[6] = (byte)((crc1 & 0xFF00) >> 8);
            req[7] = (byte)((crc1 & 0xFF));

            try
            {
                _dataPort.Write(req, 0, req.Length);
                var c = 0;
                try
                {
                    for (var i = 0; i < 8; i++)
                    {
                        rxData[i] = (byte)(_dataPort.ReadByte());
                        c++;
                    }
                }
                catch (Exception)
                { }

                if (c == 0)
                    return new MbusError(MbusError.Errorcode.NOT_ANSWER);

                var crc = new CRCCalculate().CRCmodbus(rxData, 6);
                var crc2 = (rxData[6] << 8) | (rxData[7]);
                if (crc != crc2)
                    return new MbusError(MbusError.Errorcode.NOT_ANSWER);
                if (slaveId != rxData[0])
                    return new MbusError(MbusError.Errorcode.NOT_ANSWER);

                if (c == 5) // Detecting modbus errore
                {
                    if (rxData[0x01] == 0x84) // errore
                    {
                        return new MbusError((MbusError.Errorcode) (rxData[0x02]));
                    }
                }

                if (rxData[4] != req[4]) return new MbusError(MbusError.Errorcode.ILLEGAL_DATA_VALUE);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new MbusError(MbusError.Errorcode.NOT_ANSWER);
            }

            return new MbusError(MbusError.Errorcode.DATA_OK);
        }

        /// <summary>
        /// WriteSingleRegister 0x06
        /// </summary>
        /// <param name="data"></param>
        /// <param name="slaveId"></param>
        /// <param name="adress"></param>
        /// <returns></returns>
        public MbusError WriteSingleRegister(byte[] data, byte slaveId, Int16 adress)
        {
            var req = new byte[] { slaveId, 0x06, (byte)((adress & 0xFF00) >> 8), (byte)(adress & 0x00FF), data[1], data[0], 0x00, 0x00 };
            var rxData = new byte[64];
            var crc1 = new CRCCalculate().CRCmodbus(req, 6);
            req[6] = (byte)((crc1 & 0xFF00) >> 8);
            req[7] = (byte)((crc1 & 0xFF));
            try
            {
                _dataPort.DiscardInBuffer();
                _dataPort.DiscardOutBuffer();
                _dataPort.Write(req, 0, req.Length);
                int c = 0;
                try
                {
                    for (var i = 0; i < 9; i++)
                    {
                        rxData[i] = (byte)(_dataPort.ReadByte());
                        c++;
                    }
                }
                catch (Exception)
                { }

                if (c == 0)
                    return new MbusError(MbusError.Errorcode.NOT_ANSWER);

                if (c > 8)
                {
                    c = 8;
                }

                var crc = 0;
                try
                {
                    crc = new CRCCalculate().CRCmodbus(rxData, c - 2);
                }
                catch
                {
                    return new MbusError(MbusError.Errorcode.NOT_ANSWER);
                };

                var crc2 = (int)((rxData[c - 2] << 8) | (rxData[c - 1]));
                if (crc != crc2)
                    return new MbusError(MbusError.Errorcode.NOT_ANSWER);
                if (slaveId != rxData[0])
                    return new MbusError(MbusError.Errorcode.NOT_ANSWER);

                if (c == 5) // Detecting modbus errore
                {
                    if (rxData[0x01] == 0x84) // errore
                    {
                        return new MbusError((MbusError.Errorcode) (rxData[0x02]));
                    }
                }

                if (rxData[4] != req[4]) return new MbusError(MbusError.Errorcode.ILLEGAL_DATA_VALUE);
                if (rxData[5] != req[5]) return new MbusError(MbusError.Errorcode.ILLEGAL_DATA_VALUE);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new MbusError(MbusError.Errorcode.NOT_ANSWER);
            }

            return new MbusError(MbusError.Errorcode.DATA_OK);
        }

    }
    #region Класс подсчёта CRC Modbus

    class CRCCalculate
    {
        private readonly byte[] _auchCRCHi =
        {
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
            0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
            0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
            0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
            0x80, 0x41, 0x00, 0xC1, 0x81, 0x40
        };

        private readonly byte[] _auchCRCLo =
        {
            0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06,
            0x07, 0xC7, 0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD,
            0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
            0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A,
            0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC, 0x14, 0xD4,
            0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
            0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3,
            0xF2, 0x32, 0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4,
            0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
            0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29,
            0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF, 0x2D, 0xED,
            0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
            0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60,
            0x61, 0xA1, 0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67,
            0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
            0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68,
            0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA, 0xBE, 0x7E,
            0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
            0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71,
            0x70, 0xB0, 0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92,
            0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
            0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B,
            0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89, 0x4B, 0x8B,
            0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
            0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42,
            0x43, 0x83, 0x41, 0x81, 0x80, 0x40
        };

        public int CRCmodbus(byte[] puchMsg, int usDataLen)
        {
            byte uchCRCHi = 0xFF;
            byte uchCRCLo = 0xFF;
            var nIndex = 0;
            while (usDataLen-- != 0)
            {
                var uIndex = uchCRCHi ^ puchMsg[nIndex++];
                uchCRCHi = (byte)(uchCRCLo ^ _auchCRCHi[uIndex]);
                uchCRCLo = _auchCRCLo[uIndex];
            }
            return uchCRCHi << 8 | uchCRCLo;
        }

    #endregion
    }
}
