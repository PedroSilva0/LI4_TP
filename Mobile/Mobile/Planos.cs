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

namespace Mobile
{
    [Activity(Label = "Planos")]
    public class Planos : Activity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private RecyclerView.Adapter mAdapter;
        //private ListView mListView;
        private WebClient mClient;
        private List<Plano> mPlanos;
        //private BaseAdapter<Plano> mAdapter;
        private ProgressBar mProgressBar;
        private Uri mUri;
        private string mFiscal;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Planos);

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            //mListView = FindViewById<ListView>(Resource.Id.listView);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progBar);

            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mFiscal = Intent.GetStringExtra("Fiscal");
            mClient = new WebClient();
            mUri = new Uri("http://192.168.1.69:8080/GetPlanos.php");

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("Fiscal", mFiscal);
            mClient.DownloadDataAsync(mUri);
            mClient.DownloadDataCompleted += MClient_DownloadDataCompleted;
            //mClient.UploadValuesCompleted += MClient_UploadValuesCompleted;
            //mClient.UploadValuesAsync(mUri, parameters);
        }

        private void MClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                mPlanos = JsonConvert.DeserializeObject<List<Plano>>(json);
                //mAdapter = new PlanosAdapter(this, Resource.Layout.planoAUX, mPlanos);
                mAdapter = new RecyclerAdapter(mPlanos);
                //mListView.Adapter = mAdapter;
                mRecyclerView.SetAdapter(mAdapter);
                mProgressBar.Visibility = ViewStates.Gone;
            });
        }
        /*
        private void MClient_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
           RunOnUiThread(() =>
           {
               string json = Encoding.UTF8.GetString(e.Result);
               mPlanos = JsonConvert.DeserializeObject<List<Plano>>(json);
               //mAdapter = new PlanosAdapter(this, Resource.Layout.plano, mPlanos);
               mAdapter = new RecyclerAdapter(mPlanos);
               //mListView.Adapter = mAdapter;
               mRecyclerView.SetAdapter(mAdapter);
               mProgressBar.Visibility = ViewStates.Gone;
           });
        }
        */

    }
    
    public class RecyclerAdapter : RecyclerView.Adapter
    {
        private List<Plano> mPlanos;

        public RecyclerAdapter (List<Plano> planos)
        {
            mPlanos = planos;
        }

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mIdPlano { get; set; }
            public ImageButton mInfo { get; set; }
            public ImageButton mAccept { get; set; }

            public MyView (View view) : base(view)
            {
                mMainView = view;
            }
        }

        public override int ItemCount
        {
            get { return mPlanos.Count; }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mIdPlano.Text = position.ToString();
            myHolder.mInfo.Click += MInfo_Click;
            myHolder.mAccept.Click += MAccept_Click;
        }

        private void MAccept_Click(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "Botão Info por implementar", ToastLength.Long).Show();
        }

        private void MInfo_Click(object sender, EventArgs e)
        {
            //Pull up dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            dialog_Info infoDialog = new dialog_Info();
            infoDialog.Show(transaction, "dialog fragment");
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.plano, parent, false);

            TextView txtIdPlano = row.FindViewById<TextView>(Resource.Id.txtIdPlano);
            ImageButton imgInfo = row.FindViewById <ImageButton>(Resource.Id.imageButton1);
            ImageButton imgAccept = row.FindViewById<ImageButton>(Resource.Id.imageButton3);

            MyView view = new MyView(row) { mIdPlano = txtIdPlano, mInfo = imgInfo, mAccept = imgAccept};
            return view;
        }
    }
    
}