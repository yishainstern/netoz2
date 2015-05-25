using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class BusinessOwner:Person
    {
        public virtual List<Business> Businesses { get; set; }
    }
}