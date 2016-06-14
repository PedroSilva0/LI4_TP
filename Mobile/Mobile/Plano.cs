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
    class Plano
    {
        public int id { get; }
        public bool disponivel { get; }
        public int fiscalCria { get; }
        public int fiscalExecuta { get; }
    }
}