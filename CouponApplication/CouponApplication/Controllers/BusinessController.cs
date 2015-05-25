using CouponApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CouponApplication.Controllers
{
    public class BusinessController : Controller
    {
        private CouponContext db = new CouponContext();
        private static List<Business> ListB = null;
        private static string personID = null;

        //
        // GET: /Business/

        public ActionResult Index(string id = null)
        {
            TempData["Pid"] = id;
            personID = id;
            return View();
        }

        //Showbusiness
        public ActionResult Showbusiness(string id = null)
        {
            foreach (Business b in db.Businesses)
            {
                if (b.BusinessId == id)
                {
                    return View(b);
                }
            }
            return RedirectToAction("ShowFavoritesCoupons","UserCoupon",null);
        }


        //SearchBusiness
        public ActionResult SearchBusiness()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchBusiness(Business B)
        {
            bool ans = false;
            ListB = new List<Business>();
            foreach (Business Busnis in db.Businesses)
            {
                if (Busnis.City.Equals(B.City))
                {
                    ListB.Add(Busnis);
                    ans = true;
                }
                foreach (Category cat in Busnis.BusinessCategory)
                {
                    if (cat.Name.Equals(B.Name))
                    {
                        ListB.Add(Busnis);
                        ans = true;
                    }
                }
            }
            if (ans == false)
            {
                return View(B);
            }
            else
            {
                return RedirectToAction("ShowSearchBusiness");
            }
        }


        public ActionResult ShowSearchBusiness()
        {
            return View(ListB);
        }
    }
}
