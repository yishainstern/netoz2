using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CouponApplication.Models
{
    public class CouponContext : DbContext
    {
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessOwner> BusinessOwners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Cpuon_from_network> Cpuons_netwrok { get; set; }
        public DbSet<GPS> GPS { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Sensor> Sensor { get; set; }
        public DbSet<SocialNet> SocialNets { get; set; }
        public DbSet<UseCopun> UseCopuns { get; set; }
        public DbSet<User> Users { get; set; }
    }
}