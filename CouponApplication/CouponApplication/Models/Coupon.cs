using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class Coupon:Coupons
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double OriginalPrice { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime deadline { get; set; }
        public string Status { get; set; }

        public virtual List<UseCopun> UsersCopon { get; set; }
        public virtual List<Category> Categories { get; set; }

        public int BusinessId { get; set; }
        public virtual Business Business { get; set; }
    }
}