using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Media;
using System.Globalization;
using System.Net;
using System.Collections.Specialized;
using Android.Content;
using Android.Views.InputMethods;
using Android.Provider;

namespace Mobile
{
    [Activity(Label = "Gravar áudio")]

    public class GravaVoz : Activity
    {

        private string mIdVis;
        private EditText mDescricao;

        MediaRecorder _recorder;
        MediaPlayer _player;
        Button _start;
        Button _stop;
        Button _gravar;
        byte[] picData;
        String nameVoiceFile;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.GravaVoz);

            _start = FindViewById<Button>(Resource.Id.start);
            _stop = FindViewById<Button>(Resource.Id.stop);
            _gravar = FindViewById<Button>(Resource.Id.gravar);

            DateTime localDate = DateTime.Now;
            string cultureName = "de-DE";
            var culture = new CultureInfo(cultureName);
            nameVoiceFile = String.Format("/sdcard/{0}.wav", localDate.ToString(culture));


            _start.Click += delegate {
                _stop.Enabled = !_stop.Enabled;
                _start.Enabled = !_start.Enabled;

                _recorder.SetAudioSource(AudioSource.Mic);
                _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
                _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
                _recorder.SetOutputFile(nameVoiceFile);
                _recorder.Prepare();
                _recorder.Start();
            };

            _stop.Click += delegate {
                _stop.Enabled = !_stop.Enabled;

                _recorder.Stop();
                _recorder.Reset();

                _player.SetDataSource(nameVoiceFile);
                _player.Prepare();
                _player.Start();
            };

            _gravar.Click += MGallery_Click;
        }

        private void MPic_Click(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
        }

        private void MLinearLayout_Click(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
        }


        private void MGallery_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            var sdCard = Android.OS.Environment.ExternalStorageDirectory.Path;
            intent.SetType("/sdcard/");
            Toast.MakeText(this, nameVoiceFile, ToastLength.Long).Show();
            intent.SetAction(Intent.ActionGetContent);
            this.StartActivityForResult(Intent.CreateChooser(intent, "Select a file"), 1);
        }


        private void MBtnTakePicture_Click(object sender, EventArgs e)
        {
            DateTime localDate = DateTime.Now;
            string cultureName = "de-DE";
            var culture = new CultureInfo(cultureName);
            String nameVoice = String.Format("{0}.wav", localDate.ToString(culture));

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new Java.IO.File(App._dir, nameVoice);
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
            this.StartActivityForResult(intent, 0);
        }


        private void GravaClick(object sender, EventArgs e)
        {
            if (picData != null)
            {
                WebClient client = new WebClient();
                Uri uri = new Uri("http://192.168.1.69:8080/InsertVoice.php");

                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("Descricao", mDescricao.Text);
                parameters.Add("Foto", Convert.ToBase64String(picData));
                parameters.Add("IdVis", mIdVis);

                client.UploadValuesAsync(uri, parameters);
                client.UploadValuesCompleted += Client_UploadValuesCompleted;
            }
        }

        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, "Ficheiro de voz inserido com sucesso", ToastLength.Long).Show();
            });
        }





        protected override void OnResume()
        {
            base.OnResume();

            _recorder = new MediaRecorder();
            _player = new MediaPlayer();

            _player.Completion += (sender, e) => {
                _player.Reset();
                _start.Enabled = !_start.Enabled;
            };
        }

        protected override void OnPause()
        {
            base.OnPause();

            _player.Release();
            _recorder.Release();

            _player.Dispose();
            _recorder.Dispose();
            _player = null;
            _recorder = null;
        }
    }
}