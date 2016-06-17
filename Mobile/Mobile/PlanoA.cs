using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;

namespace Mobile
{
    public class PlanoA : DialogFragment
    {
        private Button mBtnAccept;
        private Button mBtnCancel;
        private string mPlano;
        private string mFiscal;

        public PlanoA(string plano, string fiscal)
        {
            mPlano = plano;
            mFiscal = fiscal;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);

            var view = inflater.Inflate(Resource.Layout.dialog_accept, container, false);
            mBtnAccept = view.FindViewById<Button>(Resource.Id.btnAccept);
            mBtnCancel = view.FindViewById<Button>(Resource.Id.btnCancel);

            mBtnAccept.Click += MBtnAccept_Click;
            mBtnCancel.Click += MBtnCancel_Click;

            return view;
        }

        private void MBtnCancel_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

        private void MBtnAccept_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this.Activity, typeof(Visitas));
            intent.PutExtra("Id", mPlano);
            intent.PutExtra("Fiscal", mFiscal);
            this.Activity.StartActivity(intent);
        }
    }
}