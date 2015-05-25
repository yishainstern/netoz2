using CouponApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CouponApplication.Controllers
{
    public class UserController : Controller
    {
        private CouponContext db = new CouponContext();
        private static List<Coupon> ListC = null;
        private static List<Business> ListB = null;
        private static string coponID = null;
        private static string useID = null;
        private static string usecoponID = null;

        public UserController(){}

        public UserController(CouponContext c) 
        {
            db = c;
        }
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        //UserLogIn
        public ActionResult UserLogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(User u)
        {
            // this action is for handle post (login)
            if (ModelState.IsValid) // this is check validity
            {
                bool ans = chickLogIn(u);
                using (CouponContext dc = new CouponContext())
                {
                    var owner = dc.BusinessOwners.Where(a => a.Email.Equals(u.Email) && a.Password.Equals(u.Password)).FirstOrDefault();
                    var user = dc.Users.Where(a => a.Email.Equals(u.Email) && a.Password.Equals(u.Password)).FirstOrDefault();
                    var manager = dc.Manager.Where(a => a.Email.Equals(u.Email) && a.Password.Equals(u.Password)).FirstOrDefault();

                    if (ans == true)
                    {

                        if (manager != null)
                        {
                            return RedirectToAction("Index", "Manager");
                        }
                        else if (user != null)
                        {
                            Session["LogedUserID"] = user.PersonId.ToString();
                            Session["LogedUserFullname"] = user.Name.ToString();
                            useID = user.PersonId;
                            return RedirectToAction("AfterLogin");
                        }
                        
                        else if(owner != null)
                        {
                            return RedirectToAction("Index", "Business", new { id = owner.PersonId });   
                        }
                    }
                }
            }
            return View(u);
        }

        private bool chickLogIn(User u)
        {
            bool Pasword = true;
            bool Email = false;

            foreach (Person use in db.Person)
            {
                if (use.Email.Equals(u.Email))
                {
                    Email = true;
                    if (!use.Password.Equals(u.Password))
                    {
                        ModelState.AddModelError("PassEror", "--->Password error!!!");
                        Pasword = false;
                        break;
                    }
                }
            }
            if (Email == false)
            {
                ModelState.AddModelError("EmailEror", "--->Email error!!!");
            }
            if (Email == true && Pasword == true)
            {
                return true;
            }
            return false;
        }

        //ForgotPass
        public ActionResult ForgotPass()
        {
            return View();
        }

        //MyPass
        public ActionResult MyPass(User u)
        {
            bool ans = false;
            string pass = "";
            foreach (Person use in db.Person)
            {
                if (use.Email.Equals(u.Email))
                {
                    pass = use.Password;
                    ans = true;
                    break;
                }
            }

            if (ans == true)
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

                message.From = new System.Net.Mail.MailAddress("daved123daved@hotmail.com");
                message.To.Add(new System.Net.Mail.MailAddress(u.Email));

                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;
                message.Subject = "YourPassword";
                message.Body = "your password is: " + pass;

                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Send(message);
                return View();
            }
            else
            {
                TempData["SendEror"] = "--->Email error!!!";
                return RedirectToAction("ForgotPass");
            }
        }


        public ActionResult AfterLogin()
        {
            if (Session["LogedUserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        //UserSignUp
        public ActionResult UserSignUp()
        {
            var models = new AllModel();
            models.Categories = new List<Category>();
            foreach(Category cate in db.Categories)
            {
                models.Categories.Add(cate);
            }
            return View(models);
        }


        //SignUp
        [HttpPost]
        public ActionResult SignUp(AllModel models)
        {
            bool ans = ckickForm(models.User);
            if (ModelState.IsValid)
            {
                using (CouponContext dc = new CouponContext())
                {
                    var v = dc.Users.Where(a => a.Email.Equals(models.User.Email) || a.PersonId.Equals(models.User.PersonId)).FirstOrDefault();
                    if (v != null)
                    {
                        ModelState.AddModelError("Exist", "--->the Email or the Id was exist!!!");
                    }
                    if (v == null && ans == true)
                    {
                        models.User.UserPreferences = new List<Category>();
                        foreach (Category cate in models.Categories)
                        {
                            ans = false;
                            if (cate.Choose == true) 
                            {
                                ans = true;
                                Category C = db.Categories.Find(cate.CategoryId);
                                C.Users.Add(models.User);
                                models.User.UserPreferences.Add(C);
                            }
                        
                        }
                        if (ans == true)
                        {
                            db.Users.Add(models.User);
                            db.SaveChanges();
                            Session["LogedUserID"] = models.User.PersonId.ToString();
                            Session["LogedUserFullname"] = models.User.Name.ToString();
                            useID = models.User.PersonId;
                            return RedirectToAction("AddCategories", new { PersonID = useID });
                        }
                        else 
                        {
                            TempData["notice6"] = "--->you must add category!!!";
                            return RedirectToAction("UserSignUp");
                        }
                        
                    }
                }
            }
            return RedirectToAction("UserSignUp");
        }

        private bool ckickForm(User u)
        {
            bool ans = true;
            string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(emailRegex);
            if (string.IsNullOrEmpty(u.Name) || string.IsNullOrEmpty(u.PersonId) || string.IsNullOrEmpty(u.Password) || string.IsNullOrEmpty(u.Location) || u.gender == null || u.Age == null || string.IsNullOrEmpty(u.Email) || string.IsNullOrEmpty(u.Phone))
            {
                ModelState.AddModelError("Empty", "--->all fields are required!!!"); ans = false;
            }
            else if (!re.IsMatch(u.Email))
            {
                ModelState.AddModelError("Email", "--->Email is not valid!!!"); ans = false;
            }
            return ans;
        }


        //AddFavoriteCategory
     /*   public ActionResult AddFavoriteCategory(string PersonID)
        {
            var tuple = new Tuple<List<Category>, string>(db.Categories.ToList(), PersonID);
            return View(tuple);
        }*/

        //AddCategories
      //  [HttpPost]
        public ActionResult AddCategories(string PersonID="0")
        {
            return View();
        }
    }
}
