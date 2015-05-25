using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class SocialNet
    {
        public string SocialNetId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AcountName { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}