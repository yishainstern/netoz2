using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class Notifications
    {
        public int NotificationsId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public string ManagerId { get; set; }
        public virtual Manager Manager { get; set; }
    }
}