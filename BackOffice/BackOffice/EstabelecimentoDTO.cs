using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice
{
    class EstabelecimentoDTO
    {
        private LI4Entities data = new LI4Entities();

        public void registarRes(string nome, string morada, double latitude, double longitude)
        {
            int id = data.Estabelecimento.Count() + 1;

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

        public bool demasiado_perto(double latitude, double longitude)
        {
            var lista = data.Estabelecimento.ToList();
            foreach (Estabelecimento e in lista)
            {
                if ((Math.Abs(e.latitude - latitude) + Math.Abs(e.longitude - longitude)) < 0.1)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Estabelecimento> listarRestaurantes()
        {
            var lista = data.Estabelecimento.ToList();
            return lista;
        }

    }
}
