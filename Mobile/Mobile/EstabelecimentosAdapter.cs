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
    class EstabelecimentosAdapter : BaseAdapter<Estabelecimento>
    {
        private Context mContext;
        private int mLayout;
        private List<Estabelecimento> mEstabelecimentos;

        public EstabelecimentosAdapter(Context context, int layout, List<Estabelecimento> estabelecimentos)
        {
            mContext = context;
            mLayout = layout;
            mEstabelecimentos = estabelecimentos;
        }

        public override Estabelecimento this[int position]
        {
            get
            {
                return mEstabelecimentos[position];
            }
        }

        public override int Count
        {
            get
            {
               return  mEstabelecimentos.Count;
            }
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

            row.FindViewById<TextView>(Resource.Id.txtEstabelecimento).Text = mEstabelecimentos[position].nome;

            return row;
        }
    }
}