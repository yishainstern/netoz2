using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class Cpuon_from_network : Coupons
    {
        public int Cpuon_from_networkId { get; set; }
        public string Link { get; set; }
        public string netwrok_name { get; set; }

        public virtual List<UseCopun> UsersCopon { get; set; }
    }
}