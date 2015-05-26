using CouponApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CouponApplication.Controllers
{
    public class ManagerController : Controller
    {
        private CouponContext db = new CouponContext();
        private static string OwnerId = null;
        private static int busnissID = 0;

        public ManagerController()
        {
        }

        public ManagerController(string s) 
        {
            OwnerId = s;
        }
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        //businessManagment
        public ActionResult businessManagment()
        {
            List<Person> ListP = new List<Person>();
            List<User> ListU = new List<User>();
            foreach (User p in db.Users)
            {
                ListU.Add(p);
            }
            foreach (Person p in db.BusinessOwners)
            {
                if (!ListU.Contains(p))
                {
                    ListP.Add(p);
                }
            }

            return View(ListP);
        }

        //messages
        public ActionResult messages()
        {
            return View(db.Notifications.ToList());
        }
        //Delete
        public ActionResult Delete(string id=null)
        {
            Notifications not = db.Notifications.Find(id);
            db.Notifications.Remove(not);
            db.SaveChanges();
            return RedirectToAction("messages");
        }

        //DeleteBusinessById
        public ActionResult DeleteBusinessById()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AcceptDeleteBusinessById(string SearchText)
        {
            Business B = db.Businesses.Find(SearchText);
            if (B != null)
            {
                db.Businesses.Remove(B);
                db.SaveChanges();
                return RedirectToAction("DeletedMassege");
            }
            TempData["notice5"] = "-->business not found!!";
            return RedirectToAction("DeleteBusinessById");
        }
        //Deleted
        public ActionResult DeletedMassege()
        {
            return View();
        }





        //DeleteBusinessById
        public ActionResult acceptBusinessRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult acceptBusiness(string SearchText)
        {
            Business B = db.Businesses.Find(SearchText);
            if (B != null)
            {
                B.Status="מאושר";
                db.SaveChanges();
                return RedirectToAction("AcceptMassege");
            }
            TempData["notice55"] = "-->business not found!!";
            return RedirectToAction("acceptBusinessRegister");
        }
        //AcceptMassege
        public ActionResult AcceptMassege()
        {
            return View();
        }




        //acceptCouponRegister
        public ActionResult acceptCouponRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult acceptCoupon(string SearchText)
        {
            Coupon C = db.Coupons.Find(SearchText);
            if (C != null)
            {
                C.Status = "מאושר";
                db.SaveChanges();
                return RedirectToAction("AcceptCouponMassege");
            }
            TempData["notice555"] = "-->Coupon not found!!";
            return RedirectToAction("acceptCouponRegister");
        }
        //Deleted
        public ActionResult AcceptCouponMassege()
        {
            return View();
        }




        //business
        public ActionResult business(string id = null, string Admin = "true")
        {
            Session["Admin"] = Admin;
            OwnerId = id;
            List<Business> ListB = new List<Business>();
            foreach (BusinessOwner owner in db.BusinessOwners)
            {
                if (owner.PersonId == id)
                {
                    ListB = owner.Businesses;
                }
            }
            return View(ListB);
        }



        //AddbusinessOwner
        public ActionResult AddbusinessOwner()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddbusinessOwner(BusinessOwner user)
        {
            if (ModelState.IsValid)
            {
                //לבדוק שלא קיים אותו בעל עסק
                var owner = db.BusinessOwners.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(user.Password)).FirstOrDefault();

                if (owner == null)
                {
                    //send email to owner with here password
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

                    message.From = new System.Net.Mail.MailAddress("daved123daved@hotmail.com");
                    message.To.Add(new System.Net.Mail.MailAddress(user.Email));

                    message.IsBodyHtml = true;
                    message.BodyEncoding = Encoding.UTF8;
                    message.Subject = "Add Owner";
                    message.Body = "hi Owner, you can logIn in the application with email:  " + user.Email + " and your password is: " + user.Password;

                    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                    client.Send(message);
                    //
                    db.BusinessOwners.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("businessManagment");
                }
                else {
                    TempData["notice60"] = "--->this owner was existed!!!";
                    return View(user);    
                }
            }

            return View(user);
        }

        //Addbusiness
        public ActionResult Addbusiness()
        {
            var models = new AllModel();
            models.Categories = db.Categories.ToList();
            return View(models);
        }

        [HttpPost]
        public ActionResult Addbusiness(AllModel b)
        {
            string ansAdmin = "false";
            bool owner = false;
            bool field = true;
            if (Session != null) {
                if (Session["Admin"].ToString() == "true")
                {
                    ansAdmin = "true";
                    b.Business.Status = "מאושר";
                }
                else
                {
                    Notifications not = db.Notifications.Find(b.Business.BusinessId);
                    if (not == null)
                    {
                        not = new Notifications();
                        not.NotificationsId = b.Business.BusinessId;
                        not.Manager = db.Manager.First();
                        not.ManagerId = db.Manager.First().PersonId;
                        not.Type = "Accept Business";
                        not.Content = "Business id= " + b.Business.BusinessId;
                        db.Manager.First().Notifications.Add(not);
                    }
                    else
                    {
                        not.Type = "Accept Business";
                        not.Content = "Business id= " + b.Business.BusinessId;
                    }
                    b.Business.Status = "לא מאושר";
                }
            }
            if (ModelState.IsValid)
            {
                foreach (Person p in db.BusinessOwners)
                {
                    if (p.PersonId == OwnerId)
                    {
                        owner = true;
                    }
                }
                if (string.IsNullOrEmpty(b.Business.Name) || b.Business.BusinessId==null || string.IsNullOrEmpty(b.Business.Adress) || string.IsNullOrEmpty(b.Business.City))
                {
                    TempData["notice2"] = "--->all fields are required!!!"; field = false;
                }
                if (owner == false)
                {
                    TempData["notice1"] = "--->the owner id was not exist!!!";
                }
                if (field == true && owner == true)
                {
                    foreach (BusinessOwner p in db.BusinessOwners)
                    {
                        if (p.PersonId == OwnerId)
                        {
                            b.Business.Owner = p;
                            b.Business.OwnerId = OwnerId;
                            p.Businesses.Add(b.Business);
                            foreach (Category C in b.Categories)
                            {
                                if (C.Choose == true)
                                {
                                    Category cc = db.Categories.Find(C.CategoryId);
                                    cc.Businesses.Add(b.Business);
                                    if (b.Business.BusinessCategory==null)
                                    {
                                        b.Business.BusinessCategory = new List<Category>();
                                    }
                                    b.Business.BusinessCategory.Add(cc);
                                }
                            }
                        }
                    }
                    db.Businesses.Add(b.Business);
                    db.SaveChanges();

                    return RedirectToAction("business", "Manager", new { id = OwnerId, Admin = ansAdmin });
                }
            }

            return View(b);
        }

        //Categories
        public ActionResult Categories(int id = 0)
        {
            busnissID = id;
            List<Category> ListC = new List<Category>();
            foreach (Business B in db.Businesses)
            {
                if (B.BusinessId == id)
                {
                    ListC = B.BusinessCategory;
                }
            }
            return View(ListC);
        }

        //AddCategory
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category C)
        {
            bool exist = false;
            if (ModelState.IsValid)
            {
                foreach (Category cate in db.Categories)
                {
                    if (cate.CategoryId == C.CategoryId)
                    {
                        exist = true;
                        foreach (Business b in db.Businesses)
                        {
                            if (b.BusinessId == busnissID)
                            {
                                b.BusinessCategory.Add(cate);
                                cate.Businesses.Add(b);
                            }
                        }
                    }
                }
                if (exist == false)
                {
                    db.Categories.Add(C);

                    foreach (Business b in db.Businesses)
                    {
                        if (b.BusinessId == busnissID)
                        {
                            b.BusinessCategory.Add(C);
                            if (C.Businesses == null)
                            {
                                C.Businesses = new List<Business>();
                            }
                            C.Businesses.Add(b);
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Categories", "Admin", new { id = busnissID });
            }

            return View(C);
        }
        //Copons
        public ActionResult Copons(int id = 0)
        {
            busnissID = id;
            List<Coupon> ListC = new List<Coupon>();
            foreach (Business B in db.Businesses)
            {
                if (B.BusinessId == id)
                {
                    ListC = B.Coupons;
                }
            }
            return View(ListC);
        }

        //AddCopons
        public ActionResult AddCopons()
        {
            var models = new AllModel();
            models.Categories = db.Categories.ToList();

            return View(models);
        }

        [HttpPost]
        public ActionResult AddCopons(AllModel M)
        {
            M.Coupon.Categories = new List<Category>();
            if (Session["Admin"].ToString() == "true")
            {
                M.Coupon.Status = "מאושר";
            }
            else {
                Notifications not = db.Notifications.Find(M.Coupon.CouponId);
                if (not == null)
                {
                    not = new Notifications();
                    not.NotificationsId = M.Coupon.CouponId;
                    not.Manager = db.Manager.First();
                    not.ManagerId = db.Manager.First().PersonId;
                    not.Type = "Accept coupon";
                    not.Content = "coupon id= " + M.Coupon.CouponId;
                    db.Manager.First().Notifications.Add(not);
                }
                else {
                    not.Type = "Accept coupon";
                    not.Content = "coupon id= " + M.Coupon.CouponId;
                }
               
                M.Coupon.Status = "לא מאושר";
            }
            bool exist = false;
            if (ModelState.IsValid)
            {
                foreach (Coupon cate in db.Coupons)
                {
                    if (cate.CouponId == M.Coupon.CouponId)
                    {
                        exist = true;
                        foreach (Category C in M.Categories)
                        {
                            if (C.Choose == true)
                            {
                                Category cc = db.Categories.Find(C.CategoryId);
                                cc.Coupons.Add(cate);
                                cate.Categories.Add(cc);
                            }
                        }

                        foreach (Business b in db.Businesses)
                        {
                            if (b.BusinessId == busnissID)
                            {
                                b.Coupons.Add(cate);
                                cate.Business = b;
                                cate.BusinessId = b.BusinessId;
                            }
                        }
                    }
                }
                if (exist == false)
                {

                    foreach (Category C in M.Categories)
                    {
                        if (C.Choose == true)
                        {
                            Category cc = db.Categories.Find(C.CategoryId);
                            cc.Coupons.Add(M.Coupon);
                            M.Coupon.Categories.Add(cc);
                        }
                    }

                    db.Coupons.Add(M.Coupon);
                    foreach (Business b in db.Businesses)
                    {
                        if (b.BusinessId == busnissID)
                        {
                            b.Coupons.Add(M.Coupon);
                            M.Coupon.Business = b;
                            M.Coupon.BusinessId = b.BusinessId;
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Copons", "Manager", new { id = busnissID });
            }

            return View(M);
        }

        //EditCoupon
        public ActionResult EditCoupon(string id = null)
        {
            Coupon copon = db.Coupons.Find(id);
            return View(copon);
        }


        [HttpPost]
        public ActionResult EditCoupon(Coupon copon)
        {
            int Bid=0;
            if (ModelState.IsValid)
            {
                foreach (Coupon C in db.Coupons)
                {
                    if (C.CouponId == copon.CouponId)
                    {
                        C.Categories = copon.Categories;
                        C.Name = copon.Name;
                        C.Description = copon.Description;
                        C.OriginalPrice = copon.OriginalPrice;
                        C.CurrentPrice = copon.CurrentPrice;
                        C.deadline = copon.deadline;
                        C.Status = "לא מאושר";
                        Bid = C.BusinessId;
                        //
                        Notifications not = db.Notifications.Find(copon.CouponId);
                        if (not == null)
                        {
                            not = new Notifications();
                            not.NotificationsId = copon.CouponId;
                            not.Manager = db.Manager.First();
                            not.ManagerId = db.Manager.First().PersonId;
                            not.Type = "Accept coupon";
                            not.Content = "coupon id= " + copon.CouponId;
                            db.Manager.First().Notifications.Add(not);
                        }
                        else
                        {
                            not.Type = "Accept coupon";
                            not.Content = "coupon id= " +copon.CouponId;
                        }
                        //
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Copons", "Manager", new { id = Bid });
            }
            return View(copon);
        }

        //RemoveCoupon
        public ActionResult RemoveCoupon(string id = null)
        {
            Coupon copon = db.Coupons.Find(id);
            return View(copon);
        }

        //
        // POST: /Users/Delete/5

        [HttpPost, ActionName("RemoveCoupon")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Coupon copon = db.Coupons.Find(id);
            db.Coupons.Remove(copon);
            db.SaveChanges();
            return RedirectToAction("Copons", "Manager", new { id = busnissID });
        }

        //EditBusnis
        public ActionResult EditBusnis(string id = null)
        {
            Business Busnis = db.Businesses.Find(id);
            return View(Busnis);
        }


        [HttpPost]
        public ActionResult EditBusnis(Business Busnis)
        {
            if (ModelState.IsValid)
            {
                foreach (Business B in db.Businesses)
                {
                    if (B.BusinessId == Busnis.BusinessId)
                    {
                        Notifications not = db.Notifications.Find(Busnis.BusinessId);
                        if (not == null)
                        {
                            not = new Notifications();
                            not.NotificationsId = Busnis.BusinessId;
                            not.Manager = db.Manager.First();
                            not.ManagerId = db.Manager.First().PersonId;
                            not.Type = "Accept Business";
                            not.Content = "Business id= " + Busnis.BusinessId;
                            db.Manager.First().Notifications.Add(not);
                        }
                        else
                        {
                            not.Type = "Accept Business";
                            not.Content = "Business id= " + Busnis.BusinessId;
                        }
                        B.Status = "לא מאושר";
                        B.Name = Busnis.Name;
                        B.Adress = Busnis.Adress;
                        B.City = Busnis.City;
                    }
                }
                db.SaveChanges();
                return RedirectToAction("business", "Manager", new { id = OwnerId, Admin = "false" });
            }
            return View(Busnis);
        }


        //DeleteBusnis
        public ActionResult DeleteBusnis(string id = null)
        {
            Business Busnis = db.Businesses.Find(id);

                Notifications not = db.Notifications.Find(id);
                if (not == null)
                {
                    not = new Notifications();
                    not.NotificationsId = Busnis.BusinessId;
                    not.Manager = db.Manager.First();
                    not.ManagerId = db.Manager.First().PersonId;
                    not.Type = "Delete Business";
                    not.Content = "Business id= " + Busnis.BusinessId;
                    db.Manager.First().Notifications.Add(not);
                }
                else 
                {
                    not.Type = "Delete Business";
                    not.Content = "Business id= " + Busnis.BusinessId;
                }
               
                db.SaveChanges();

            return View();
        }

    }
}
