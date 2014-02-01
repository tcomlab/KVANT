using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using КВАНТ.Logical.ProgrammEngine;

namespace КВАНТ.ViewsFormsPages.FormView.Control
{
    /// <summary>
    /// Interaction logic for ProgramControl.xaml
    /// </summary>
    public partial class ProgramControl : UserControl
    {
        public ProgramControl()
        {
            InitializeComponent();
            switch (Kernel.KernelStatus)
            {
                case KernelStatus.Stop:
                    ch3.IsChecked = true;
                    break;
                case KernelStatus.Start:
                    break;
                case KernelStatus.Pause:
                    break;
                case KernelStatus.Reset:
                    break;
                case KernelStatus.Unknow:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StopCheck(object sender, RoutedEventArgs e)
        {
            Kernel.KernelStatus =KernelStatus.Stop;
        }

        private void PauseCheck(object sender, RoutedEventArgs e)
        {
            Kernel.KernelStatus = KernelStatus.Pause;
        }

        private void RunCheck(object sender, RoutedEventArgs e)
        {
            Kernel.KernelStatus = KernelStatus.Start;
        }
    }
}
