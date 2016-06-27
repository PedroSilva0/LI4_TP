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
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;

namespace Mobile
{
    [Activity(Label = "Formulario", Icon = "@android:color/transparent")]
    class Questionario:Activity
    {
        
        private Button _proxima;
        private Button _anterior;
        private Button _guardar;
        private string id_vis;
        private int id_pergunta;
        private EditText _resposta;
        private TextView _pergunta;
        List<string> questoes;
        List<string> respostas;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Preparar o layout
            base.OnCreate(savedInstanceState);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            SetContentView(Resource.Layout.Questionario);

            _proxima = FindViewById<Button>(Resource.Id.btnProxima);
            _anterior = FindViewById<Button>(Resource.Id.btnAnterior);
            _guardar = FindViewById<Button>(Resource.Id.btnGuardar);
            _resposta = FindViewById<EditText>(Resource.Id.txtResposta);
            _pergunta = FindViewById<TextView>(Resource.Id.pergunta);
            id_vis = Intent.GetStringExtra("visita");
            id_pergunta = 0;

            _proxima.Click += pergunta_seguinte_click;
            _anterior.Click += pergunta_anterior_click;
            _guardar.Click += guardar_respostas_click;

            _anterior.Enabled = false;

            //Buscar as perguntas
            WebClient mClient = new WebClient();
            Uri mUri = new Uri("http://192.168.1.69:8080/GetQuestoes2.php");

            mClient.DownloadDataAsync(mUri);
            mClient.DownloadDataCompleted += MClient_DownloadDataCompleted;
        }

        private void pergunta_seguinte_click(object sender, EventArgs e)
        {
            //guardar resposta em memória
            respostas[id_pergunta] = _resposta.Text;
            //alterar pergunta atual
            id_pergunta++;
            //alterar layout
            _pergunta.Text = questoes[id_pergunta];
            _resposta.Text = respostas[id_pergunta];
            //corrigir butão
            if (id_pergunta == questoes.Count-1)
            {
                _proxima.Enabled = false;
                _anterior.Enabled = true;
            }
            else
            {
                _proxima.Enabled = true;
                _anterior.Enabled = true;
            }
        }

        private void pergunta_anterior_click(object sender, EventArgs e) {
            //guardar resposta em memória
            respostas[id_pergunta] = _resposta.Text;
            //alterar pergunta atual
            id_pergunta--;
            //alterar layout
            _pergunta.Text = questoes[id_pergunta];
            _resposta.Text = respostas[id_pergunta];
            //corrigir butão
            if (id_pergunta == 0)
            {
                _proxima.Enabled = true;
                _anterior.Enabled = false;
            }else
            {
                _proxima.Enabled = true;
                _anterior.Enabled = true;
            }
        }

        private void guardar_respostas_click(object sender, EventArgs e) {
            respostas[id_pergunta] = _resposta.Text;
            for(int i=0;i< questoes.Count; i++) {

                WebClient client = new WebClient();
                Uri uri = new Uri("http://192.168.1.69:8080/InsertResposta.php");

                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("visita", id_vis);
                parameters.Add("questao", Convert.ToString(i+1));
                parameters.Add("resposta", respostas[i]);
                client.UploadValuesCompleted += Client_UploadValuesCompleted;
                client.UploadValuesAsync(uri, parameters);
            }
        }

        private void MClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                questoes = JsonConvert.DeserializeObject<List<string>>(json);
                respostas = new List<string>();
                if (questoes.Count > 0)
                {
                    _pergunta.Text = questoes[id_pergunta];
                    //Inicializar respostas
                    for (int i = 0; i < questoes.Count; i++)
                    {
                        respostas.Add("");
                    }
                }

            });
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, "Respostas inseridas com sucesso", ToastLength.Long).Show();
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