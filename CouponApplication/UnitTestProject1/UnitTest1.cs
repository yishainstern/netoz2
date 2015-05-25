using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CouponApplication;
using CouponApplication.Controllers;
using CouponApplication.Models;
using System.Web.Mvc;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        CouponContext db = new CouponContext();
        

        [TestMethod]
        public void TestSignIn()
        {
         /*   db.Categories.Add(new Category { Name = "aaa", Choose = false, CategoryId = "4" });
            db.Categories.Add(new Category { Name = "bbb", Choose = false, CategoryId = "5" });
            db.Categories.Add(new Category { Name = "ccc", Choose = false, CategoryId = "6" });
            db.SaveChanges();*/

            UserController controller = new UserController(db);
            User use = new User();
            use.PersonId = "111";
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
            m.Categories.Add(new Category { Name = "food", Choose = true, CategoryId = "1" });
            m.Categories.Add(new Category { Name = "eat", Choose = true, CategoryId = "2" });
            m.Categories.Add(new Category { Name = "muzic", Choose = true, CategoryId = "3" });
            var result = controller.SignUp(m) as RedirectToRouteResult;
            Assert.AreEqual("AfterLogin", result.RouteValues["AddCategories"]);
        }

        [TestMethod]
        public void TestLogIn()
        {
            //CouponContext db = new CouponContext();
            UserController controller = new UserController(db);
            User use = new User();
            use.Email = "dodo@hotmail.com";
            use.Password = "123";
            var result = controller.LogIn(use) as RedirectToRouteResult;
            Assert.AreEqual("AfterLogin", result.RouteValues["action"]);
        }

    }
}
