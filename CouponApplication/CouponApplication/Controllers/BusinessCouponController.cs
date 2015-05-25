using CouponApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CouponApplication.Controllers
{
    public class BusinessCouponController : Controller
    {
        private CouponContext db = new CouponContext();
        private static string personID = null;

        //
        // GET: /BusinessCoupon/

        public ActionResult Index()
        {
            return View();
        }

        //CouponRealization
        public ActionResult CouponRealization()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CouponRealization(string searchText)
        {
            bool ans = false;
            foreach (UseCopun useC in db.UseCopuns)
            {
                if (useC.Code == searchText)
                {
                    ans = true;
                    useC.Status = "Realization";
                }
            }
            if (ans == true)
            {
                db.SaveChanges();
                return RedirectToAction("AcceptUseCoupon");
            }
            TempData["notice4"] = "--->copon code is error!!";
            return View();
        }

        public ActionResult AcceptUseCoupon()
        {
            return View((object)personID);
        }

        //CouponInOrder
        public ActionResult CouponInOrder(string id=null)
        {
            personID = id;
            List<UseCopun> UC = new List<UseCopun>();
            BusinessOwner p = db.BusinessOwners.Find(personID);
            foreach (Business b in p.Businesses)
            {
                foreach (Coupon c in b.Coupons)
                {
                    foreach (UseCopun useC in c.UsersCopon)
                    {
                        UC.Add(useC);
                    }
                }
            }
            return View(UC);
        }


        //SeeUser
        public ActionResult SeeUser(string id = null)
        {
            User use = db.Users.Find(id);
            return View(use);
        }
    }
}
