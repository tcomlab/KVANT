using System;
using System.Drawing;
using System.Windows;
using System.Xml.Serialization;
using Type = КВАНТ.Logical.ProgrammEngine.array_l_element.Type;

namespace КВАНТ.Logical.Engine
{
    public enum Errore { NoAnswer, Connected, SensorError };
    public class ModuleRemot
    {
        [XmlElement("Adress")]
        public byte Adress { set; get; }

        [XmlElement("Name")]
        public string Name { set; get; }

        [XmlElement("Type")]
        public Type Type { set; get; }

        [XmlElement("CPoint")]
        public Point CPoint { set; get; } // Для визуального компонента

        [XmlElement("BusID")]
        public int BusId { set; get; }

        [XmlElement("PointPosiltionPoint")]
        public Point PointPosiltionPoint { set; get; }

        [XmlElement("ModuleId")]
        public int ModuleId { set; get; }

        [XmlIgnore]
        public bool Cancel = false;

        public event EventHandler RefreshSetting;

        protected virtual void OnRefreshSetting()
        {
            EventHandler handler = RefreshSetting;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public ModuleRemot(byte adres, Type type)
        {
            Adress = adres;
            Type = type;
        }

        public ModuleRemot()
        {
            Adress = 0;
            Type = Type.AI;
            Name = "";
        }

        public void ModuleSettingDialog()
        {
            var dw = new ModuleSettingDialog(this);
            dw.ShowDialog();
            if (dw.DialogResult == false)
            {
                Cancel = true;
                return;
            }
            Cancel = false;
            var a = dw.MRemote;
            Adress = a.Adress;
            Name = a.Name;
            Type = a.Type;
            OnRefreshSetting();
        }

        [XmlIgnore]
        public object Data
        {
            set
            {
                _lData = value;
                if (DataChenge != null) DataChenge(this, EventArgs.Empty);
            }
            get { return _lData; }
        }

        private object _lData;

        private Errore err;

        private bool _a;
        private bool _b;
        [XmlIgnore]
        public Errore Err
        {
            set
            {
                err = value;
                if (value == Errore.NoAnswer)
                {
                    if (_a == false)
                    {_a = true;
                        OnConectFault();
                    }
                }
                else _a = false;

                if (value == Errore.Connected)
                {
                    if (_b == false)
                    {
                        _b = true;
                        OnConnectNormal();
                    }
                }
                else _b = false;
            }
            get { return err; }
        }
        public event EventHandler DataChenge;
        public event EventHandler ConnectFault;
        public event EventHandler ConnectNormal;

        private void OnConnectNormal()
        {
            EventHandler handler = ConnectNormal;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void OnConectFault()
        {
            EventHandler handler = ConnectFault;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return "Adress: " + Adress + " Type: " + Type.ToString() + " " + ShowData();
        }

        public string ShowData()
        {
            switch (Type)
            {
                case Type.AI:
                    var bg = Data as short[];
                    if (bg == null) return "null";
                    return Convert.ToString(" " + bg[0x0] + " " + bg[1] + " " + bg[2] + " " + bg[3] + " " + bg[4] + " " + bg[5] + " " + bg[6] + " " + bg[7]);
                case Type.EN: return Convert.ToString(Data);
                case Type.DO:
                case Type.DI:
                    var bt1 = Data as bool[];
                    if (bt1 == null) return "null";
                    return Convert.ToString(string.Format("{1} {2} {0} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15}", bt1[2], bt1[0].ToString(), bt1[1].ToString(), bt1[3].ToString(), bt1[4].ToString(), bt1[5].ToString(), bt1[6].ToString(), bt1[7].ToString(), bt1[8].ToString(), bt1[9].ToString(), bt1[10].ToString(), bt1[11].ToString(), bt1[12].ToString(), bt1[13].ToString(), bt1[14].ToString(), bt1[15].ToString()));
                case Type.DT:
                    var dt = Data as float[];
                    if (dt == null) return "null";
                    return Convert.ToString(" " + dt[0] + " " + dt[1] + " " + dt[2] + " " + dt[3] + " " + dt[4] + " " + dt[5] + " " + dt[6] + " " + dt[7]);
                default: return "unknow";

            }
        }
    }
}