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
using System.Windows.Shapes;

namespace BackOffice
{
    /// <summary>
    /// Interaction logic for Add_Est_PopUp.xaml
    /// </summary>
    public partial class Add_Est_PopUp : Window
    {

        public bool resposta;

        public Add_Est_PopUp()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            resposta = true;
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            resposta = false;
            this.Close();
        }
    }
}
