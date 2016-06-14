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

namespace Mobile
{
    [Activity(Label = "Planos")]
    public class Planos : Activity
    {
        private ListView mListView;
        private WebClient mClient;
        private List<Plano> mPlanos;
        private BaseAdapter<Plano> mAdapter;
        private ProgressBar mProgressBar;
        private Uri mUri;
        private string mFiscal;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Planos);

            mListView = FindViewById<ListView>(Resource.Id.listView);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progBar);

            mFiscal = Intent.GetStringExtra("Fiscal");
            mClient = new WebClient();
            mUri = new Uri("http://192.168.1.69:8080/GetPlanos.php");

            //NameValueCollection parameters = new NameValueCollection();
            //parameters.Add("Fiscal", mFiscal);
            mClient.DownloadDataAsync(mUri);
            //mClient.UploadValuesAsync(mUri, parameters);
            mClient.DownloadDataCompleted += MClient_DownloadDataCompleted;
        }

        private void MClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                mPlanos = JsonConvert.DeserializeObject<List<Plano>>(json);
                mAdapter = new PlanosAdapter(this, Resource.Layout.plano, mPlanos);
                mListView.Adapter = mAdapter;
                mProgressBar.Visibility = ViewStates.Gone;
            });
        }
    }
}