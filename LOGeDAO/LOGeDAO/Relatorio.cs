using System;
using System.Collections.Generic;
using System.Text;

namespace LOGeDAO
{
    class Relatorio
    {
        private DateTime data { get; set; }
        private string classificacao { get; set; }
        private string observacoes { get; set; }
        private string html { get; set; }

        public Relatorio()
        {
            data = DateTime.Now;
            classificacao = null;
            observacoes = null;
            html = null;

        }

        public void enviarRelatorio(string destino)
        {

        }

        public string consultarRelatorio()
        {
            return this.html;
        }

    }
}
