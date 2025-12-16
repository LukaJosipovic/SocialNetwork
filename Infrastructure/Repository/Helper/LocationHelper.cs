using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Helper
{
    public static class LocationHelper
    {
        public static double GetDistanceInKm(double lat1, double lon1, double lat2, double lon2)
        {
            // Earth's radius in kilometers
            const double R = 6371;

            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) *
                Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // distance in KM
            return R * c;
        }

        private static double DegreesToRadians(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
