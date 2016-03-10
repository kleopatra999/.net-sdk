using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{
    public class CloudGeoPoint
    {
        internal Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
        protected double[] coordinates = new double[2];
        public CloudGeoPoint(double longitude, double latitude)
        {
            
            dictionary.Add("_type", "point");
            dictionary.Add("_isModified", true);
            if((latitude >= -90.0 && latitude <= 90.0) && (longitude >= -180.0 && longitude <= 180.0))
            {
                coordinates[0] = longitude;
                coordinates[1] = (latitude);
                dictionary.Add("coordinates", coordinates);
                dictionary.Add("longitude", longitude);
                dictionary.Add("latitude", latitude);
            }
            else
            {
                throw new Exception.CloudBoostException("latitude and longitudes are not in range");
            }
        }

        public void SetLongitude(double longitude)
        {
            if (longitude >= -180 && longitude <= -180)
            {
                dictionary.Add("longitude", longitude);
                coordinates[0] = longitude;
                dictionary.Add("_isModified", true);
            }
            else
            {
                throw new Exception.CloudBoostException("Longitude is not in Range");
            }
        }

        public void SetLatitude(double latitude)
        {
            if (latitude >= -90 && latitude <= -90)
            {
                dictionary.Add("latitude", latitude);
                coordinates[1] = latitude;
                dictionary.Add("_isModified", true);
            }
            else
            {
                throw new Exception.CloudBoostException("Latitude is not in Range");
            }
        }

        public Object Get(string columnName)
        {
            return dictionary[columnName];
        }

        public void Set(string columnName, Object data)
        {
            if (columnName == "latitude")
            {
                if ((double)data >= -90 && (double)data <= -90)
                {
                    dictionary.Add("latitude", (double)data);
                    coordinates[1] = (double)data;
                    dictionary.Add("_isModified", true);
                }
                else
                {
                    throw new Exception.CloudBoostException("Latitude is not in Range");
                }
            }
            else
            {
                if ((double)data >= -180 && (double)data <= -180)
                {
                    dictionary.Add("longitude", (double)data);
                    coordinates[0] = (double)data;
                    dictionary.Add("_isModified", true);
                }
                else
                {
                    throw new Exception.CloudBoostException("Longitude is not in Range");
                }
            }
        }

        public double DistanceInKMs(CB.CloudGeoPoint point)
        {

            int earthRedius = 6371; //in Kilometer
            return earthRedius * greatCircleFormula(point);
        }

        public double DistanceInMiles(CB.CloudGeoPoint point)
        {

            int earthRedius = 3959; // in Miles
            return earthRedius * greatCircleFormula(point);

        }

        public double DistanceInRadians(CB.CloudGeoPoint point)
        {

            return this.greatCircleFormula(point);
        }

        private double greatCircleFormula(CB.CloudGeoPoint point)
        {

            coordinates = (double[])dictionary["coordinates"];
            point.coordinates = (double[])point.dictionary["coordinates"];
            
            double dLat = toRad(coordinates[1] - point.coordinates[1]);
            double dLon = toRad(coordinates[0] - point.coordinates[0]);

            double lat1 = toRad(point.coordinates[1]);
            double lat2 = toRad(coordinates[1]);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return c;
        }

        private double toRad(double number)
        {
            return number * Math.PI / 180;
           
        }
    }
}
