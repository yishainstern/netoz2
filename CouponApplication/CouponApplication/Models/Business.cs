using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class Business
    {
        public int BusinessId { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string Status { get; set; }

        public int GPSlocationId { get; set; }
        public virtual GPS GPSlocation { get; set; }
        public virtual List<Coupon> Coupons { get; set; }
        public virtual List<Category> BusinessCategory { get; set; }

        public string OwnerId { get; set; }
        public virtual BusinessOwner Owner { get; set; }
    }
}