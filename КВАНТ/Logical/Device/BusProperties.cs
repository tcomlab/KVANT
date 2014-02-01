using System;
using System.Xml.Serialization;

namespace КВАНТ.Logical.Device
{
    [Serializable]
    public class BusProperties
    {
        [XmlElement("Name")]
        public string Name { set; get; }

        [XmlElement("Port")]
        public string Port { set; get; }

        [XmlElement("Baudrate")]
        public int Baudrate { set; get; }

        [XmlElement("Color")]
        public string Color { set; get; }

        public BusProperties()
        {
            Name = "";
            Port = "COM1";
            Baudrate = 9600;
            Color = "#FFFF00FF";
        }

        public void BusSettingDialog()
        {
            var bd = new BusDialog(this);
            bd.ShowDialog();
            var a = bd.propetry;
            Name = a.Name;
            Port = a.Port;
            Baudrate = a.Baudrate;
        }
    }
}