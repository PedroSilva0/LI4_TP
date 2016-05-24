using System;
using System.Collections.Generic;
using System.Text;

namespace LOGeDAO
{
    class NotaTXT
    {
        private int id { get;  }
        private string descricao { get; set; }
        private string texto { get; set; }

        public NotaTXT(int mId, string mDesc, string mTXT)
        {
            id = mId;
            descricao = mDesc;
            texto = mTXT;
        }
    }
}
