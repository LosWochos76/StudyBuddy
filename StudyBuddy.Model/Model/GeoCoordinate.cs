using System;
using System.Globalization;

namespace StudyBuddy.Model
{
    public class GeoCoordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }

        public static GeoCoordinate FromString(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new Exception("No valid geo-coordinate!");

            content = content.Replace(',', '.');
            var parts = content.Split(";");
            if (parts.Length < 2 || parts.Length > 3)
                throw new Exception("No valid geo-coordinate!");

            var result = new GeoCoordinate();
            result.Latitude = Convert(parts[0]);
            result.Longitude = Convert(parts[1]);

            if (parts.Length == 3)
                result.Radius = Convert(parts[2]);

            return result;
        }

        private static double Convert(string input)
        {
            double d;
            if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out d))
                return d;
            else
                throw new Exception("No valid geo-coordinate!");
        }

        public bool IsInRadius(GeoCoordinate other)
        {
            var distance = Distance(other);
            return distance <= Radius;
        }

        public double Distance(GeoCoordinate other)
        {
            var d1 = Latitude * (Math.PI / 180.0);
            var num1 = Longitude * (Math.PI / 180.0);
            var d2 = other.Latitude * (Math.PI / 180.0);
            var num2 = other.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                        Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}