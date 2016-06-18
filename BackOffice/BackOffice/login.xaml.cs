using System;
using System.Windows;


namespace BackOffice
{

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
            if (textBox.Text.Equals("") || passwordBox.Password.Equals(""))
            {
                label2.Content = "Por favor preencha todos os campos";
            }
            else
            {
                if (!IsDigitsOnly(textBox.Text))
                {
                    label2.Content = "O identificador de fiscal apenas contêm digitos";
                    textBox.Text = "";
                }
                else
                {
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
