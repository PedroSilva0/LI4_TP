    using System;
    using System.Collections.Generic;
    using Android.App;
    using Android.Content;
    using Android.Content.PM;
    using Android.Graphics;
    using Android.OS;
    using Android.Provider;
    using Android.Widget;
    using Java.IO;
    using Environment = Android.OS.Environment;
    using System.Globalization;
    using System.Net;
    using System.Collections.Specialized;
    using System.Text;
    using Android.Runtime;
    using System.IO;

namespace Mobile
{
    [Activity(Label = "Câmera", Icon = "@android:color/transparent")]
    public class Photo : Activity
    {
        private ImageView mPic;
        private string mIdVis;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Photo);

            mPic = FindViewById<ImageView>(Resource.Id.imageView1);
            mPic.Click -= MPic_Click;
            mPic.Click += MPic_Click;
        }

        private void MPic_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);
            this.StartActivityForResult(Intent.CreateChooser(intent, "Select a picture"), 0);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                mIdVis = Intent.GetStringExtra("IdVis");
                Stream stream = ContentResolver.OpenInputStream(data.Data);
                mPic.SetImageBitmap(DecodeBitmapFromStream(data.Data, 150, 150));

                Bitmap bitmap = BitmapFactory.DecodeStream(stream);
                MemoryStream memStream = new MemoryStream();
                bitmap.Compress(Bitmap.CompressFormat.Webp, 100, memStream);
                byte[] picData = memStream.ToArray();

                WebClient client = new WebClient();
                Uri uri = new Uri("http://192.168.1.69:8080/InsertPhoto.php");
                
                NameValueCollection parameters = new NameValueCollection();
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
                System.Console.WriteLine(Encoding.UTF8.GetString(e.Result));
            });
        }

        private Bitmap DecodeBitmapFromStream(Android.Net.Uri data, int requestedWidth, int requestedHeight)
        {
            Stream stream = ContentResolver.OpenInputStream(data);
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeStream(stream);

            options.InSampleSize = CalculateInSampleSize(options, requestedWidth, requestedHeight);
            
            stream = ContentResolver.OpenInputStream(data);
            options.InJustDecodeBounds = false;
            Bitmap bitmap = BitmapFactory.DecodeStream(stream, null, options);
            return bitmap;
        }

        private int CalculateInSampleSize(BitmapFactory.Options options, int requestedWidth, int requestedHeight)
        {
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = -1;

            if(height > requestedHeight || width > requestedWidth)
            {
                int halfHeight = height / 2;
                int halfWidth = width / 2;

                while((halfHeight / inSampleSize) > requestedHeight && (halfWidth / inSampleSize) > requestedWidth)
                {
                    inSampleSize *= 2;
                }
                   
            }

            return inSampleSize;
        }
    }

        /*private WebClient mClient;
        private Uri mUri;
        String namefoto;
        private string mIdVis;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Make it available in the gallery
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);
            
            mClient = new WebClient();
            mUri = new Uri("http://192.168.1.69:8080/InsertPhoto.php");
            
            NameValueCollection parameters = new NameValueCollection();
            mIdVis = Intent.GetStringExtra("IdVis");
            parameters.Add("Foto", namefoto);
            parameters.Add("IdVis", mIdVis);

            mClient.UploadValuesCompleted += MClient_UploadValuesCompleted;
            mClient.UploadValuesAsync(mUri, parameters);
            // Dispose of the Java side bitmap.
            GC.Collect();
        }

        private void MClient_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string result = Encoding.UTF8.GetString(e.Result);
                if (result.Equals("ok"))
                {
                    Toast.MakeText(this, "OK", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "KO", ToastLength.Long).Show();
                }
            });
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Photo);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();
                Button button = FindViewById<Button>(Resource.Id.myButton);

                button.Click += TakeAPicture;
                //id++;
            }

        }

        private void CreateDirectoryForPictures()
        {
            App._dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "CameraAppDemo");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {

            // String namefotoaux = String.Concat("myPhoto_",id);
            // String namefoto = String.Concat(namefotoaux,".jpg");
            DateTime localDate = DateTime.Now;
            string cultureName = "de-DE";
            var culture = new CultureInfo(cultureName);
            //String namefoto = String.Format("myPhoto_{0}.jpg", Guid.NewGuid());
            namefoto = String.Format("{0}.jpg",localDate.ToString(culture));

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new File(App._dir, namefoto);
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);
        }
    }

    public static class App
    {

        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }*/
}