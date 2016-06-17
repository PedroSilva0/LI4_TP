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
using Android.Support.V4.Widget;
using System.ComponentModel;

namespace Mobile
{
    [Activity(Label = "Estabelecimentos", Icon = "@android:color/transparent")]
    public class PlanoI : Activity
    {
        private ListView mListView;
        private WebClient mClient;
        private List<Estabelecimento> mEstabelecimentos;
        private BaseAdapter<Estabelecimento> mAdapter;
        private ProgressBar mProgressBar;
        private Uri mUri;
        private string mId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Info);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progBarI);
            mListView = FindViewById<ListView>(Resource.Id.listViewI);
            mClient = new WebClient();
            mUri = new Uri("http://172.26.10.5:8080/GetEstabelecimentos.php");

            mId = Intent.GetStringExtra("Id");
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("Plano", mId);
            mClient.UploadValuesCompleted += MClient_UploadValuesCompleted;
            mClient.UploadValuesAsync(mUri, parameters);
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

        private void MClient_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                mEstabelecimentos = JsonConvert.DeserializeObject<List<Estabelecimento>>(json);
                mAdapter = new EstabelecimentosAdapter(this, Resource.Layout.estabelecimento, mEstabelecimentos);
                mListView.Adapter = mAdapter;
                mProgressBar.Visibility = ViewStates.Gone;
            });
        }
    }
}