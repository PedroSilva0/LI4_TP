using System;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Media;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace BackOffice
{
    class Nota_Voz
    {
        private int id_voz;
        private LI4Entities data = new LI4Entities();
        private string transcript;


        public string getTranscript()
        {
            return transcript;
        }

        public Nota_Voz(int id)
        {
            id_voz = id;
        }

        public Nota_Voz()
        {

        }

        private void convert3gppToWav(string inFile, string outFile)
        {

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C ffmpeg -y -i "+ inFile + " -acodec pcm_u8 "+ outFile;
            process.StartInfo = startInfo;
        
            process.Start();
       
           // process.WaitForExit(2000);

        }

     

        public void convert()
        {
            Voz v = data.Voz.Find(id_voz);

            if (v.xml_file != null)//skip se ja converteu
            {
                this.transcript = v.xml_file;
            }
            else
            {
                String file3gpp = "C:\\Windows\\Temp\\voz" + v.id_voz.ToString() + ".3gpp";
                String fileWav = "C:\\Windows\\Temp\\voz" + v.id_voz.ToString() + ".wav";
                System.IO.File.WriteAllBytes(file3gpp, v.voz_file);
                convert3gppToWav(file3gpp, fileWav);

                CultureInfo c = new System.Globalization.CultureInfo("en-US");
                SpeechRecognitionEngine sre = new SpeechRecognitionEngine(c);
                Grammar gr = new DictationGrammar();
                sre.LoadGrammar(gr);
                sre.SetInputToWaveFile(fileWav);
                sre.BabbleTimeout = new TimeSpan(Int32.MaxValue);
                sre.InitialSilenceTimeout = new TimeSpan(Int32.MaxValue);
                sre.EndSilenceTimeout = new TimeSpan(100000000);
                sre.EndSilenceTimeoutAmbiguous = new TimeSpan(100000000);
                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    try
                    {
                        RecognitionResult recText = sre.Recognize();
                        if (recText == null)
                        {
                            break;
                        }
                        sb.Append(recText.Text);
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }

                sre.UnloadAllGrammars();
                transcript = sb.ToString().ToLower();
                v.xml_file = transcript;
                data.SaveChanges();
            }
        }

        public string parse_xml()
        {

            string res = "";
            string[] ssize = transcript.Split(null);
            for (int i = 0; i < ssize.Count(); i++)
            {
                if (ssize[i].Equals("access"))
                {
                    res = res + "<access>\n\t<content=\"";
                    i++;
                    while ((i < ssize.Count()) && (!(ssize[i].Equals("storage")) && !(ssize[i].Equals("frigde")) && !(ssize[i].Equals("kitchen")) && !(ssize[i].Equals("toilet"))))
                    {
                        res = res + " " + ssize[i];
                        i++;
                    }
                    res = res + "\"/>\n</access>\n";
                }
                else
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
                }
                else
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
                }
                else
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
                }
                else
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
                else //sem match
                {
                    res = res + " " + ssize[i] + " ";
                }
                //i--;
            }
            return res;
        }

        public List<Display_aux> listarTodosVoz()
        {
            var lista = data.Voz.ToList();
            List<Display_aux> res = new List<Display_aux>();
            foreach (Voz v in lista)
            {
                Visita vi = data.Visita.Find(v.Visita);
                if (vi.concluido)
                {
                    Estabelecimento aux = data.Estabelecimento.Find(vi.estabelecimento);
                    string itDesc = aux.nome + ", " + ((DateTime)vi.dataVisita).ToShortDateString() + ", " + v.descricao;
                    res.Add(new Display_aux { id = v.id_voz, desc = itDesc });
                }
            }
            return res;
        }

        public void playAudio()
        {
            Voz v = data.Voz.Find(id_voz);
            //var stream = new MemoryStream(v.voz_file, true);
            //SoundPlayer simpleSound = new SoundPlayer(stream);

            String file3gpp = "C:\\Windows\\Temp\\voz" + v.id_voz.ToString() + ".3gpp";
            String fileWav = "C:\\Windows\\Temp\\voz" + v.id_voz.ToString() + ".wav";

            if (!File.Exists(fileWav))
            {
                System.IO.File.WriteAllBytes(file3gpp, v.voz_file);
                convert3gppToWav(file3gpp, fileWav);
            }

            SoundPlayer simpleSound = new SoundPlayer(fileWav);
            simpleSound.Play();
        }

    }
}
