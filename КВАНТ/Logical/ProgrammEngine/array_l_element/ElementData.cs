using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;
using Brush = System.Windows.Media.Brush;

namespace КВАНТ.Logical.ProgrammEngine.array_l_element
{
    /*
    public interface IElementData
    {
        string Name { set; get; }
        string[] Text { set; get; }
        string[] Color { set; get; }
        int[] Val { set; get; }
        Point VisPoint { set; get; }
        int[] Misc { set; get; }
        int Y { set; get; }
        int X { set; get; }
        KVANT_PLC.Logical.ProgrammEngine.array_l_element.Type Type { set; get; }
        DeviceBind DeviceBinding { set; get; }
        double[] DoubleData { set; get; }
        object[] MiscObject { set; get; }
        bool BlockExec1Bool { set; get; }
        bool BlockExec2Bool { set; get; }
    }
    */
    public class DeviceBind
    {
        public byte Adress { set; get; }
        public byte Chanel { set; get; }

        public int ModuleId { set; get; }

        public DeviceBind()
        {
            Adress = 0;
            Chanel = 0;
            ModuleId = 0;
        }
    }

    public class VisualElement
    {
        [XmlElement("Position")] // 
        public Point Position { set; get; }

        //[XmlElement("Visible")] // 
        //public bool Visible { set; get; }

        [XmlElement("X")]
        public int X { set; get; }
 
        [XmlElement("Y")]
        public int Y { set; get; }

        public VisualElement()
        {
            Position = new Point();
            //Visible = false;
        }
    }

    public class ElementData : INotifyPropertyChanged
    {
        /*
        [XmlElement("Visual")]
        public VisualElement Visual { set { _visual = value; OnPropertyChanged("Visual"); } get { return _visual; } }
        private VisualElement _visual; */

        [XmlIgnore]
        public bool BlockExec1Bool { set { _blockExec1Bool = value; OnPropertyChanged("BlockExec1Bool"); } get { return _blockExec1Bool; } }
        private bool _blockExec1Bool;

        [XmlIgnore]
        public bool BlockExec2Bool { set { _blockExec2Bool = value; OnPropertyChanged("BlockExec2Bool"); } get { return _blockExec2Bool; } }
        private bool _blockExec2Bool;

        [XmlIgnore]
        public string CurrentValueStr { set { _currentValueStr = value; OnPropertyChanged("CurrentValueStr"); } get { return _currentValueStr; } }
        private string _currentValueStr;

        [XmlIgnore]
        public int CurrentValueSInt { set { _currentValueInt = value; OnPropertyChanged("CurrentValueSInt"); } get { return _currentValueInt; } }
        private int _currentValueInt;

        [XmlIgnore]
        public bool CurrentValueBool { set { _currentValueBool = value; OnPropertyChanged("CurrentValueBool"); } get { return _currentValueBool; } }
        private bool _currentValueBool;

        [XmlIgnore]
        public double CurrentValueDouble { set { _currentValueDouble = value; OnPropertyChanged("CurrentValueDouble"); } get { return _currentValueDouble; } }
        private double _currentValueDouble;

        [XmlElement("Name")]
        public string Name { set { _name = value; OnPropertyChanged("Name"); } get { return _name; } }
        private string _name;

        [XmlElement("Text")]
        public string[] Text { set { _text = value; OnPropertyChanged("Text"); } get { return _text; } }
        private string[] _text;

        [XmlElement("Color")]
        public string[] Color { set { _color = value; OnPropertyChanged("Color"); } get { return _color; } }
        private string[] _color;

        [XmlElement("Val")]
        public int[] Val { set { _val = value; OnPropertyChanged("Val"); } get { return _val; } }
        private int[] _val;

        [XmlElement("VisPoint")]
        public Point VisPoint { set { _visPoint = value; OnPropertyChanged("VisPoint"); } get { return _visPoint; } }
        private Point _visPoint;

        [XmlElement("Misc")]
        public int[] Misc { set { _misc = value; OnPropertyChanged("Misc"); } get { return _misc; } }
        private int[] _misc;

        [XmlElement("Object")]
        public object[] MiscObject { set { _miscObject = value; OnPropertyChanged("MiscObject"); } get { return _miscObject; } }
        private object[] _miscObject;

        [XmlElement("DoubleData")]
        public double[] DoubleData { set { _doubleData = value; OnPropertyChanged("DoubleData"); } get { return _doubleData; } }
        private double[] _doubleData;

        [XmlElement("DeviceBind")]
        public DeviceBind DeviceBinding { set { _deviceBind = value; OnPropertyChanged("DeviceBinding"); } get { return _deviceBind; } }
        private DeviceBind _deviceBind;

        [XmlElement("type")]
        public КВАНТ.Logical.ProgrammEngine.array_l_element.Type Type { set { _type = value; OnPropertyChanged("Type"); } get { return _type; } }
        private КВАНТ.Logical.ProgrammEngine.array_l_element.Type _type;

        [XmlElement("ArrayLogicX")]
        public int X { set { _x = value; OnPropertyChanged("X"); } get { return _x; } }
        private int _x;

        [XmlElement("ArrayLogicY")]
        public int Y { set { _y = value; OnPropertyChanged("Y"); } get { return _y; } }
        private int _y;

        public Brush GetColor(int index)
        {
            if (Color.Length > index)
            {
                if (Color == null) return null;
                if (Color[index] == null) return null;
                return new BrushConverter().ConvertFromString(Color[index]) as SolidColorBrush;
            }
            return new BrushConverter().ConvertFromString(Color[0]) as SolidColorBrush;
        }

        public void SetColor(Brush color,int index)
        {
            if (Color == null) return ;
            if (Color[index] == null) return ;
            Color[index] = color.ToString();
        }
        
        public ElementData()
        {
            Name = "";
            Text = new string[8];
            Color = new string[8];
            Val = new int[8];
            VisPoint = new Point();
            Misc = new int[8];
            MiscObject = new object[8];
            DoubleData = new double[8];
            DeviceBinding = new DeviceBind();
            Type = Type.NN;
            X = 0;
            Y = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

      
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
