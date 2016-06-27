using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BackOffice
{
    public partial class MainWindow : Window
    {
        private facade fac=new facade();
        private int id_fiscal;

        public MainWindow(int i)
        {
            InitializeComponent();
            fac = new facade();
            id_fiscal = i;
            listarRestaurantes();
            listarTodosVoz();
            listaVisitas();
            listaRelatorios();
        }

        private void inspectionTabSelected(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                listaVisitas();
            }
        }

        private void vozTabSelected(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                listarTodosVoz();
            }
        }

        private void relatorioTabSelected(object sender, RoutedEventArgs e)
        {
            var tab = sender as TabItem;
            if (tab != null)
            {
                listaRelatorios();
            }
        }


        private void listaVisitas()
        {
            listView2.Items.Clear();
            fac = new facade();
            var lista_visita = fac.listarVisitas();
            if (lista_visita.Count() == 0)
            {
                label8.Content = "Não existem inspeções concluidas";
            }
            else
            {
                foreach (Display_aux item in lista_visita)
                {
                    this.listView2.Items.Add(new Display_aux
                    {
                        id = item.id,
                        desc = item.desc


                    });
                }
                label8.Content = "";
            }
        }

        private void listaRelatorios()
        {
            listView3.Items.Clear();
            fac = new facade();
            var lista_relatorios = fac.listarRelatorios();

            if (lista_relatorios.Count() == 0)
            {
                label9.Content = "Não existem relatórios";
            }
            else
                foreach (Display_aux item in lista_relatorios)
                {
                    this.listView3.Items.Add(new Display_aux
                    {
                        id = item.id,
                        desc = item.desc
                    });
                }

        }

        private void listarRestaurantes()
        {
            listView.Items.Clear();
            fac = new facade();
            var lista_estabelecimento = fac.listarRestaurantes();
            if (lista_estabelecimento.Count() == 0)
            {
                label7.Content = "Não se encontram estabelecimentos registados";
            }
            else
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
        }

        private void listarTodosVoz()
        {
            listView1.Items.Clear();
            fac = new facade();
            var lista_voz = fac.listarTodosVoz();
            if (lista_voz.Count() == 0)
            {
                label6.Content = "Não existem ficheiros de voz";
            }
            else
            {
                foreach (Display_aux item in lista_voz)
                {
                    this.listView1.Items.Add(new Display_aux
                    {
                        desc = item.desc,
                        id = item.id,
                    });
                }
                label6.Content = "";
            }
        }

        private void registar_estabelecimento_Click(object sender, RoutedEventArgs e)
        {
            bool inserir = true;

            double latitude = 0;
            double longitude = 0;
            if (textBox3.Text.Equals("") || textBox4.Text.Equals("") || textBox5.Text.Equals("") || textBox6.Text.Equals(""))
            {
                label10.Content = "Por favor preencha todos os campos";
                inserir = false;
            }
            else
            {
                if(Double.TryParse(textBox5.Text,out latitude) && 
                            Double.TryParse(textBox6.Text, out longitude))
                {
                    if (fac.demasiado_perto(latitude, longitude))
                    {
                        Add_Est_PopUp getup = new Add_Est_PopUp();
                        getup.ShowDialog();
                        inserir = getup.resposta;
                    }
                }
                else
                {
                    textBox5.Text = "";
                    textBox6.Text = "";
                    label10.Content = "Coordenadas GPS com formato inválido";
                    inserir = false;
                }
            }

            if (inserir)
            {
                fac.registarRes(textBox3.Text, textBox4.Text, latitude, longitude);
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                listarRestaurantes();
                label10.Content = "Restaurante adicionado com sucesso";
            }
            else
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
            }
 
        }



        private void add_to_map_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                var pushPins = this.myMap.Children.OfType<Pushpin>();
                bool add = true;
                Estabelecimento est = (Estabelecimento)listView.SelectedItems[0];
                for (int i = pushPins.Count() - 1; i >= 0; i--)
                {
                    Pushpin p = pushPins.ElementAt(i);
                    if (p.Tag.Equals(est.id_est))
                    {
                        add = false;
                    }
                }
                if (add)
                {
                    Pushpin pushpin = new Pushpin();
                    pushpin.Tag = est.id_est;
                    MapLayer.SetPosition(pushpin, new Location(est.latitude, est.longitude));
                    pushpin.Location = new Location(est.latitude, est.longitude);
                    myMap.Children.Add(pushpin);
                    label7.Content = "Estabelecimento adicionado";
                    //centrar mapa
                    Location center = pushpin.Location;
                    double zoom = 15;
                    this.myMap.SetView(center, zoom);
                }
                else
                {
                    label7.Content = "Estabelecimento já adicionado";
                }
            }
            else
            {
                label7.Content = "Por favor selecione um estabelecimento";
            }
            draw_route();
        }
        
        private void draw_route()
        {
            clear_route();
            LocationCollection vertices = new LocationCollection();
            var pushPins = this.myMap.Children.OfType<Pushpin>();
            for (int i = 0; i < pushPins.Count(); i++)
            {
                Pushpin p = pushPins.ElementAt(i);
                vertices.Add(p.Location);
            }
            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            polyline.StrokeThickness = 5;
            polyline.Opacity = 0.7;
            polyline.Locations = vertices;
            myMap.Children.Add(polyline);
        }

        private void clear_route()
        {
            int intTotalChildren = myMap.Children.Count - 1;
            for (int intCounter = intTotalChildren; intCounter > 0; intCounter--)
            {
                if (myMap.Children[intCounter].GetType() == typeof(MapPolyline))
                {
                    MapPolyline ucCurrentChild = (MapPolyline)myMap.Children[intCounter];
                    myMap.Children.Remove(ucCurrentChild);
                }
            }
        }

        private void remove_from_map_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem != null)
            {
                Estabelecimento est = (Estabelecimento)listView.SelectedItems[0];
                var pushPins = this.myMap.Children.OfType<Pushpin>();
                for (int i = pushPins.Count() - 1; i >= 0; i--)
                {
                    Pushpin p = pushPins.ElementAt(i);
                    if (p.Tag.Equals(est.id_est))
                    {
                        myMap.Children.Remove(p);
                    }
                }
                label7.Content = "Estabelecimento removido";
            }
            else
            {
                label7.Content = "Por favor selecione um estabelecimento";
            }
            draw_route();
        }

        private void limpar_selecao_Click(object sender, RoutedEventArgs e)
        {
            listView.SelectedItem = null;
            myMap.Children.Clear();
            label7.Content = "Todos os estabelecimentos removidos";
        }

        private void criar_plano_Click(object sender, RoutedEventArgs e)
        {
            List<int> estabelecimentos = new List<int>();
            var pushPins = this.myMap.Children.OfType<Pushpin>();
            if (pushPins.Count() > 0)
            {
                for (int i = 0; i < pushPins.Count(); i++)
                {
                    Pushpin p = pushPins.ElementAt(i);
                    estabelecimentos.Add(Convert.ToInt32(p.Tag.ToString()));
                }
                fac.criarPlano(id_fiscal, estabelecimentos);
                label7.Content = "Plano criado com sucesso";
            }
            else
            {
                label7.Content = "Não selecionou nenhum estabelecimento a inspecionar";
            }
        }

        private void converter_xml_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                Display_aux vozAux = (Display_aux)listView1.SelectedItems[0];
                textBox.Text = fac.converter_xml(vozAux.id);
                label6.Content = "Ficheiro convertido com sucesso";
            }
            else
            {
                label6.Content = "Por favor selecione um ficheiro a converter";
            }
        }

        private void criar_relatorio_Click(object sender, RoutedEventArgs e)
        {

            if (listView2.SelectedItem != null)
            {
                Display_aux vis = (Display_aux)listView2.SelectedItems[0];
                int visita = Convert.ToInt32(vis.id);
                CriarRelatorio newWindow = new CriarRelatorio(visita);
                newWindow.Show();
            }
            else
            {
                label8.Content = "Por favor escolha uma inspeção do qual deseja criar relatório";
            }
        }

        private void consultar_relatorio_Click(object sender, RoutedEventArgs e)
        {
            if (listView3.SelectedItem != null)
            {
                Display_aux vis = (Display_aux)listView3.SelectedItems[0];
                int visita = Convert.ToInt32(vis.id);
                ConsultarRelatorio newWindow = new ConsultarRelatorio(visita);
                newWindow.Show();
            }
            else
            {
                label9.Content = "Por favor seleciona um relatório a consultar";
            }
        }

        private void enviar_relatorio_Click(object sender, RoutedEventArgs e)
        {
            if (listView3.SelectedItem != null)
            {
                if (!textBox2.Text.Equals(""))
                {
                    
                    Display_aux vis = (Display_aux)listView3.SelectedItems[0];
                    int visita = Convert.ToInt32(vis.id);
                    fac.enviar_relatorio(visita, textBox2.Text, vis.desc);
                    label9.Content = "Relatório enviado com sucesso";
                }
                else
                {
                    label9.Content = "Por favor insira o e-mail do destinatário";
                }
            }
            else
            {
                label9.Content = "Por favor seleciona um relatório a enviar";
            }
        }

        private void ouvir_voz_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                Display_aux est = (Display_aux)listView1.SelectedItems[0];
                fac.playAudio(est.id);
                label6.Content = "A tocar ficheiro";
            }
            else
            {
                label6.Content = "Por favor selecione um ficheiro a ouvir";
            }
        }
    }
}
