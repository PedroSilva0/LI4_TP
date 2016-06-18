using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice
{
    class Fiscais
    {
        private LI4Entities data = new LI4Entities();

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

    }
}
