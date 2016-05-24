using System;
using System.Collections.Generic;
using System.Text;

namespace LOGeDAO
{
    class Resposta
    {
        private int idQuestao { get; }
        private string resposta { get; set; }

        public Resposta(int mQuestao, string mResp)
        {
            idQuestao = mQuestao;
            resposta = mResp;
        }
    }
}
