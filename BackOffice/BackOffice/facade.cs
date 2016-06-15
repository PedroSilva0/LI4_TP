using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice
{
    class MyWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 30 * 60 * 1000;
            return w;
        }
    }

    class facade
    {
        private LI4Entities data = new LI4Entities();

                public void registarRes(int id, string nome, string morada, string latitude, string longitude)
        {
            id = Convert.ToInt32(id);
            double latitude2 = Convert.ToDouble(latitude);
            double longitude2 = Convert.ToDouble(longitude);
            data.Estabelecimento.Add(new Estabelecimento()
            {
                id_est = id,
                latitude = latitude2,
                morada = morada,
                nome = nome,
                longitude = longitude2
            });
            data.SaveChanges();
        }

        public List<Estabelecimento> listarRestaurantes()
        {
            //var lista = (from t in data.tasks select new { t.id, t.name, type=t.taskType.name, username = t.user.name }).ToList(); 
            var lista = data.Estabelecimento.ToList();
            return lista;
        }

        public List<Voz> listarTodosVoz()
        {
            //var lista = (from t in data.tasks select new { t.id, t.name, type=t.taskType.name, username = t.user.name }).ToList(); 
            var lista = data.Voz.ToList();
            return lista;
        }

        public List<Visita> listarVisitas()
        {
            var lista2 = data.Visita.SqlQuery("select * from Visita where concluido=1").ToList();
            //var lista = data.Visita.ToList();
            //for(int i=lista.Count()-1;i>=0;i--)
            return lista2;
        }

        public void criarPlano(int fiscal, List<int> estab)
        {
            //var new_id = (from t in data.Estabelecimento select max(id_est)).ToList();
            //return new_id;
            int new_id_pla = data.Plano.Count() + 1;
            int new_id_vis = data.Visita.Count() + 1;
            data.Plano.Add(new Plano()
            {
                id_plano = new_id_pla,
                disponivel = true,
                FiscalCriador = fiscal,
                Fiscal = fiscal                         //ESTA LINHA ESTÁ MAL COM A NOVA BD ATENÇÃO
            });
            foreach (int i in estab)
            {
                //data.Visita.
                data.Visita.Add(new Visita()
                {
                    id_vis = new_id_vis,
                    plano = new_id_pla,
                    estabelecimento = i,
                    concluido = false,
                    dataVisita = DateTime.Now        //ESTA LINHA ESTÁ MAL COM A NOVA BD ATENÇÃO 
                });
                new_id_vis++;
            }
            data.SaveChanges();
        }

        public List<restauranteDTO> listTeste()
        {
            //var lista = (from t in data.tasks select new { t.id, t.name, type=t.taskType.name, username = t.user.name }).ToList(); 
            var lista = data.Estabelecimento.ToList();
            //this.dataGrid.ItemsSource = lista;

            //comboxBox
            var listRest = new List<restauranteDTO>();
            listRest.Add(new restauranteDTO() { id_est = 0, nome = "Todos Restaurante", morada = "sitio", latitude = 0, longitude = 0 });
            var restList = (from u in data.Estabelecimento
                            select new restauranteDTO
                            {
                                id_est = u.id_est,
                                nome = u.nome,
                                morada = u.morada,
                                latitude = u.latitude,
                                longitude = u.longitude
                            }).ToList();
            listRest.AddRange(restList);
            return listRest;
            /*this.comboBox.ItemsSource = listUsers;
            this.comboBox.DisplayMemberPath = "name";
            this.comboBox.SelectedValuePath = "id";

            this.comboBox.SelectedIndex = 0;

            //Combobox (user) insert
            var listaByUser = (from t in data.users
                               select new { t.id, t.name }).ToList();
            var listaByTaskType = (from t in data.taskTypes
                                   select new { t.id, t.name }).ToList();
            this.comboBox1.ItemsSource = listaByTaskType;
            this.comboBox1.DisplayMemberPath = "name";
            this.comboBox1.SelectedValuePath = "id";
            this.comboBox1.SelectedIndex = 0;
            this.comboBox2.ItemsSource = listaByUser;
            this.comboBox2.DisplayMemberPath = "name";
            this.comboBox2.SelectedValuePath = "id";
            this.comboBox2.SelectedIndex = 0;*/
        }

        public string convertXML(int id_voz)
        {
            //buscar a bd, por no pc
            var lista = data.Voz.ToList();
            Voz v = lista.ElementAt(id_voz - 1);
            String path_voz = "C:\\Windows\\Temp\\voz" + v.id_voz.ToString() + ".wav";
            String path_xml= "C:\\Windows\\Temp\\xml" + v.id_voz.ToString() + ".xml";
            System.IO.File.WriteAllBytes(path_voz, v.voz_file);

            // change these:
            String user = "dankrestaurantli4@gmail.com";
            String passwd = "5GPXP87483759G3";
            String wavFile = path_voz;

            // send:
            String url = "https://api.nexiwave.com/SpeechIndexing/file/storage/" + user + "/recording/?authData.passwd=" + passwd + "&response=application/json&targetDecodingConfigName=voicemail&auto-redirect=true";

            // Comment this out to receive transcript in plain text (for SMS, for example)
            //url = url + "&transcriptFormat=html";

            using (MyWebClient wc = new MyWebClient())
            {
                byte[] myByteArray = wc.UploadFile(url, wavFile);
                System.Text.Encoding enc = System.Text.Encoding.ASCII;
                string json = enc.GetString(myByteArray);

                // Note: you will need to add reference to System.Web.Extensions for JavaScriptSerializer
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                Dictionary<string, string> values = serializer.Deserialize<Dictionary<string, string>>(json);
                string transcript = values["text"];

                // perform magic with the transcript here:
                //string text = "A class is the most powerful data type in C#. Like a structure, " +
                // "a class defines the data and behavior of the data type. ";
                //System.IO.File.WriteAllText(path_xml, transcript);
                String res = parse_xml(transcript,id_voz);
                
                Console.Write(transcript);
                return res;
            }

        }
        
        public string parse_xml(string transcript,int id)   //mudar para private depois do teste
        {                                              //mudar string teste para string transcript depois do teste
            String teste = "Access good wheelchair ready Storage no problem Fridge rusty and old must be replaced Kitchen no problem Toilet doesnt flush needs to be fixed";
            teste.ToLower();
            String res = "";
            string[] ssize = teste.Split(null);
            foreach(String s in ssize)
            {
                if (s.Equals("access")){

                }
            }
            return res;
        }

        public void playAudio(int id_voz)
        {
            //Console.WriteLine(id_voz);
            var lista = data.Voz.ToList();
            Voz v = lista.ElementAt(id_voz - 1);
            var stream = new MemoryStream(v.voz_file,true);
            //System.IO.File.WriteAllBytes("C:\\Windows\\Temp\\" + v.id_voz.ToString() + ".wav", v.voz_file);
            SoundPlayer simpleSound = new SoundPlayer(stream);
            simpleSound.Stream.Seek(0, SeekOrigin.Begin);
            simpleSound.Stream.Write(v.voz_file, 0, v.voz_file.Length);
            simpleSound.Play();
            //simpleSound.Load();
            //simpleSound.PlaySync();
            Console.WriteLine("ficheiro tocado");
        }
    }
}
