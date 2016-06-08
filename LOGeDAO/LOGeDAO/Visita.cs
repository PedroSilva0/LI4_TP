using System;
using System.Collections.Generic;
using System.Text;

namespace LOGeDAO
{
    class Visita
    {
        private int id { get; }
        private DateTime data { get; set; }
        private bool concluida { get; set; }
        private List<Foto> fotos { get; }
        private List<NotaTXT> notas { get; }
        private List<NotaVoz> audios { get; }
        private List<Resposta> formulario { get; }
        private Relatorio relatorio { get; }
        private Restaurante restaurante { get; }

        public Visita(int mId)
        {
            id = mId;
            data = DateTime.Now;
            concluida = false;
            fotos = new List<Foto>();
            notas = new List<NotaTXT>();
            audios = new List<NotaVoz>();
            formulario = new List<Resposta>();
            relatorio = new Relatorio();


        }
    }
}
