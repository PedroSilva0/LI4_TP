using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice
{
    
    class facade
    {
        private LI4Entities data=new LI4Entities();

        public void registarRes(int id,string nome,string morada,string latitude,string longitude)
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


    }
}
