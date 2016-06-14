using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
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
        private int id_rest;
        private int id_planos;
        private int id_fiscal=1;

        public MainWindow()
        {
            InitializeComponent();
            fac = new facade();
            lista_restaurantes();
        }

        private void lista_restaurantes()
        {
            listView.Items.Clear();
            fac = new facade();
            var lista_estabelecimento = fac.listarRestaurantes();
            foreach (Estabelecimento item in lista_estabelecimento)
            {
                
                this.listView.Items.Add(new Estabelecimento
                {
                    nome = item.nome,
                    id_est = item.id_est,
                    morada = item.morada,
                    latitude = item.latitude,
                    longitude = item.longitude
                });
            }
            id_rest = lista_estabelecimento.Count+1;
        }



        private void button6_Click(object sender, RoutedEventArgs e)
        {
            fac.registarRes(id_rest, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text);
            lista_restaurantes();
            label10.Content = "Restaurante adicionado com sucesso";
        }

        private void add_to_map_Click(object sender, RoutedEventArgs e)
        {
                Estabelecimento est = (Estabelecimento)listView.SelectedItems[0];
                //Console.WriteLine(est.nome + "         " + est.latitude + "             " + est.longitude);
                Pushpin pushpin = new Pushpin();
                pushpin.Tag = est.id_est;
                MapLayer.SetPosition(pushpin, new Location(est.latitude, est.longitude));
                myMap.Children.Add(pushpin);
            //draw_route();
        }

        //nao funciona
        private void draw_route()
        {
            //List<Location> vertices = new List<Location>();
            LocationCollection vertices = new LocationCollection();
            var pushPins = this.myMap.Children.OfType<Pushpin>();
            for (int i = 0; i < pushPins.Count(); i++)
            {
                Pushpin p = pushPins.ElementAt(i);
                Console.WriteLine("tou aqui"+p.Location);
                if (p.Location == null)
                {
                    Console.WriteLine("tou qui 2");
                }
                vertices.Add(p.Location);
            }
            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            polyline.StrokeThickness = 5;
            polyline.Opacity = 0.7;
            polyline.Locations = vertices;
            myMap.Children.Add(polyline);
        }


        private void remove_from_map_Click(object sender, RoutedEventArgs e)
        {
                Estabelecimento est = (Estabelecimento)listView.SelectedItems[0];
                var pushPins = this.myMap.Children.OfType<Pushpin>();
            
                for(int i=pushPins.Count()-1; i >= 0; i--)
            {
                Pushpin p = pushPins.ElementAt(i);
                if (p.Tag.Equals(est.id_est))
                {
                    myMap.Children.Remove(p);
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            listView.SelectedItem = null;
            myMap.Children.Clear();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            List<int> estabelecimentos=new List<int>();
            //Estabelecimento est = (Estabelecimento)listView.SelectedItems[0];
            var pushPins = this.myMap.Children.OfType<Pushpin>();

            for (int i = 0; i < pushPins.Count(); i++)
            {
                Pushpin p = pushPins.ElementAt(i);
                estabelecimentos.Add(Convert.ToInt32(p.Tag.ToString()));
            }
            fac.criarPlano(id_fiscal,estabelecimentos);
           /* foreach(int i in estabelecimentos)
            {
                Console.WriteLine(i);
            }*/
            label7.Content = "Plano criado com sucesso";
            //Console.WriteLine(t);

        }
    }
}
