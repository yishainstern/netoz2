using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class UseCopun
    {
        public int UseCopunId { get; set; }
        public string Code { get; set; }
        public DateTime OrderDay { get; set; }
        public string receipt { get; set; }
        public string Status { get; set; }
        public int num { get; set; }
        public int rank { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int CouponId { get; set; }
        public virtual Coupon Coupon { get; set; }
        public int Cpuon_from_networkId { get; set; }
        public virtual Cpuon_from_network CouponNet { get; set; }
    }
}