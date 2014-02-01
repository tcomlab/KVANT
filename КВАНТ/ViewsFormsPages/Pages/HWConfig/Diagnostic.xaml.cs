using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using КВАНТ.Logical.Engine;
using Type = КВАНТ.Logical.ProgrammEngine.array_l_element.Type;

namespace КВАНТ.ViewsFormsPages.Pages.HWConfig
{
    /// <summary>
    /// Interaction logic for Diagnostic.xaml
    /// </summary>

    public class DataItem
    {
        public string Column1 { get; set; }
        public object Column2 { get; set; }
    }

    public partial class Diagnostic : Window
    {
        private ModuleRemot _moduleRemot;

        

        public Diagnostic(ModuleRemot mrRemot)
        {
            InitializeComponent();

            _moduleRemot = mrRemot;
            DataGrid1.Items.Add(new DataItem { Column1 = "Адрес", Column2 = Convert.ToString(_moduleRemot.Adress) });
            DataGrid1.Items.Add(new DataItem { Column1 = "Имя", Column2 = _moduleRemot.Name });
            DataGrid1.Items.Add(new DataItem { Column1 = "Тип", Column2 = Convert.ToString(_moduleRemot.Type) });

            switch (_moduleRemot.Type)
            {
                case Type.DI:
                    //for (int i = 0; i < 16; i++)
                       // DataGrid1.Items.Add(new DataItem { Column1 = "Канал" + Convert.ToString(i+1),  });
                    break;
                case Type.DO:
                    //for (int i = 0; i < 16; i++)
                        //DataGrid1.Items.Add(new DataItem { Column1 = "Канал" + Convert.ToString(i+1), Column2 = "---" });
                    break;
                case Type.EN:
                    //    DataGrid1.Items.Add(new DataItem { Column1 = "Значение", Column2 = "---" });
                    break;
                case Type.DT:
                    //for (int i = 0; i < 16; i++)
                    //    DataGrid1.Items.Add(new DataItem { Column1 = "Канал" + Convert.ToString(i+1), Column2 = "---" });
                    break;
                case Type.AI:
                    var data = (ushort[])_moduleRemot.Data;
                    for (int i = 0; i < 8; i++)
                        DataGrid1.Items.Add(new DataItem { Column1 = "Канал" + Convert.ToString(i + 1), Column2 = data[i] });
                    break;
            }

            
        }

        
    }
}
