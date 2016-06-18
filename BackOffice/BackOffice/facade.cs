using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;

namespace BackOffice
{


    class facade
    {
        private EstabelecimentoDTO est = new EstabelecimentoDTO();
        private Nota_Voz voz;
        private PlanoDTO planos = new PlanoDTO();
        private Relatorio rel = new Relatorio();
        private Fiscais fisc = new Fiscais();

        public void registarRes(string nome, string morada, double latitude, double longitude)
        {
            est.registarRes(nome, morada, latitude, longitude);
        }

        public bool demasiado_perto(double latitude, double longitude)
        {
            return est.demasiado_perto(latitude, longitude);
        }

        public List<Estabelecimento> listarRestaurantes()
        {
            return est.listarRestaurantes();
        }

        public List<Display_aux> listarTodosVoz()
        {
            voz = new Nota_Voz();
            return voz.listarTodosVoz();
        }


        public List<Display_aux> listarVisitas()
        {
            return planos.listarVisitas();
        }

        public List<Display_aux> listarRelatorios()
        {
            return rel.listarRelatorios();
        }

        public void criarPlano(int fiscal, List<int> estab)
        {
            planos.criarPlano(fiscal, estab);
        }

        public bool login(int id_fiscal, string password)
        {
            return fisc.login(id_fiscal, password);
        }

        public void playAudio(int id_voz)
        {
            voz = new Nota_Voz();
            voz.playAudio(id_voz);
        }

        public string converter_xml(int e)
        {
            Nota_Voz c = new Nota_Voz(e);
            Thread t = new Thread(c.convert);
            t.Start();
            t.Join();
            return c.parse_xml();
        }

        public void enviar_relatorio(int e, string mail,string desc)
        {
            Relatorio r = new Relatorio(e);
            r.enviarRelatorio(mail,desc );
        }
    }
}
