using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class GPS: Sensor
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}