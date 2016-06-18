using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BackOffice
{

    public partial class CriarRelatorio : Window
    {
        Relatorio r;//ler o numero da visita antes
        public CriarRelatorio(int visita)
        {
            InitializeComponent();
            r = new Relatorio(visita);
            rtb1.AppendText(r.criarRelatorio());
            button1_Click(null, null);
        }
        private void rtb1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try { button1.Background = Brushes.White; }
            catch (Exception) {; }

        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            //classificacao
            try { button1.Background = Brushes.White; }
            catch (Exception) {; }
        }


        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            //observacoes
            try { button1.Background = Brushes.White; }
            catch (Exception) {; }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //refresh
            try { button1.Background = Brushes.LightGray; }
            catch (Exception) { }
            string html = refresh();
            try
            {
                wb1.NavigateToString(html);
            }
            catch (Exception) { }

        }

        private string refresh()
        {

            var textRange = new TextRange(rtb1.Document.ContentStart, rtb1.Document.ContentEnd);
            string t = textRange.Text;
            string h = r.loadCabecalho();
            string cla = r.classificacao(textBox1.Text);
            string obs = r.observacoes(textBox2.Text);

            return h + t + cla + obs;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //guardar
            if (textBox1.Text.Equals("") && textBox2.Text.Equals(""))
                label4.Content = "Preencher classificação e observações";
            else if (textBox1.Text.Equals(""))
                label4.Content = "Preencher classificação";
            else if (textBox2.Text.Equals(""))
                label4.Content = "Preencher observações";
            else
            {
                r.guardarRelatorio(refresh(), textBox1.Text);
                label4.Content = "Guardado com sucesso";
            }
        }
    }
}