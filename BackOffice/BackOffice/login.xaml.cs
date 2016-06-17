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
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        public login()
        {
            InitializeComponent();
        }
        
        private void button_Click(object sender, RoutedEventArgs e)
        {
            facade fac = new facade();
            int id_fiscal;
            bool sucesso;
            if (textBox.Text.Equals("") || passwordBox.Password.Equals("")) {
                label2.Content = "Por favor preencha todos os campos";
            }else
            {
                if (!IsDigitsOnly(textBox.Text))
                {
                    label2.Content = "O identificador de fiscal apenas contêm digitos";
                    textBox.Text = "";
                }else {
                    fac = new facade();
                    id_fiscal = Convert.ToInt32(textBox.Text);
                    sucesso = fac.login(id_fiscal, passwordBox.Password);
                    if (sucesso)
                    {
                        MainWindow newWindow = new MainWindow(id_fiscal);
                        newWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        Popup1.IsOpen = true;
                        textBox.Text = "";
                        passwordBox.Password = "";

                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Popup1.IsOpen = false;
        }

        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}
