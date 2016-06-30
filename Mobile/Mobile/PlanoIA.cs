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
using Android.Views.InputMethods;

namespace Mobile
{
    [Activity(Label = "Plano de Trabalho", Icon = "@android:color/transparent")]
    public class PlanoIA : Activity
    {
        private Button btnInfo;
        private Button btnAccept;
        private TextView txtNr;
        private string mNr;
        private string mId;
        private string mFiscal;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PlanoInfoAccept);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            txtNr = FindViewById<TextView>(Resource.Id.txtPlanoNr);
            btnInfo = FindViewById<Button>(Resource.Id.btnInfo);
            btnAccept = FindViewById<Button>(Resource.Id.btnAccept);
            mFiscal = Intent.GetStringExtra("Fiscal");
           

            mId = Intent.GetStringExtra("Id");
            mNr = Intent.GetStringExtra("Pos");
            txtNr.Text = mNr;
            btnInfo.Click += BtnInfo_Click;
            btnAccept.Click += BtnAccept_Click;
            
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

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok && requestCode == 50)
            {
                Finish();
            }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            PlanoA planoA = new PlanoA(mId,mFiscal);
            planoA.Show(transaction, "accept");
        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(PlanoI));
            intent.PutExtra("Id", mId);
            this.StartActivity(intent);
        }
    }
}