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
    [Activity(Label = "Visita", Icon = "@android:color/transparent")]
    public class VisitasGPSA : Activity
    {
        private string mEstabelecimento;
        private TextView estabelecimentoTV;
        private Button mIniciar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.VisitasGPSAccept);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            estabelecimentoTV = FindViewById<TextView>(Resource.Id.txtEstabelecimento);
            mEstabelecimento = Intent.GetStringExtra("Nome");
            estabelecimentoTV.Text = mEstabelecimento;
            mIniciar = FindViewById<Button>(Resource.Id.btnIniciar);
            mIniciar.Click += MIniciar_Click;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void MIniciar_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PVFN));
            this.StartActivity(intent);
        }
    }
}