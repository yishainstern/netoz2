using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool Choose { get; set; }

        public virtual List<Business> Businesses { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual List<Coupon> Coupons { get; set; }
    }
}