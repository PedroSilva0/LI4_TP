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
using Android.Support.V4.Widget;
using System.Net;
using System.Collections.Specialized;
using System.ComponentModel;
using Newtonsoft.Json;
using Java.Lang;

namespace Mobile
{
    [Activity(Label = "Visitas", Icon = "@android:color/transparent")]
    public class Visitas : Activity
    {
        private ListView mListView;
        private WebClient mClient;
        private List<Estabelecimento> mEstabelecimentos;
        private List<Visita> mVisitas;
        private BaseAdapter<Estabelecimento> mAdapter;
        private ProgressBar mProgressBar;
        private Uri mUri;
        private string mId;
        private string mFiscal;
        private string mEstabelecimento;
        private TextView estabelecimentoTV;
        //bool doubleBackToExitPressedOnce = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Visitas);

            mListView = FindViewById<ListView>(Resource.Id.listViewV);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progBarV);
            mFiscal = Intent.GetStringExtra("Fiscal");

            mClient = new WebClient();
            mUri = new Uri("http://192.168.1.69:8080/UpdateGetEstabelecimentos.php");

            mId = Intent.GetStringExtra("Id");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("Plano", mId);
            parameters.Add("Fiscal", mFiscal);
            mClient.UploadValuesCompleted += MClient_UploadValuesCompleted;
            mClient.UploadValuesAsync(mUri, parameters);
            mListView.ItemClick += MListView_ItemClick;
        }

        private void MListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(VisitasGPSA));
            intent.PutExtra("Nome",mEstabelecimentos[e.Position].nome);
            intent.PutExtra("IdVis", mEstabelecimentos[e.Position].id_vis.ToString());
            intent.PutExtra("IdPlano", mId);
            intent.PutExtra("IdFiscal", mFiscal);
            this.StartActivity(intent);
        }

        /*
public override void OnBackPressed()
{
   if (doubleBackToExitPressedOnce)
   {
       base.OnBackPressed();
       return;
   }
   this.doubleBackToExitPressedOnce = true;
   Toast.MakeText(this, "Please click BACK again to exit", ToastLength.Short).Show();
   RunOnUiThread(() =>
   {
       Thread.Sleep(2000);

   });
   doubleBackToExitPressedOnce = false;
}*/

        private void MClient_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                mEstabelecimentos = JsonConvert.DeserializeObject<List<Estabelecimento>>(json);
                mAdapter = new EstabelecimentosAdapter(this, Resource.Layout.visita, mEstabelecimentos);
                mListView.Adapter = mAdapter;
                mProgressBar.Visibility = ViewStates.Gone;
                if (mListView.Count == 0)
                {
                    Intent intent = new Intent(this, typeof(Planos));
                    intent.PutExtra("Fiscal", mFiscal);
                    this.StartActivity(intent);
                    Toast.MakeText(this, "Plano concluido", ToastLength.Long).Show();
                }
            });
        }
    }
}