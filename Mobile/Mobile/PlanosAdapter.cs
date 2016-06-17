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
    class PlanosAdapter : BaseAdapter<Plano>
    {
        private Context mContext;
        private int mLayout;
        private List<Plano> mPlanos;

        public PlanosAdapter(Context context, int layout, List<Plano> planos)
        {
            mContext = context;
            mLayout = layout;
            mPlanos = planos;
        }

        public override Plano this[int position]
        {
            get { return mPlanos[position]; }
        }

        public override int Count
        {
            get { return mPlanos.Count; }
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

            //row.FindViewById<TextView>(Resource.Id.txtIdPlano).Text = mPlanos[position].id.ToString();
            row.FindViewById<TextView>(Resource.Id.txtIdPlano).Text = position.ToString();

            return row;
        }
    }
}