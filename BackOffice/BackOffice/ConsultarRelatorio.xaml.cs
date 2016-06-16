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
    /// Interaction logic for ConsultarRelatorio.xaml
    /// </summary>
    public partial class ConsultarRelatorio : Window
    {
        private LI4Entities data = new LI4Entities();

        public ConsultarRelatorio(int visita)
        {
            InitializeComponent();
            loadRelatorio(visita);
        }

        private void loadRelatorio(int visita)
        {

            Visita aux = data.Visita.Find(visita);
            string relatorio = aux.html_file;
            wb2.NavigateToString(relatorio);
        }
    }




}

