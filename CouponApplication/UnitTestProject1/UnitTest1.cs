using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CouponApplication;
using CouponApplication.Controllers;
using CouponApplication.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        CouponContext db = new CouponContext();
       
        [TestMethod]
        public void TestSignIn()
        {
           // AppDomain.CurrentDomain.SetData("DataDirectory", "../CouponApplication/App_Data");
            Database.SetInitializer<CouponContext>(null);
            UserController controller = new UserController(db);
            User use = new User();
            use.PersonId = "123";
            use.Name = "dodo";
            use.Email = "dodo@hotmail.com";
            use.Password = "123";
            use.Location = "haifa";
            use.Phone = "0555";
            use.Age = 20;
            use.gender = 0;
            AllModel m = new AllModel();
            m.User = use;
            m.Categories = new List<Category>();
            m.Categories.Add(new Category { Name = "food", Choose = true, CategoryId = 1 });
            m.Categories.Add(new Category { Name = "eat", Choose = true, CategoryId = 2 });
            m.Categories.Add(new Category { Name = "muzic", Choose = true, CategoryId = 3 });
            controller.SignUp(m);
            foreach (User u in db.Users)
            {
                if (u.Email == use.Email)
                {
                    Assert.IsTrue(true);
                    return;

                }
            }
            Assert.Fail();


        }

        [TestMethod]
        public void TestLogIn()
        {
            CouponContext db = new CouponContext();
            UserController controller = new UserController(db);

            User use = new User();
            use.Email = "dodo@hotmail.com";
            use.Password = "123";
            var result = controller.LogIn(use) as RedirectToRouteResult;
            Assert.AreEqual("AfterLogin", result.RouteValues["action"]);
        }


        [TestMethod]
        public void TestForgetPass()
        {
            CouponContext db = new CouponContext();
            UserController controller = new UserController();

            User use = new User();
            use.Email = "dodo@hotmail.com";
            var result = controller.MyPass(use) as RedirectToRouteResult;
            Assert.IsNull(result);
        }



        //AddbusinessOwner
        [TestMethod]
        public void TestAddbusinessOwner()
        {
            CouponContext db = new CouponContext();
            ManagerController controller = new ManagerController("1212");

            BusinessOwner Owner = new BusinessOwner();
            Owner.PersonId = "1212";
            Owner.Name = "dede";
            Owner.Password = "123";
            Owner.Email = "dede@hotmail.com";
            Owner.Phone = "05454";
            Owner.Businesses = new List<Business>();
            var result = controller.AddbusinessOwner(Owner) as RedirectToRouteResult;
            foreach (BusinessOwner o in db.BusinessOwners)
            {
                if (o.Email == Owner.Email && o.PersonId == Owner.PersonId)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.AreEqual("businessManagment", result.RouteValues["action"]);
        }

       // Addbusiness
           [TestMethod]
           public void TestAddbusiness()
           {
               Database.SetInitializer<CouponContext>(null);
               ManagerController controller = new ManagerController(db);

               BusinessOwner use = new BusinessOwner();
               use.PersonId = "123";
               use.Name = "dodo";
               use.Email = "dodo@hotmail.com";
               use.Password = "123";
               use.Phone = "0555";
               
               controller.AddbusinessOwner(use);
               Business B = new Business();
               B.Adress = "naharya";
               B.City = "naharya";
               B.Name = "castro";
               B.Status = "מאושר";
               B.Coupons = new List<Coupon>();
               B.BusinessCategory = new List<Category>();
               B.Owner = use;
               B.OwnerId = "123";
               AllModel m = new AllModel();
               m.Business = B;
               m.Categories = new List<Category>();
               m.Categories.Add(new Category { Name = "food", Choose = true, CategoryId = 1 });
               m.Categories.Add(new Category { Name = "eat", Choose = true, CategoryId = 2 });
               m.Categories.Add(new Category { Name = "muzic", Choose = true, CategoryId = 3 });
               controller.Addbusiness(m);
               foreach (Business bes in db.Businesses)
               {
                   if (bes.Name == B.Name && bes.City == B.City)
                   {
                       Assert.IsTrue(true);
                       return;
                   }
               }
               Assert.Fail();
           }



        //SearchBusiness that excits and not
        [TestMethod]
        public void TestSearchBusinessIsIn()
        {
            bool flag,flag2 = false;
            CouponContext db = new CouponContext();
            Database.SetInitializer<CouponContext>(null);
            ManagerController controller = new ManagerController(db);
            BusinessOwner use = new BusinessOwner();
            use.PersonId = "123";
            use.Name = "dodo";
            use.Email = "dodo@hotmail.com";
            use.Password = "123";
            use.Phone = "0555";

            controller.AddbusinessOwner(use);
            Business B = new Business();
            B.Adress = "naharya";
            B.City = "naharya";
            B.Name = "castro";
            B.Status = "מאושר";
            B.Coupons = new List<Coupon>();
            B.BusinessCategory = new List<Category>();
            B.Owner = use;
            B.OwnerId = "123";
            AllModel m = new AllModel();
            m.Business = B;
            m.Categories = new List<Category>();
            m.Categories.Add(new Category { Name = "food", Choose = true, CategoryId = 1 });
            m.Categories.Add(new Category { Name = "eat", Choose = true, CategoryId = 2 });
            m.Categories.Add(new Category { Name = "muzic", Choose = true, CategoryId = 3 });
            controller.Addbusiness(m);

            //second bussiness

            BusinessOwner use1 = new BusinessOwner();
            use1.PersonId = "123";
            use1.Name = "dodo";
            use1.Email = "dodo@hotmail.com";
            use1.Password = "123";
            use1.Phone = "0555";

            controller.AddbusinessOwner(use1);
            Business B1 = new Business();
            B1.Adress = "haifa";B1.City = "hafia";B1.Name = "castro";B1.Status = "מאושר";
            B1.Coupons = new List<Coupon>();
            B1.BusinessCategory = new List<Category>();
            B1.Owner = use1;
            B1.OwnerId = "123";
            AllModel m1 = new AllModel();
            m1.Business = B1;
            m1.Categories = new List<Category>();
            m1.Categories.Add(new Category { Name = "eat", Choose = true, CategoryId = 2 });
            m1.Categories.Add(new Category { Name = "muzic", Choose = true, CategoryId = 3 });
            controller.Addbusiness(m1);
            BusinessController controllerq = new BusinessController();
            Business serch = new Business();
            serch.Adress = "haifa";serch.City = "haifa";serch.Name = "castro";serch.Status = "מאושר";
            serch.Coupons = new List<Coupon>();
            serch.BusinessCategory = new List<Category>();
            serch.Owner = use1;serch.OwnerId = "123";AllModel m2 = new AllModel();
            m2.Business = serch;
            m2.Categories = new List<Category>();
            m2.Categories.Add(new Category { Name = "sport", Choose = true, CategoryId = 3 });
           object bb =  controllerq.SearchBusiness(serch);
           if (bb.GetType().Name == "ViewResults"){flag = true;}
           var result = controllerq.SearchBusiness(serch) as RedirectToRouteResult;
           Assert.AreEqual("ShowSearchBusiness", result.RouteValues["action"]);
        }


        [TestMethod]
        public void TestSearchBusinessIsNotIn()
        {
            bool flag, flag2 = false;
            CouponContext db = new CouponContext();
            Database.SetInitializer<CouponContext>(null);
            ManagerController controller = new ManagerController(db);
            BusinessOwner use = new BusinessOwner();
            use.PersonId = "123";
            use.Name = "dodo";
            use.Email = "dodo@hotmail.com";
            use.Password = "123";
            use.Phone = "0555";

            controller.AddbusinessOwner(use);
            Business B = new Business();
            B.Adress = "naharya";
            B.City = "naharya";
            B.Name = "castro";
            B.Status = "מאושר";
            B.Coupons = new List<Coupon>();
            B.BusinessCategory = new List<Category>();
            B.Owner = use;
            B.OwnerId = "123";
            AllModel m = new AllModel();
            m.Business = B;
            m.Categories = new List<Category>();
            m.Categories.Add(new Category { Name = "food", Choose = true, CategoryId = 1 });
            m.Categories.Add(new Category { Name = "eat", Choose = true, CategoryId = 2 });
            m.Categories.Add(new Category { Name = "muzic", Choose = true, CategoryId = 3 });
            controller.Addbusiness(m);

            //second bussiness

            BusinessOwner use1 = new BusinessOwner();
            use1.PersonId = "123";
            use1.Name = "dodo";
            use1.Email = "dodo@hotmail.com";
            use1.Password = "123";
            use1.Phone = "0555";

            controller.AddbusinessOwner(use1);
            Business B1 = new Business();
            B1.Adress = "haifa";
            B1.City = "hafia";
            B1.Name = "castro";
            B1.Status = "מאושר";
            B1.Coupons = new List<Coupon>();
            B1.BusinessCategory = new List<Category>();
            B1.Owner = use1;
            B1.OwnerId = "123";
            AllModel m1 = new AllModel();
            m1.Business = B1;
            m1.Categories = new List<Category>();
            m1.Categories.Add(new Category { Name = "eat", Choose = true, CategoryId = 2 });
            m1.Categories.Add(new Category { Name = "muzic", Choose = true, CategoryId = 3 });
            controller.Addbusiness(m1);

            BusinessController controllerq = new BusinessController();
            Business serch = new Business();
            serch.Adress = "haifa";
            serch.City = "ddddddd";
            serch.Name = "castro";
            serch.Status = "מאושר";
            serch.Coupons = new List<Coupon>();
            serch.BusinessCategory = new List<Category>();
            serch.Owner = use1;
            serch.OwnerId = "123";
            AllModel m2 = new AllModel();
            m2.Business = serch;
            m2.Categories = new List<Category>();
            m2.Categories.Add(new Category { Name = "sport", Choose = true, CategoryId = 3 });
            object bb = controllerq.SearchBusiness(serch);
            if (bb.GetType().Name == "ViewResult")
            {
                Assert.IsTrue(true);
                return;
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        //add category
         [TestMethod]
        public void addCategory()
        {
            Database.SetInitializer<CouponContext>(null);
            ManagerController controller = new ManagerController(db);

            Category c = new Category();
            c.Name = "shlom";
            c.Choose = false;

            controller.AddCategory(c);

            foreach (Category u in db.Categories)
            {
                if (u.Name == c.Name)
                {
                    Assert.IsTrue(true);
                    return;

                }
            }
            Assert.Fail();
        }



         [TestMethod]
         public void addCupon()
         {
             bool flag, flag2 = false;
             CouponContext db = new CouponContext();
             Database.SetInitializer<CouponContext>(null);
             ManagerController controller = new ManagerController(db);
             BusinessOwner use = new BusinessOwner();
             use.PersonId = "123";
             use.Name = "dodo";
             use.Email = "dodo@hotmail.com";
             use.Password = "123";
             use.Phone = "0555";

             controller.AddbusinessOwner(use);
             Business B = new Business();
             B.Adress = "naharya";
             B.City = "naharya";
             B.Name = "castro";
             B.Status = "מאושר";
             B.Coupons = new List<Coupon>();
             B.BusinessCategory = new List<Category>();
             B.Owner = use;
             B.OwnerId = "123";
             AllModel m = new AllModel();
             m.Business = B;
             m.Categories = new List<Category>();
             m.Categories.Add(new Category { Name = "food", Choose = true, CategoryId = 1 });
             m.Categories.Add(new Category { Name = "eat", Choose = true, CategoryId = 2 });
             m.Categories.Add(new Category { Name = "muzic", Choose = true, CategoryId = 3 });
             controller.Addbusiness(m);

             //new coupun
             Coupon b = new Coupon();
             b.Business = B;
             b.BusinessId = 1;
             b.Categories = m.Categories;
             b.CouponId = 1;
             b.CurrentPrice = 20;
             DateTime cc = new DateTime();
             
             //b.deadline = ddd;
         }
    }
}
