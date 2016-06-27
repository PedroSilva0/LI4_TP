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
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Android.Support.V7.Widget;
using Android.Support.V4.Widget;
using System.ComponentModel;
using Java.Lang;
using Android.Views.InputMethods;

namespace Mobile
{
    [Activity(Label = "Notas de texto", Icon = "@android:color/transparent")]
    public class Notas_Texto : Activity
    {

        private Button mSaveNota;
        private EditText mDescricao;
        private EditText mNota;
        private int visita;
        private LinearLayout mLinearLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            SetContentView(Resource.Layout.Notas_Texto);

            mSaveNota = FindViewById<Button>(Resource.Id.btnNota);
            mDescricao = FindViewById<EditText>(Resource.Id.txtDescricao);
            mLinearLayout = FindViewById<LinearLayout>(Resource.Id.mainView);
            mNota = FindViewById<EditText>(Resource.Id.txtNota);
            visita = Convert.ToInt32(Intent.GetStringExtra("visita"));

            mSaveNota.Click += MGuardar_Nota_Click;
            mLinearLayout.Click += MLinearLayout_Click;
        }

        private void MLinearLayout_Click(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
        }

        private void MGuardar_Nota_Click(object sender, EventArgs e)
        {
            if(mNota.Text != "")
            {
                WebClient client = new WebClient();
                Uri uri = new Uri("http://169.254.80.80:8080/InsertNota.php");
                NameValueCollection parameters = new NameValueCollection();

                parameters.Add("descricao", mDescricao.Text);
                parameters.Add("visita", visita.ToString());
                parameters.Add("nota", mNota.Text);




                client.UploadValuesCompleted += Client_UploadValuesCompleted;
                client.UploadValuesAsync(uri, parameters);


                mDescricao.Text = "";
                mNota.Text = "";
            }else
            {
                Toast.MakeText(this, "Insira uma Nota", ToastLength.Long).Show();
            }
            
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, "Nota inserida com sucesso", ToastLength.Long).Show();
            });
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
    }
}