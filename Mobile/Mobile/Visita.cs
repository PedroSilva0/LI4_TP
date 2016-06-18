using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Mobile
{
    class Visita
    {
        public int id_vis { get; set; }
        private string dataVisita { get; set; }
        private int plano { get; set; }
        private int estabelecimento { get; set; }
        private string dataRelatorio { get; set; }
        private string classificacao { get; set; }
        private string html_file { get; set; }
        private bool concluido { get; set; }

    }
}