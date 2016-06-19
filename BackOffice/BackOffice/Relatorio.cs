using PdfSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace BackOffice
{
    class Relatorio
    {
        private LI4Entities data;
        private int myVisita;

        public Relatorio(int idVisita)
        {
            data = new LI4Entities();
            myVisita = idVisita;
        }

        public Relatorio()
        {
            data = new LI4Entities();

        }

        private string loadTemplate()
        {
            string html = "<div id=\"header\">\n<h1>Relatório de Inspeção</h1>\n</div>\n\n<div id=\"nav\">\n<i>Estabelecimento: </i><!--estabelecimento--> -- <i>Morada: </i><!--morada--> -- <i>Coordenadas: </i><!--coordenadas--><br>\n<i>Data da visita: </i><!--dataVisita--> -- <i>No. fiscal: </i><!--fiscal--> -- <i>Data do relatório: </i><!--dataRel-->\n</div>\n\n<div id=\"section\">\n\n<h2>Formulário da visita</h2>\n<!--formulario-->\n\n<h2>Notas da visita</h2>\n<!--notasTxt-->\n<!--notasVoz-->\n\n<h2>Registo Fotográfico</h2>\n<!--fotos-->\n";
            return html;
        }

        public string loadCabecalho()
        {
            string html = "<head>\n<meta charset=\"UTF-8\">\n<title>Relatório de Inspeção --- Dank Restaurant</title>\n<style>\n#header {\n    background-color:black;\n    color:white;\n    text-align:center;\n    padding:5px;\n}\n#nav {\n    line-height:30px;\n    background-color:#eeeeee;\n    width:100%;\n    float:left;\n    padding:5px; \n}\n#section {\n    width:100%;\n    float:left;\n    padding:10px; \n}\n#footer {\n    background-color:black;\n    color:white;\n    clear:both;\n    text-align:center;\n    padding:5px; \n}\n</style>\n</head>\n";
            return html;
        }

        public string observacoes(string obs)
        {
            string res = "<h2>Observações</h2>\n" + obs + "\n</div>\n<div id=\"footer\">\nDank Restaurant\n</div>";
            return res;
        }

        public string classificacao(string cla)
        {
            string res = "<h2>Classificação</h2>\n" + cla + "\n";
            return res;
        }

        public string criarRelatorio()
        {
            string res = loadTemplate();

            //colocar a data de hoje
            res = res.Replace("<!--dataRel-->", DateTime.Now.ToShortDateString());

            Visita vis = new Visita();
            vis = data.Visita.Find(myVisita);

            Estabelecimento est = new Estabelecimento();
            est = data.Estabelecimento.Find(vis.estabelecimento);
            //preencher os campos do estabelecimento
            res = res.Replace("<!--estabelecimento-->", est.nome);
            res = res.Replace("<!--morada-->", est.morada);
            res = res.Replace("<!--coordenadas-->", est.latitude.ToString() + ", " + est.longitude.ToString());

            Plano pla = new Plano();
            pla = data.Plano.Find(vis.plano);

            //preencher os campos da visita
            res = res.Replace("<!--dataVisita-->", ((DateTime)vis.dataVisita).ToShortDateString());
            res = res.Replace("<!--fiscal-->", pla.Fiscal.ToString());

            //formulario
            var respostas = (from f in data.VisitaQuestao
                             where f.Visita == myVisita
                             select new { f.Visita, f.Questao, f.Resposta }
                            ).ToList();
            if (respostas.Count != 0) //fazer skip de null
            {
                StringBuilder form = new StringBuilder();
                foreach (var r in respostas)
                {
                    form.Append("<i>").Append(data.Questao.Find(r.Questao).pergunta).Append("</i><br>\n");
                    form.Append(r.Resposta).Append("<br><br><br>\n");
                }
                res = res.Replace("<!--formulario-->", form.ToString());
            }

            //notasTXT
            var notas = (from n in data.Nota
                         where n.visita == myVisita
                         select new { n.id_nota, n.descricao, n.text_file, n.visita }
                         ).ToList();
            if (notas.Count != 0)
            { //skip null
                StringBuilder notasStr = new StringBuilder();
                foreach (var n in notas)
                {
                    notasStr.Append("<i>").Append(n.descricao).Append(":</i><br>\n");
                    notasStr.Append(n.text_file).Append("<br><br>\n");
                }
                res = res.Replace("<!--notasTxt-->", notasStr.ToString());
            }

            //notasVoz
            var notasVoz = (from n in data.Voz
                            where n.Visita == myVisita
                            select new { n.id_voz, n.descricao, n.xml_file, n.Visita }
                            ).ToList();
            if (notas.Count != 0)
            { //skip null
                StringBuilder notasVozStr = new StringBuilder();
                foreach (var n in notasVoz)
                {
                    if (n.xml_file == null)
                    {
                        Nota_Voz aux = new Nota_Voz(n.id_voz);
                        Thread t = new Thread(aux.convert);
                        t.Start();
                        t.Join();
                        notasVozStr.Append("<i>").Append(n.descricao).Append(":</i><br>\n");
                        notasVozStr.Append(aux.getTranscript()).Append("<br><br>\n");
                    }
                    else
                    {
                        notasVozStr.Append("<i>").Append(n.descricao).Append(":</i><br>\n");
                        notasVozStr.Append(n.xml_file).Append("<br><br>\n");
                    }

                }
                res = res.Replace("<!--notasVoz-->", notasVozStr.ToString());
            }

            //Fotos
            var fotos = (from f in data.Foto
                         where f.visita == myVisita
                         select new { f.id_foto, f.descricao, f.foto_file, f.visita }
                        ).ToList();

            if (fotos.Count != 0)//skip null
            {
                StringBuilder fotosStr = new StringBuilder();
                foreach (var f in fotos)
                {
                    System.IO.File.WriteAllBytes("C:\\Windows\\Temp\\" + f.id_foto.ToString() + ".jpg", f.foto_file);
                    fotosStr.Append("<img src=\"file:///C:/Windows/Temp/").Append(f.id_foto.ToString() + ".jpg").Append("\" width=\"70%\"/>").Append("<br>\n");
                    fotosStr.Append(f.descricao).Append("<br>\n");
                }
                res = res.Replace("<!--fotos-->", fotosStr.ToString());
            }

            return res;
        }

        public void guardarRelatorio(string rel, string cla)
        {
            Visita v = data.Visita.Find(myVisita);
            v.html_file = rel;
            v.dataRelatorio = DateTime.Now;
            v.classificacao = cla;
            data.SaveChanges();
        }

        public bool enviarRelatorio(string para, string desc)
        {
            try
            {
                SmtpClient mailServer = new SmtpClient("smtp.gmail.com", 587);
                mailServer.EnableSsl = true;
                mailServer.Credentials = new System.Net.NetworkCredential("dankrestaurantli4@gmail.com", "li42016dk");

                string from = "dankrestaurantli4@gmail.com";
                string to = para;
                MailMessage msg = new MailMessage(from, to);
                msg.Subject = "Relatório de inspeção, " + desc;                          //assunto
                msg.Body = "Segue em anexo o relatório de inspeção," + desc + "\n\n\nDankRestaurantInspections";//corpo da mensagem

                string attach = this.geraPDF();

                Attachment a = new Attachment(attach);
                msg.Attachments.Add(a); //anexo


                mailServer.Send(msg);
                a.Dispose();
                mailServer.Dispose();

            }
            catch (SmtpFailedRecipientException ex)
            {
                Console.WriteLine("Unable to send email. Error : " + ex);
                return false;
            }
            return true;
        }

        /**
         * @return filepath
         * */
        private string geraPDF()
        {

            string html = data.Visita.Find(myVisita).html_file;
            string filePath = "C:\\Windows\\Temp\\RelatorioVisita" + myVisita.ToString() + ".pdf";
            PdfSharp.Pdf.PdfDocument pdf;

            pdf = PdfGenerator.GeneratePdf(html, PageSize.A4);
            pdf.Save(filePath);
            pdf.Close();
            return filePath;
        }

        public List<Display_aux> listarRelatorios()
        {
            List<Display_aux> res = new List<Display_aux>();


            var lista = data.Visita.SqlQuery("select * from Visita where concluido=1 and dataRelatorio is not null").ToList();

            
            foreach (Visita item in lista)
            {
                Estabelecimento aux = data.Estabelecimento.Find(item.estabelecimento);
                string itDesc = aux.nome + ", " + ((DateTime)item.dataRelatorio).ToShortDateString();
                res.Add(new Display_aux { id = item.id_vis, desc = itDesc });
            }
           


            return res;
        }

    }
}
