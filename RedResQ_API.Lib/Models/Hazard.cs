using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Models
{
    public class Hazard
    {
        #region Constructor

        public Hazard(long id, string title, double latitude, double longitude, int radius, DateTime timestamp, HazardType hazardType)
        {
            Id = id;
            Title = title;
            Latitude = latitude;
            Longitude = longitude;
            Radius = radius;
            Timestamp = timestamp;
            HazardType = hazardType;
        }

        #endregion

        #region Properties

        public long Id { get; private set; }

        public string Title { get; private set; }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public int Radius { get; private set; }

        public DateTime Timestamp { get; private set; }

        public HazardType HazardType { get; private set; }

        #endregion
    }
}
