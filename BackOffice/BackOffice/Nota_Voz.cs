using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;
using System.Speech.AudioFormat;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace BackOffice
{
    class Nota_Voz
    {
        private int id_voz;
        private LI4Entities data = new LI4Entities();
        private string transcript;

        public Nota_Voz(int id)
        {
            id_voz = id;
        }

        public void convert()
        {
            Voz v = data.Voz.Find(id_voz);
            String path_voz = "C:\\Windows\\Temp\\voz" + v.id_voz.ToString() + ".wav";
            //String path_xml = "C:\\Windows\\Temp\\xml" + v.id_voz.ToString() + ".xml";
            System.IO.File.WriteAllBytes(path_voz, v.voz_file);
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
            Grammar gr = new DictationGrammar();
            sre.LoadGrammar(gr);
            sre.SetInputToWaveFile(path_voz);
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
            transcript = sb.ToString();
            v.xml_file = transcript;
            data.SaveChanges();
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
                        //Console.Write((!(ssize[i].Equals("storage")) || !(ssize[i].Equals("frigde")) || !(ssize[i].Equals("kitchen")) || !(ssize[i].Equals("toilet"))));
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
                i--;
            }
            return res;
        }
    }
}
