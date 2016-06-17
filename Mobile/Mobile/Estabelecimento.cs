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
    class Estabelecimento
    {
        private int id { get; set; }
        public string nome { get; set; }
        private string morada { get; set; }
        private float latitude { get; set; }
        private float longitude { get; set; }
    }
}