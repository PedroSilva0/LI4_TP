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

namespace Mobile
{
    [Activity(Label = "Opções", Icon = "@android:color/transparent")]
    public class PVFN : Activity
    {
        private ImageButton mPhoto;
        private ImageButton mNote;
        private ImageButton mForm;
        private ImageButton mVoice;
        private Button mFinVis;
        private string mIdVis;
        private string mPlano;
        private string mFiscal;
        private bool flag;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PVFN);
            mPhoto = FindViewById<ImageButton>(Resource.Id.imgBtnPhoto);
            mNote = FindViewById<ImageButton>(Resource.Id.imgBtnText);
            mForm = FindViewById<ImageButton>(Resource.Id.imgBtnForm);
            mVoice = FindViewById<ImageButton>(Resource.Id.imgBtnVoice);
            mFinVis = FindViewById<Button>(Resource.Id.btnFinVis);
            mIdVis = Intent.GetStringExtra("IdVis");
            mPlano = Intent.GetStringExtra("IdPlano");
            mFiscal = Intent.GetStringExtra("IdFiscal");
            flag = false;

            mPhoto.Click += MPhoto_Click;
            mNote.Click += MNote_Click;
            mForm.Click += MForm_Click;
            mVoice.Click += MVoice_Click;
            mFinVis.Click += MFinVis_Click;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(resultCode == Result.Ok && requestCode == 0)
            {
                flag = true;
            }
        }

        private void MFinVis_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("http://192.168.1.69:8080/TerminarVisita.php");
            NameValueCollection parameters = new NameValueCollection();
            //Toast.MakeText(this, mIdVis, ToastLength.Long).Show();
            parameters.Add("id_vis", mIdVis);

            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(uri, parameters);

            Intent intent = new Intent(this, typeof(Visitas));
            intent.PutExtra("Id", mPlano);
            intent.PutExtra("Fiscal", mFiscal);
            this.StartActivity(intent);
            Finish();
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                System.Console.WriteLine(Encoding.UTF8.GetString(e.Result));
            });
        }

        private void MVoice_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(GravaVoz));
            intent.PutExtra("visita", mIdVis);
            this.StartActivity(intent);
        }

        private void MForm_Click(object sender, EventArgs e)
        {
            if(flag == false)
            {
                Intent intent = new Intent(this, typeof(Questionario));
                intent.PutExtra("visita", mIdVis);
                this.StartActivityForResult(intent, 0);
            }else
            {
                Toast.MakeText(this, "Só pode preencher o formulário uma vez!", ToastLength.Long).Show();
            }
            
        }

        private void MNote_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Notas_Texto));
            intent.PutExtra("visita", mIdVis);
            this.StartActivity(intent);
        }

        private void MPhoto_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Photo));
            intent.PutExtra("IdVis", mIdVis);
            this.StartActivity(intent);
        }
    }
}