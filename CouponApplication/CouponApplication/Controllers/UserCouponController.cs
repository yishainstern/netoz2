using CouponApplication.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CouponApplication.Controllers
{
    public class UserCouponController : Controller
    {

        private CouponContext db = new CouponContext();
        private static List<Coupon> ListC = null;
        private static List<Business> ListB = null;
        private static int coponID = 0;
        private static string useID = null;
        private static string usecoponID = null;


        //
        // GET: /UserCoupon/

        public ActionResult Index()
        {
            return View();
        }


        //ShowFavoritesCoupons
        public ActionResult ShowFavoritesCoupons()
        {
            List<Coupon> copon = new List<Coupon>();
            foreach (User u in db.Users)
            {
                if (u.PersonId == Session["LogedUserID"].ToString())
                {
                    foreach (Coupon c in db.Coupons)
                    {
                        foreach (Category cate in u.UserPreferences)
                        {
                            if (c.Categories.Contains(cate) && !copon.Contains(c))
                            {
                                copon.Add(c);
                            }
                        }
                    }
                }
            }

            return View(copon);
        }


        //AddCouponFromNetWork
        public ActionResult AddCouponFromNetWork()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCouponFromNetWork(Cpuon_from_network C)
        {
            if (ModelState.IsValid)
            {
                C.Cpuon_from_networkId = C.CouponId;
                db.Cpuons_netwrok.Add(C);
                db.SaveChanges();
                return RedirectToAction("CouponFromNetWork");
            }

            return View(C);
        }

        //CouponFromNetWork
        public ActionResult CouponFromNetWork()
        {
            return View();
        }

        //SearchCoupon
        public ActionResult SearchCoupon()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchCoupon(Coupon C)
        {
            bool ans = false;
            ListC = new List<Coupon>();
            foreach (Coupon Copon in db.Coupons)
            {
                foreach (Category cate in Copon.Categories)
                {
                    if(C.Categories.Count!=0)
                    if (cate.Name.Equals(C.Categories[0].Name))
                    {
                        ListC.Add(Copon);
                        ans = true;
                    }

                    if (Copon.Business != null)
                    {
                        if (Copon.Business.Name.Equals(C.Business.Name) || Copon.Business.City.Equals(C.Business.City))
                        {

                            if (ans == false)
                            {
                                ListC.Add(Copon);
                                ans = true;
                            }

                        }
                    }
                }
            }
            if (ans == false)
            {
                return View(C);
            }
            else
            {
                return RedirectToAction("ShowCoupons");
            }
        }

        //ShowCoupons
        public ActionResult ShowCoupons()
        {
            return View(ListC);
        }


        //ShowCopons
        public ActionResult ShowCopons(int id = 0)
        {
            List<Coupon> ListCopon = new List<Coupon>();
            foreach (Business B in db.Businesses)
            {
                if (B.BusinessId == id)
                {
                    foreach (Coupon C in B.Coupons)
                    {
                        ListCopon.Add(C);
                    }
                }
            }
            return View(ListCopon);
        }

        //OrderCoupon
        public ActionResult OrderCoupon(string id=null)
        {
            useID = id;
            return View(db.Coupons.ToList());
        }

        //ToOrder
        public ActionResult ToOrder(int id = 0)
        {
            coponID = id;
            return View();
        }

        [HttpPost]
        public ActionResult ToOrder(UseCopun useCopon)
        {
            string email = null;
            if (useCopon.Code == null || useCopon.OrderDay == null || useCopon.rank < 1 || useCopon.rank > 5)
            {
                TempData["notice3"] = "--->all fields are required!!!";
                return View(useCopon);
            }
            else
            {
                useCopon.Status = "Ordering";
                useCopon.UserId = useID;
                useCopon.CouponId = coponID;
                useCopon.UseCopunId = coponID;
                foreach (Coupon C in db.Coupons)
                {
                    if (C.CouponId == coponID)
                    {
                        useCopon.Coupon = C;
                        C.UsersCopon.Add(useCopon);
                    }
                }
                foreach (User U in db.Users)
                {
                    if (U.PersonId == useID)
                    {
                        email = U.Email;
                        useCopon.User = U;
                        U.UserCoupons.Add(useCopon);
                    }
                }
                db.SaveChanges();
                //send kpala to email
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

                message.From = new System.Net.Mail.MailAddress("daved123daved@hotmail.com");
                message.To.Add(new System.Net.Mail.MailAddress(email));

                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;
                message.Subject = "Copon Key";
                message.Body = "your Serial Key for Coupon that you ordered:  " + useCopon.Code;

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Send(message);

                return RedirectToAction("CoponWasSending");
            }
        }


        //ShowUseCopon
        public ActionResult CoponWasSending()
        {
            return View();
        }


        //CouponTatOrdered
        public ActionResult CouponThatOrdered()
        {
            List<UseCopun> UseC = new List<UseCopun>();
            foreach (User use in db.Users)
            {
                if (use.PersonId == useID)
                {
                    UseC = use.UserCoupons;
                }
            }
            return View(UseC);
        }

        //ChangeRank
        public ActionResult ChangeRank(string id = null)
        {
            usecoponID = id;
            UseCopun usecopon = db.UseCopuns.Find(id);

            return View(usecopon);

        }

        [HttpPost]
        public ActionResult ChangeRank(UseCopun useCopon)
        {
            if (useCopon.rank < 1 || useCopon.rank > 5) { return View(useCopon); }
            UseCopun usecopon = db.UseCopuns.Find(usecoponID);
            usecopon.rank = useCopon.rank;
            db.Entry(usecopon).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("CouponThatOrdered");
        }

        //SeeCoupon
        public ActionResult SeeCoupon(string id = null)
        {
            Coupon copon = db.Coupons.Find(id);
            return View(copon);
        }




    }
}
