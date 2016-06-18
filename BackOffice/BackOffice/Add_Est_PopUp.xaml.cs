using System.Windows;

namespace BackOffice
{
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
