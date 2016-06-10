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

namespace BackOffice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private LI4Entities data;
        private facade fac;
        public MainWindow()
        {

            InitializeComponent();
            fac = new facade();
            //login l = new login();
            //l.Show();
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            fac.registarRes(4, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text);
            label10.Content = "Restaurante adicionado com sucesso";
        }
    }
}
