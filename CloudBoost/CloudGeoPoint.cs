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
        protected decimal[] coordinates = new decimal[2];
        public CloudGeoPoint(decimal longitude, decimal latitude)
        {
            dictionary.Add("_type", "point");
            dictionary.Add("_isModified", true);
            if((latitude >= (decimal)-90.0 && latitude <= (decimal)90.0) && (longitude >= (decimal)-180.0 && longitude <= (decimal)180.0))
            {
                coordinates[0] = longitude;
                coordinates[1] = latitude;
                dictionary.Add("coordinates", coordinates);
                dictionary.Add("longitude", longitude);
                dictionary.Add("latitude", latitude);
            }
            else
            {
                throw new Exception.CloudBoostException("Latitude or Longiturde are not in range. Latitude should be in between -90.0 to 90.0 and Longitude should be in between -180 to 180.");
            }
        }

        public decimal Longitude
        {
            get
            {
                return (decimal)dictionary["longitude"];
            }
            set
            {
                var longitude = value;
                if (longitude >= -180 && (longitude <= -180))
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
            
        }

        public decimal Latitude
        {
            get
            {
                return (decimal)dictionary["latitude"];
            }
            set
            {
                var latitude = value;
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
                    coordinates[1] = (decimal)data;
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
                    coordinates[0] = (decimal)data;
                    dictionary.Add("_isModified", true);
                }
                else
                {
                    throw new Exception.CloudBoostException("Longitude is not in Range");
                }
            }
        }

        public decimal DistanceInKMs(CB.CloudGeoPoint point)
        {
            int earthRedius = 6371; //in Kilometer
            return earthRedius * greatCircleFormula(point);
        }

        public decimal DistanceInMiles(CB.CloudGeoPoint point)
        {
            int earthRedius = 3959; // in Miles
            return earthRedius * greatCircleFormula(point);

        }

        public decimal DistanceInRadians(CB.CloudGeoPoint point)
        {
            return this.greatCircleFormula(point);
        }

        private decimal greatCircleFormula(CB.CloudGeoPoint point)
        {

            coordinates = (decimal[])dictionary["coordinates"];
            point.coordinates = (decimal[])point.dictionary["coordinates"];

            decimal dLat = (decimal)toRad((double)(coordinates[1] - point.coordinates[1]));
            decimal dLon = (decimal)toRad((double)(coordinates[0] - point.coordinates[0]));

            decimal lat1 = (decimal)toRad((double)point.coordinates[1]);
            decimal lat2 = (decimal)toRad((double)coordinates[1]);

            decimal a = (decimal)(Math.Sin((double)dLat / 2) * Math.Sin((double)dLat / 2) + Math.Sin((double)dLon / 2) * Math.Sin((double)dLon / 2) * Math.Cos((double)lat1) * Math.Cos((double)lat2));
            decimal c = (decimal)(2 * Math.Atan2(Math.Sqrt((double)a), Math.Sqrt(1 - (double)a)));

            return c;
        }

        private double toRad(double number)
        {
            return number * Math.PI / 180;
        }
    }
}
