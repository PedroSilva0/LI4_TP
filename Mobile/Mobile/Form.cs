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
        private Button mEnviaForm;
        private TextView mPergunta1;
        private TextView mPergunta2;
        private TextView mPergunta3;
        private TextView mPergunta4;
        private TextView mPergunta5;
        private TextView mPergunta6;
        private TextView mPergunta7;
        private TextView mPergunta8;
        private TextView mPergunta9;
        private TextView mPergunta10;
        private TextView mPergunta11;
        private EditText mResposta1;
        private EditText mResposta2;
        private EditText mResposta3;
        private EditText mResposta4;
        private EditText mResposta5;
        private EditText mResposta6;
        private EditText mResposta7;
        private EditText mResposta8;
        private EditText mResposta9;
        private EditText mResposta10;
        private EditText mResposta11;

        private List<TextView> mPerguntas;
        private List<EditText> mRespostas;
        private string mIdVis;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Form);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            mPergunta1 = FindViewById<TextView>(Resource.Id.txtPergunta1);
            mPergunta2 = FindViewById<TextView>(Resource.Id.txtPergunta2);
            mPergunta3 = FindViewById<TextView>(Resource.Id.txtPergunta3);
            mPergunta4 = FindViewById<TextView>(Resource.Id.txtPergunta4);
            mPergunta5 = FindViewById<TextView>(Resource.Id.txtPergunta5);
            mPergunta6 = FindViewById<TextView>(Resource.Id.txtPergunta6);
            mPergunta7 = FindViewById<TextView>(Resource.Id.txtPergunta7);
            mPergunta8 = FindViewById<TextView>(Resource.Id.txtPergunta8);
            mPergunta9 = FindViewById<TextView>(Resource.Id.txtPergunta9);
            mPergunta10 = FindViewById<TextView>(Resource.Id.txtPergunta10);
            mPergunta11 = FindViewById<TextView>(Resource.Id.txtPergunta11);

            mResposta1 = FindViewById<EditText>(Resource.Id.txtResposta1);
            mResposta2 = FindViewById<EditText>(Resource.Id.txtResposta2);
            mResposta3 = FindViewById<EditText>(Resource.Id.txtResposta3);
            mResposta4 = FindViewById<EditText>(Resource.Id.txtResposta4);
            mResposta5 = FindViewById<EditText>(Resource.Id.txtResposta5);
            mResposta6 = FindViewById<EditText>(Resource.Id.txtResposta6);
            mResposta7 = FindViewById<EditText>(Resource.Id.txtResposta7);
            mResposta8 = FindViewById<EditText>(Resource.Id.txtResposta8);
            mResposta9 = FindViewById<EditText>(Resource.Id.txtResposta9);
            mResposta10 = FindViewById<EditText>(Resource.Id.txtResposta10);
            mResposta11 = FindViewById<EditText>(Resource.Id.txtResposta11);

            mPerguntas = new List<TextView>();
            mRespostas = new List<EditText>();

            int i;
            for (i = 0; i < 11; i++)
            {
                mPerguntas.Add(mPergunta1);
                mPerguntas.Add(mPergunta2);
                mPerguntas.Add(mPergunta3);
                mPerguntas.Add(mPergunta4);
                mPerguntas.Add(mPergunta5);
                mPerguntas.Add(mPergunta6);
                mPerguntas.Add(mPergunta7);
                mPerguntas.Add(mPergunta8);
                mPerguntas.Add(mPergunta9);
                mPerguntas.Add(mPergunta10);
                mPerguntas.Add(mPergunta11);
            }

            for (i = 0; i < 11; i++)
            {
                mRespostas.Add(mResposta1);
                mRespostas.Add(mResposta2);
                mRespostas.Add(mResposta3);
                mRespostas.Add(mResposta4);
                mRespostas.Add(mResposta5);
                mRespostas.Add(mResposta6);
                mRespostas.Add(mResposta7);
                mRespostas.Add(mResposta8);
                mRespostas.Add(mResposta9);
                mRespostas.Add(mResposta10);
                mRespostas.Add(mResposta11);
            }

            mEnviaForm = FindViewById<Button>(Resource.Id.btnForm);
            mIdVis = Intent.GetStringExtra("visita");

            mClient = new WebClient();
            mUri = new Uri("http://169.254.80.80:8080/GetQuestoes.php");

            mEnviaForm.Click += MEnviaForm_Click;
            mClient.DownloadDataAsync(mUri);
            mClient.DownloadDataCompleted += MClient_DownloadDataCompleted;

            mPergunta1.Text = mPerguntas[0].Text;
        }
        
        private void MEnviaForm_Click(object sender, EventArgs e)
        {
            int i;
            bool flag = true;
            for (i = 0; i < mRespostas.Count; i++)
            {
               if (mRespostas[i].Text == "")
                {
                    flag = false;
                    break;
                }
            }
            if (flag == false)
            {
                Toast.MakeText(this, "Preencha todos os campos do Formulario", ToastLength.Long).Show();
            }
            else
            {
                for (i = 1; i <= mPerguntas.Count; i++)
                {
                    WebClient client = new WebClient();
                    Uri uri = new Uri("http://169.254.80.80:8080/InsertResposta.php");

                    NameValueCollection parameters = new NameValueCollection();
                    parameters.Add("IdQuestao", i.ToString());
                    parameters.Add("IdVis", mIdVis);
                    parameters.Add("Resposta", mRespostas[i].Text);

                    client.UploadValuesAsync(uri, parameters);
                    client.UploadValuesCompleted += Client_UploadValuesCompleted;
                }
                Toast.MakeText(this, "Formulario inserido com sucesso", ToastLength.Long).Show();
            }
        }
        
        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
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
                mPerguntas = JsonConvert.DeserializeObject<List<TextView>>(json);

            });
        }
        
    }
}