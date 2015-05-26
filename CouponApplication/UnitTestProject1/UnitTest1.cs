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
            foreach(BusinessOwner o in db.BusinessOwners)
            {
                if(o.Email==Owner.Email)
                {
                    Assert.IsNull(result);
                    return;
                }
            }
            Assert.AreEqual("businessManagment", result.RouteValues["action"]);
        }

        //Addbusiness
     /*   [TestMethod]
        public void TestAddbusiness()
        {
            ManagerController controller = new ManagerController("1212");
            Business B = new Business();
            B.Adress = "naharya";
            B.City = "naharya";
            B.Name = "castro";
            B.Status = "מאושר";
            B.Coupons = new List<Coupon>();
            B.BusinessCategory = new List<Category>();
            B.Owner = new BusinessOwner();
            B.OwnerId = "1";
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
        }*/







        //SearchBusiness
     /*   [TestMethod]
        public void TestSearchBusiness()
        {
            CouponContext db = new CouponContext();
            BusinessController controller = new BusinessController();

            User use = new User();
            use.Email = "dodo@hotmail.com";
            var result = controller.MyPass(use) as RedirectToRouteResult;
            Assert.IsNull(result);
        }*/
    }
}
