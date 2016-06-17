using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using Android;
using Java.Lang;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Collections.Specialized;
using System.Text;

namespace Mobile
{
    [Activity(Label = "Dank Restaurant", MainLauncher = true, Icon = "@drawable/Dank")]
    public class MainActivity : Activity
    {
        LinearLayout mLinearLayout;
        private Button mLogIn;
        private ProgressBar mprogBar;
        private EditText mId;
        private EditText mPassword;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            mLinearLayout = FindViewById<LinearLayout>(Resource.Id.mainView);
            mLogIn = FindViewById<Button>(Resource.Id.btnLogin);
            mprogBar = FindViewById<ProgressBar>(Resource.Id.progBar);
            mId = FindViewById<EditText>(Resource.Id.txtUserName);
            mPassword = FindViewById<EditText>(Resource.Id.txtPassword);

            mLogIn.Click += MLogIn_Click;
            mLinearLayout.Click += mLinearLayout_Click;
        }

        private void MLogIn_Click(object sender, EventArgs e)
        {
            mprogBar.Visibility = ViewStates.Visible;
            WebClient client = new WebClient();
            Uri uri = new Uri("http://172.26.10.5:8080/LogIn.php");
            NameValueCollection parameters = new NameValueCollection();

            parameters.Add("id_fisc", mId.Text);
            parameters.Add("pass", mPassword.Text);

            client.UploadValuesCompleted += Client_UploadValuesCompleted;
            client.UploadValuesAsync(uri, parameters);
        }
        
        private void Client_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string result = Encoding.UTF8.GetString(e.Result);
                if (result.Equals("ok"))
                {
                    mprogBar.Visibility = ViewStates.Invisible;
                    Intent intent = new Intent(this, typeof(Planos));
                    intent.PutExtra("Fiscal", mId.Text);
                    this.StartActivity(intent);
                }else
                {
                    Toast.MakeText(this, "Login inválido", ToastLength.Long).Show();
                    mprogBar.Visibility = ViewStates.Invisible;
                }
            });
        }

        void mLinearLayout_Click(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
        }
    }
}