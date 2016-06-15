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

        public void convertXML()
        {
            // change these:
            String user = "dankrestaurantli4@gmail.com";
            String passwd = "5GPXP87483759G3";
            String wavFile = "C:\\li4\\teste2.wav";

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
                System.IO.File.WriteAllText(@"C:\li4\testept.txt", transcript);
                Console.Write(transcript);
            }

        }

        public void playAudio(int id_voz)
        {
            Console.WriteLine(id_voz);
            var lista = data.Voz.ToList();
            //byte[] teste = (byte[]) audioFile.ElementAt(0);
            //audioFile.ElementAt(0);
            Stream stream = new MemoryStream(lista.ElementAt(id_voz-1).voz_file);
            //audioFile.Stream();
            //System.IO.File.WriteAllBytes(f.id_foto.ToString() + ".jpg", f.foto_file);
            SoundPlayer simpleSound = new SoundPlayer(@"C:\temp\teste1.wav");
            simpleSound.Load();
            simpleSound.Play();
            Console.WriteLine("ficheiro tocado");
        }
    }
}
