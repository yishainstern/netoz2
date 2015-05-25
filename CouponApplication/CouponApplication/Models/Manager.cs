using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class Manager:Person
    {
        public virtual List<Notifications> Notifications { get; set; }
    }
}