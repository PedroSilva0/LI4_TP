using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BackOffice
{
    /// <summary>
    /// Interaction logic for data_diagram_teste.xaml
    /// </summary>
    public partial class data_diagram_teste : Window
    {

        private LI4Entities data = new LI4Entities();


        public data_diagram_teste()
        {
            InitializeComponent();
            var lista = data.Estabelecimento.ToList();
            foreach (Estabelecimento item in lista)
            {
                Console.WriteLine(item.nome);
                string[] row2 = { Convert.ToString(item.id_est), item.nome, item.morada};
                string[] row = { "1", "3", "4" };
                this.listView.Items.Add(new Estabelecimento {nome = item.nome, id_est = item.id_est,
                                    morada=item.morada, latitude=item.latitude,longitude=item.longitude});
            }

        }

        private void listView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                Estabelecimento est = (Estabelecimento)listView.SelectedItems[0];
                Console.WriteLine(est.latitude + "             " + est.longitude);
            }
        }
    }
}
