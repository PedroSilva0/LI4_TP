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
    class FormAdapter : BaseAdapter<Form_row>
    {
        private Context mContext;
        private int mLayout;
        private List<Form_row> mForm;

        public FormAdapter(Context context, int layout, List<Form_row> form)
        {
            mContext = context;
            mLayout = layout;
            mForm = form;
        }

        public override Form_row this[int position]
        {
            get { return mForm[position]; }
        }

        public override int Count
        {
            get { return mForm.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(mLayout, parent, false);
            }

            row.FindViewById<TextView>(Resource.Id.txtPergunta).Text = mForm[position].pergunta;

            return row;
        }
    }
}