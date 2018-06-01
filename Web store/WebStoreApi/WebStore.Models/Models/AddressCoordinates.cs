using System;

namespace WebStore.Models.Models
{
    public class AddressCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double Distance(AddressCoordinates other)
        {
            other = other ?? throw new ArgumentNullException(nameof(other));
            Func<double, double> toRadians = (double angle) => angle * Math.PI / 180.0;

            double dlon = toRadians(other.Longitude - Longitude);
            double dlat = toRadians(other.Latitude - Latitude);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(toRadians(Latitude)) * Math.Cos(toRadians(other.Latitude)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            return 12742.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }
    }
}
