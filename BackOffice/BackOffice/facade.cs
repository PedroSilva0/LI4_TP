using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Mail;
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

        /*public List<Visita> listarVisitas()
        {
            var lista2 = data.Visita.SqlQuery("select * from Visita where concluido=1").ToList();
            //var lista = data.Visita.ToList();
            //for(int i=lista.Count()-1;i>=0;i--)
            return lista2;
        }*/

        public List<visitaDTO> listarVisitas()
        {
            var lista2 = data.Visita.SqlQuery("select * from Visita where concluido=1").ToList();
            List<visitaDTO> res = new List<visitaDTO>();
            foreach (Visita item in lista2)
            {
                Estabelecimento aux = data.Estabelecimento.Find(item.estabelecimento);
                //String des = "" + aux.nome + " - " + item.dataVisita.Date.ToString("d");

                string itDesc = aux.nome + ", " + ((DateTime)item.dataVisita).ToShortDateString();
                res.Add(new visitaDTO { id_vis = item.id_vis, desc = itDesc });
            }

            return res;
        }

        public List<visitaDTO> listarRelatorios()
        {
            List<Visita> lista = data.Visita.SqlQuery("select * from Visita where dataRelatorio is not null").ToList();
            List<visitaDTO> res = new List<visitaDTO>();
            foreach (Visita item in lista)
            {
                Estabelecimento aux = data.Estabelecimento.Find(item.estabelecimento);
                string itDesc = aux.nome + ", " + ((DateTime)item.dataRelatorio).ToShortDateString();
                res.Add(new visitaDTO { id_vis = item.id_vis, desc = itDesc });
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

        public string convertXML(int id_voz)
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
                //System.IO.File.WriteAllText(path_xml, transcript);
                String res = parse_xml(transcript, id_voz);

                //Console.Write(transcript);
                return res;
            }

        }

        private string parse_xml(string transcript, int id)
        {                                              //mudar string teste para string transcript depois do teste
            //string teste = "Access good wheelchair ready Storage no problem Fridge rusty and old must be replaced Kitchen no problem Toilet doesnt flush needs to be fixed";
            transcript = transcript.ToLower();
            //Console.Write(teste);
            string res = "";
            string[] ssize = transcript.Split(null);
            for (int i = 0; i < ssize.Count(); i++)
            {
                if (ssize[i].Equals("access")) {
                    res = res + "<access>\n\t<content=\"";
                    i++;
                    while ((i < ssize.Count()) && (!(ssize[i].Equals("storage")) && !(ssize[i].Equals("frigde")) && !(ssize[i].Equals("kitchen")) && !(ssize[i].Equals("toilet"))))
                    {
                        //Console.Write((!(ssize[i].Equals("storage")) || !(ssize[i].Equals("frigde")) || !(ssize[i].Equals("kitchen")) || !(ssize[i].Equals("toilet"))));
                        res = res + " " + ssize[i];
                        i++;
                    }
                    res = res + "\"/>\n</access>\n";
                } else
                if (ssize[i].Equals("storage"))
                {
                    res = res + "<storage>\n\t<content=\"";
                    i++;
                    while ((i < ssize.Count()) && (!(ssize[i].Equals("access")) && !(ssize[i].Equals("frigde")) && !(ssize[i].Equals("kitchen")) && !(ssize[i].Equals("toilet"))))
                    {
                        res = res + " " + ssize[i];
                        i++;
                    }
                    res = res + "\"/>\n</storage>\n";
                } else
                if (ssize[i].Equals("fridge"))
                {
                    res = res + "<fridge>\n\t<content=\"";
                    i++;
                    while ((i < ssize.Count()) && (!(ssize[i].Equals("storage")) && !(ssize[i].Equals("access")) && !(ssize[i].Equals("kitchen")) && !(ssize[i].Equals("toilet"))))
                    {
                        res = res + " " + ssize[i];
                        i++;
                    }
                    res = res + "\"/>\n</fridge>\n";
                } else
                if (ssize[i].Equals("kitchen"))
                {
                    res = res + "<kitchen>\n\t<content=\"";
                    i++;
                    while ((i < ssize.Count()) && (!(ssize[i].Equals("storage")) && !(ssize[i].Equals("frigde")) && !(ssize[i].Equals("access")) && !(ssize[i].Equals("toilet"))))
                    {
                        res = res + " " + ssize[i];
                        i++;
                    }
                    res = res + "\"/>\n</kitchen>\n";
                } else
                if (ssize[i].Equals("toilet"))
                {
                    res = res + "<toilet>\n\t<content=\"";
                    i++;
                    while ((i < ssize.Count()) && (!(ssize[i].Equals("storage")) && !(ssize[i].Equals("frigde")) && !(ssize[i].Equals("kitchen")) && !(ssize[i].Equals("access"))))
                    {
                        res = res + " " + ssize[i];
                        i++;
                    }
                    res = res + "\"/>\n</toilet>\n";
                }
                i--;
            }
            Voz v = data.Voz.Find(id);
            v.xml_file = res;
            data.SaveChanges();
            return res;
        }

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

        

        //nao da
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
