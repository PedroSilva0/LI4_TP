using System;
using System.Collections.Generic;
using System.Text;

namespace LOGeDAO
{
    class Restaurante
    {
        private int id { get; set; }
        private string nome { get; set; }
        private string morada { get; set; }
        private GPS coordenadas { get; set; }

        public Restaurante(int mId, string mNome, string mMorada, float longitude, float latitude)
        {
            id = mId;
            nome = mNome;
            morada = mMorada;
            coordenadas = new GPS(longitude, latitude);
             
        }
    }
}



