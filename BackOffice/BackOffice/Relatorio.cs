using PdfSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                             select new { f.Visita, f.Questao, f.resposta }
                            ).ToList();
            if (respostas.Count != 0) //fazer skip de null
            {
                StringBuilder form = new StringBuilder();
                foreach (var r in respostas)
                {
                    form.Append("<i>").Append(data.Questao.Find(r.Questao).pergunta).Append("</i><br>\n");
                    form.Append(r.resposta).Append("<br><br>\n");
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
                    notasStr.Append(n.text_file).Append("<br>\n");
                }
                res = res.Replace("<!--notasTxt-->", notasStr.ToString());
            }
            //notasVoz TODO

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
                    /*
                    string imageBase64 = Convert.ToBase64String(f.foto_file);
                    string imageSrc = string.Format("data:image/jpg;base64,{0}", imageBase64);
                    fotosStr.Append("<img src=\"").Append(imageSrc).Append("\" width=\"80%\"/>").Append("<br>\n");
                    fotosStr.Append(f.descricao).Append("<br>\n");
                    */


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

            return filePath;
        }



    }
}
