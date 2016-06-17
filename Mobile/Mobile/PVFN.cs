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
        Button mFinVis;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PVFN);
            mPhoto = FindViewById<ImageButton>(Resource.Id.imgBtnPhoto);
            mNote = FindViewById<ImageButton>(Resource.Id.imgBtnText);
            mForm = FindViewById<ImageButton>(Resource.Id.imgBtnForm);
            mVoice = FindViewById<ImageButton>(Resource.Id.imgBtnVoice);
            mFinVis = FindViewById<Button>(Resource.Id.btnFinVis);

            mPhoto.Click += MPhoto_Click;
            mNote.Click += MNote_Click;
            mForm.Click += MForm_Click;
            mVoice.Click += MVoice_Click;
            mFinVis.Click += MFinVis_Click;
        }

        private void MFinVis_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MVoice_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MForm_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Form));
            this.StartActivity(intent);
        }

        private void MNote_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MPhoto_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(Photo));
            this.StartActivity(intent);
        }
    }
}