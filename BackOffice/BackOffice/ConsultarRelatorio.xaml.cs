using System.Windows;

namespace BackOffice
{

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

