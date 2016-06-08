using System;
using System.Collections.Generic;
using System.Text;

namespace LOGeDAO
{
    class GPS
    {
        private float longitude { get; set; }
        private float latitude { get; set; }

        public GPS(float mLong, float mLat)
        {
            longitude = mLong;
            latitude = mLat;

        }
    }
}
