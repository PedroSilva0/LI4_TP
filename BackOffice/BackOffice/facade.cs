using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Speech;
using System.Speech.Recognition;

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

        public void registarRes(string nome, string morada, double latitude, double longitude)
        {
            int id = data.Estabelecimento.Count()+1;
            
            data.Estabelecimento.Add(new Estabelecimento()
            {
                id_est = id,
                latitude = latitude,
                morada = morada,
                nome = nome,
                longitude = longitude
            });
            data.SaveChanges();
        }

        public bool demasiado_perto(double latitude,double longitude)
        {
            var lista = data.Estabelecimento.ToList();
            foreach(Estabelecimento e in lista)
            {
                if(e.latitude==latitude && e.longitude == longitude)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Estabelecimento> listarRestaurantes()
        {
            //var lista = (from t in data.tasks select new { t.id, t.name, type=t.taskType.name, username = t.user.name }).ToList(); 
            var lista = data.Estabelecimento.ToList();
            return lista;
        }

        public List<Display_aux> listarTodosVoz()
        {
            //var lista = (from t in data.tasks select new { t.id, t.name, type=t.taskType.name, username = t.user.name }).ToList(); 
            var lista = data.Voz.ToList();
            List<Display_aux> res = new List<Display_aux>();
            foreach(Voz v in lista)
            {
                Visita vi = data.Visita.Find(v.Visita);
                if (vi.concluido)
                {
                    Estabelecimento aux = data.Estabelecimento.Find(vi.estabelecimento);
                    //String des = "" + aux.nome + " - " + item.dataVisita.Date.ToString("d");

                    string itDesc = aux.nome + ", " + ((DateTime)vi.dataVisita).ToShortDateString()+", "+v.descricao;
                    res.Add(new Display_aux { id = v.id_voz, desc = itDesc });

                }
            }
            return res;
        }

        /*public List<Visita> listarVisitas()
        {
            var lista2 = data.Visita.SqlQuery("select * from Visita where concluido=1").ToList();
            //var lista = data.Visita.ToList();
            //for(int i=lista.Count()-1;i>=0;i--)
            return lista2;
        }*/

        public List<Display_aux> listarVisitas()
        {
            var lista2 = data.Visita.SqlQuery("select * from Visita where concluido=1").ToList();
            List<Display_aux> res = new List<Display_aux>();
            foreach (Visita item in lista2)
            {
                Estabelecimento aux = data.Estabelecimento.Find(item.estabelecimento);
                //String des = "" + aux.nome + " - " + item.dataVisita.Date.ToString("d");

                string itDesc = aux.nome + ", " + ((DateTime)item.dataVisita).ToShortDateString();
                res.Add(new Display_aux { id = item.id_vis, desc = itDesc });
            }

            return res;
        }

        public List<Display_aux> listarRelatorios()
        {
            List<Visita> lista = data.Visita.SqlQuery("select * from Visita where dataRelatorio is not null").ToList();
            List<Display_aux> res = new List<Display_aux>();
            foreach (Visita item in lista)
            {
                Estabelecimento aux = data.Estabelecimento.Find(item.estabelecimento);
                string itDesc = aux.nome + ", " + ((DateTime)item.dataRelatorio).ToShortDateString();
                res.Add(new Display_aux { id = item.id_vis, desc = itDesc });
            }

            return res;
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
                FiscalCriador = fiscal
            });
            foreach (int i in estab)
            {
                //data.Visita.
                data.Visita.Add(new Visita()
                {
                    id_vis = new_id_vis,
                    plano = new_id_pla,
                    estabelecimento = i,
                    concluido = false

                });
                new_id_vis++;
            }
            data.SaveChanges();
        }

        /*public string convertXML(int id_voz)
        {
            //buscar a bd, por no pc
            var lista = data.Voz.ToList();
            Voz v = lista.ElementAt(id_voz - 1);
            String path_voz = "C:\\Windows\\Temp\\voz" + v.id_voz.ToString() + ".wav";
            String path_xml = "C:\\Windows\\Temp\\xml" + v.id_voz.ToString() + ".xml";
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
                Voz voz = data.Voz.Find(id_voz);
                transcript = transcript.ToLower();
                voz.xml_file = transcript;
                data.SaveChanges();
                //System.IO.File.WriteAllText(path_xml, transcript);
                String res = parse_xml(transcript, id_voz);

                //Console.Write(transcript);
                return res;
            }

        }*/
        

        public bool login(int id_fiscal, string password)
        {
            Fiscal f = data.Fiscal.Find(id_fiscal);
            if (f != null)
            {
                if (f.pass.Equals(password))
                {
                    return true;
                }
            }
            return false;
        }

        

        
        public void playAudio(int id_voz)
        {
            //Console.WriteLine(id_voz);
            //var file = File.ReadAllBytes("C: \\li4\\teste6.mp3");
            //var lista = data.Voz.ToList();
            Voz v = data.Voz.Find(id_voz);
            var stream = new MemoryStream(v.voz_file,true);
            //System.IO.File.WriteAllBytes("C:\\Windows\\Temp\\" + v.id_voz.ToString() + ".wav", v.voz_file);
            SoundPlayer simpleSound = new SoundPlayer(stream);
            //simpleSound.Stream.Seek(0, SeekOrigin.Begin);
            //simpleSound.Stream.Write(v.voz_file, 0, v.voz_file.Length);
            simpleSound.Play();
            //simpleSound.Load();
            //simpleSound.PlaySync();
            /*foreach(RecognizerInfo ri in SpeechRecognitionEngine.InstalledRecognizers())
            {
                Console.WriteLine(ri.Culture.Name);
            }*/
            //Console.WriteLine("ficheiro tocado");
        }

    }
}
