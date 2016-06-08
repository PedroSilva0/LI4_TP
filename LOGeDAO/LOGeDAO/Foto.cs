using System;
using System.Collections.Generic;
using System.Text;

namespace LOGeDAO
{
    class Foto
    {
        private int id { get; }
        private string descricao { get; set; }

        public Foto(int mId, string mDesc)
        {
            id = mId;
            descricao = mDesc;
        }

        public void getFile()
        {

        }
    }
}
