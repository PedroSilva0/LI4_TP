using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice
{
   

    class PlanoDTO
    {
        private LI4Entities data = new LI4Entities();

        public List<Display_aux> listarVisitas()
        {
            var lista2 = data.Visita.SqlQuery("select * from Visita where concluido=1 and dataRelatorio is null").ToList();
            List<Display_aux> res = new List<Display_aux>();
            foreach (Visita item in lista2)
            {
                Estabelecimento aux = data.Estabelecimento.Find(item.estabelecimento);
                string itDesc = aux.nome + ", " + ((DateTime)item.dataVisita).ToShortDateString();
                res.Add(new Display_aux { id = item.id_vis, desc = itDesc });
            }

            return res;
        }

        public void criarPlano(int fiscal, List<int> estab)
        {
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
    }
}
