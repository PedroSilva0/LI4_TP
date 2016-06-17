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

namespace Mobile
{
    [Activity(Label = "Opções", Icon = "@android:color/transparent")]
    public class PVFN : Activity
    {
        ImageButton mPhoto;
        ImageButton mNote;
        ImageButton mForm;
        ImageButton mVoice;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PVFN);
            mPhoto = FindViewById<ImageButton>(Resource.Id.imgBtnPhoto);
            mPhoto.Click += MPhoto_Click;
        }

        private void MPhoto_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Photo));
            this.StartActivity(intent);
        }
    }
}