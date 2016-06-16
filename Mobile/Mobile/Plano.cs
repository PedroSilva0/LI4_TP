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
    public class Plano
    {
        private int id { get; }
        private bool disponivel { get; }
        private int fiscalCria { get; }
        private int fiscalExecuta { get; }
    }
}