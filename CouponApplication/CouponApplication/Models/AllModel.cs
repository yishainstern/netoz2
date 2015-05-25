using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class AllModel
    {
        public List<Business> Businesses { get; set; }
        public List<BusinessOwner> BusinessOwners { get; set; }
        public List<Category> Categories { get; set; }
        public List<Coupon> Coupons { get; set; }
        public List<Cpuon_from_network> Cpuons_netwrok { get; set; }
        public List<GPS> GPS { get; set; }
        public List<Manager> Manager { get; set; }
        public List<Notifications> Notifications { get; set; }
        public List<Person> Person { get; set; }
        public List<Sensor> Sensor { get; set; }
        public List<SocialNet> SocialNets { get; set; }
        public List<UseCopun> UseCopuns { get; set; }
        public List<User> Users { get; set; }

        public Business Business { get; set; }
        public BusinessOwner BusinessOwner { get; set; }
        public Category Category { get; set; }
        public Coupon Coupon { get; set; }
        public Cpuon_from_network Cpuon_netwrok { get; set; }
        public GPS GP { get; set; }
        public Manager Manag { get; set; }
        public Notifications Notification { get; set; }
        public Person Pers { get; set; }
        public Sensor Sens { get; set; }
        public SocialNet SocialNet { get; set; }
        public UseCopun UseCopun { get; set; }
        public User User { get; set; }
    }
}