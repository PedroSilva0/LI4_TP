using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackOffice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            //bool hasBeenClicked = false; //VARIAVEL GLOBAL
           // if (!hasBeenClicked)
            //{
                TextBox box = sender as TextBox;
                box.Text = String.Empty;
                //hasBeenClicked = true;
            //}
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            box.Text = "Pesquisar";
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            box.Text = String.Empty;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            box.Text = "Pesquisar";
        }
    }
}
