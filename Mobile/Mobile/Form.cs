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
    [Activity(Label = "Formulario", Icon = "@android:color/transparent")]
    public class Form : Activity
    {
        private WebClient mClient;
        private Uri mUri;
        private ListView mListView;
        private List<Form_row> mForm;
        private BaseAdapter<Form_row> mAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Form);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            mListView = FindViewById<ListView>(Resource.Id.listViewF);

            mClient = new WebClient();
            mUri = new Uri("http://169.254.80.80:8080/GetQuestoes.php");
            
            mClient.DownloadDataAsync(mUri);
            mClient.DownloadDataCompleted += MClient_DownloadDataCompleted;
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

        private void MClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                mForm = JsonConvert.DeserializeObject<List<Form_row>>(json);
                mAdapter = new FormAdapter(this, Resource.Layout.form_row, mForm);
                mListView.Adapter = mAdapter;
            });
        }
    }
}