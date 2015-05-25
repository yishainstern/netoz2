using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public enum Gender { M = 0, F = 1 };

    public class User : Person
    {
        public string Location { get; set; }
        public int Age { get; set; }
        public Gender gender { get; set; }
        public Boolean didCofirm { get; set; }

        public string GPSlocationId { get; set; }
        public virtual GPS GPSlocation { get; set; }

        public virtual List<SocialNet> AccountsOf { get; set; }
        public virtual List<Category> UserPreferences { get; set; }
        public virtual List<User> Friends { get; set; }
        public virtual List<UseCopun> UserCoupons { get; set; }
    }
}