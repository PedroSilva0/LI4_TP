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
    class FormAdapter : BaseAdapter<TextView>
    {
        private Context mContext;
        private int mLayout;
        private List<TextView> mPerguntas;

        public FormAdapter(Context context, int layout, List<TextView> perguntas)
        {
            mContext = context;
            mLayout = layout;
            mPerguntas = perguntas;
        }

        public override TextView this[int position]
        {
            get { return mPerguntas[position]; }
        }

        public override int Count
        {
            get { return mPerguntas.Count; }
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

            row.FindViewById<TextView>(Resource.Id.txtPergunta1).Text = mPerguntas[position].Text;

            return row;
        }
    }
}