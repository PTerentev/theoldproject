using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using ConcervantGames.SiteForGame;
using Domain;
using Domain.Entities;

namespace VoidInFateSite.Controllers
{
    public class RecentComparer : IComparer<Recent>
    {
        public int Compare(Recent o1, Recent o2)
        {
            if (o1.Time > o2.Time)
            {
                return 1;
            }
            else if (o1.Time < o2.Time)
            {
                return -1;
            }

            return 0;
        }
    }
    public class HomeController : Controller
    {

        private EFDbContext dbContext = new EFDbContext();
        private EfAccountsRepository repository = new EfAccountsRepository();
        [HttpPost]
        public ActionResult Create(string Caption, string Text, string FullText, byte[] LinkToSmallImage, byte[] LinkToLargeImage, string Author)
        {
            if (!(String.IsNullOrEmpty(Caption) && String.IsNullOrEmpty(Text) && String.IsNullOrEmpty(FullText) && String.IsNullOrEmpty(Author) &&
                  LinkToSmallImage == null))
            {

                Recent recent = new Recent()
                {
                    Author = Author,
                    Caption = Caption,
                    Text = Text,
                    FullText = FullText,
                    LinkSmallImg = LinkToSmallImage,
                    LinkLargeImg = LinkToLargeImage,

                };
                recent.Time = DateTime.Now;
                try
                {
                    dbContext.Recents.Add(recent);
                    dbContext.SaveChanges();
                    string ok = "Ok";
                    return PartialView(ok);
                }
                catch
                {
                    return PartialView("Error");
                }

            }
            string mess = "Не все поля были заполнены";
            return PartialView(mess);
        }
        [HttpPost]
        public ActionResult AddNews()
        {
            var recents = dbContext.Recents.ToList();
            List<Recent> items = new List<Recent>();
            for (int i = 0; i < 4; i++)
            {
                Recent cacheitem = new Recent();
                cacheitem.Time = DateTime.MinValue;
                if (recents.Count == 0)
                    break;
                foreach (var item in recents)
                {

                    if (item.Time > cacheitem.Time)
                        cacheitem = item;
                }
                if (cacheitem.Time != DateTime.MinValue)
                {
                    if (i > 2)
                        items.Add(cacheitem);
                    recents.Remove(cacheitem);
                }
            }
            return PartialView(items);
        }

        [HttpPost]
        public ActionResult Register(string UserName, string Email, string ConfirmPassword, string Password)
        {

            if (String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password) || String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(ConfirmPassword))
            {


                string model = "Не все данные были введены";
                
                return View("Register",model:model);
            }
            if (Password == ConfirmPassword)
            {
                Check test = new Check();
                if (test.CheckingMail(Email))
                {
                    if (test.CheckingLogin(UserName))
                    {
                        if (test.CheckingPassword(Password))
                        {

                            repository.Register(UserName, Email, Password, out string Message);
                            if (Message == null)
                            {


                                string mes = "Вы зарегистрированны!";
                                    
                                
                                return View("Register", model: mes);
                            }
                            else
                            {

                                return View("Register", model: Message);
                            }
                            
                        }
                        else
                        {
                            string mes = "Неправильное заполнение полей";
                            return View("Register", model: mes);
                        }
                    }
                    else
                    {
                        string mes = "Неправильное заполнение полей";
                        return View("Register", model: mes);
                    }
                }

                else
                {
                    string mes = "Неправильное заполнение полей";
                    return View("Register", model: mes);
                }
            }
            else
            {
                string mes = "Ошибка в подтверждении пароля";
                return View("Register", model: mes);
            }
        }
        public ActionResult Sign_in()
        {
            HttpCookie cookie = Request.Cookies["Account"];
            Account account = new Account();
            if (cookie != null)
            {
                if (repository.SignIn(cookie["Email"], cookie["Password"], out string message1) != null)
                {
                    account = repository.SignIn(cookie["Email"], cookie["Password"], out string message);
                    ViewBag.Login = account.Login;
                    return View("Index");
                }
                else
                {
                    HttpCookie cookie2 = new HttpCookie("Account");
                    cookie2.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie2);
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Changing(string Id,string Name ,string ChangeItem,string ChangeItemConfirm)
        {
            HttpCookie cookie = Request.Cookies["Account"];
            Account account = new Account();
            if (cookie != null)
            {


                if (repository.SignIn(cookie["Email"], cookie["Password"], out string message1) != null)
                {
                    account = repository.SignIn(cookie["Email"], cookie["Password"], out string message2);
                    ViewBag.Login = account.Login;
                    if (!String.IsNullOrEmpty(ChangeItem))
                        if (ChangeItem == ChangeItemConfirm)
                        {
                            if (Name == "Password")
                            {
                                Check check = new Check();
                                if (check.CheckingPassword(ChangeItem))
                                {
                                    var accounts = dbContext.Accounts.ToList();
                                    foreach (var item in accounts)
                                    {
                                        if (item.Id == int.Parse(Id))
                                        {
                                            item.Password = ChangeItem;
                                            try
                                            {
                                                dbContext.Accounts.AddOrUpdate(item);
                                                dbContext.SaveChanges();
                                            }
                                            catch (Exception e)
                                            {
                                                return View("Index");
                                            }
                                            cookie["Password"] = item.Password;
                                            Response.Cookies.Add(cookie);
                                            string mess = "Изменено!";
                                            return PartialView(mess);
                                        }
                                    }
                                }
                            }
                            if (Name == "Email")
                            {
                                Check check = new Check();
                                if (check.CheckingMail(ChangeItem))
                                {
                                    var accounts = dbContext.Accounts.ToList();
                                    foreach (var item in accounts)
                                    {
                                        if (item.Id == int.Parse(Id))
                                        {
                                            item.Email = ChangeItem;
                                            try
                                            {
                                                dbContext.Accounts.AddOrUpdate(item);
                                                dbContext.SaveChanges();
                                            }
                                            catch (Exception e)
                                            {
                                                return View("Index");
                                            }
                                            cookie["Email"] = item.Email;
                                            Response.Cookies.Add(cookie);
                                            string mess = "Изменено!";
                                            return PartialView(mess);
                                        }
                                    }
                                }
                            }
                            return View("Index");
                        }

                    return View("Index");
                }
                else
                {
                    HttpCookie cookie2 = new HttpCookie("Account");
                    cookie2.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie2);
                }
            }
            return View("Index");
        }
        
        public ActionResult Change(string Id)
        {
            HttpCookie cookie = Request.Cookies["Account"];
            Account account = new Account();

            if (cookie != null)
            {


                if (repository.SignIn(cookie["Email"], cookie["Password"], out string message2) != null)
                {
                    account = repository.SignIn(cookie["Email"], cookie["Password"], out string message22);
                    ViewBag.Id = Id;
                    ViewBag.Login = account.Login;
                    return View(account);
                }
                else
                {
                    HttpCookie cookie2 = new HttpCookie("Account");
                    cookie2.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie2);
                    return View("Index");
                }
            }
            ViewBag.Name = Id;
            return View("Index");
        }

        [HttpPost]
        public ActionResult Sign_in(string Email, string Password, string Check)
        {
            if (Email != null && Password != null)
            {


                if (repository.SignIn(Email, Password,out string message) != null)
                {
                    Account account = repository.SignIn(Email, Password, out string message1);
                    HttpCookie cookie = new HttpCookie("Account");
                    if (Check == "remember-me")
                    {
                        cookie.Expires = DateTime.Now.AddDays(1);
                    }
                    else
                        cookie.Expires = DateTime.Now.AddMinutes(20);


                    cookie["Login"] = account.Login;
                    cookie["Email"] = account.Email;
                    cookie["Password"] = account.Password;
                    Response.Cookies.Add(cookie);

                    ViewBag.Login = cookie["Login"];

                    return Index();
                }
                ViewBag.Message = message;
                return View("Sign_in");
            }
            ViewBag.Message = "Не все данные были введены";
            return View("Sign_in");
        }
        public ActionResult Register()
        {

            HttpCookie cookie = Request.Cookies["Account"];
            Account account = new Account();
            if (cookie != null)
            {


                if (repository.SignIn(cookie["Email"], cookie["Password"], out string message1) != null)
                {
                    account = repository.SignIn(cookie["Email"], cookie["Password"], out string message);
                    ViewBag.Login = account.Login;
                    return View("Index");
                }
                else
                {
                    HttpCookie cookie2 = new HttpCookie("Account");
                    cookie2.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie2);
                }
            }
            return View();

        }
        public ActionResult Unlogin()
        {
            HttpCookie cookie = new HttpCookie("Account");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            return View("Index");
        }
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["Account"];
            Account account = new Account();
            if (cookie != null)
            {


                if (repository.SignIn(cookie["Email"], cookie["Password"], out string message1) != null)
                {
                    account = repository.SignIn(cookie["Email"], cookie["Password"], out string message);
                    ViewBag.Login = account.Login;

                }
                else
                {
                    HttpCookie cookie2 = new HttpCookie("Account");
                    cookie2.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie2);
                }
            }
            try
            {
                var recents = dbContext.Recents.ToList();
                List<Recent> items = new List<Recent>();
                for (int i = 0; i < 2; i++)
                {
                    Recent cacheitem = new Recent();
                    cacheitem.Time = DateTime.MinValue;

                    foreach (var item in recents)
                    {

                        if (item.Time > cacheitem.Time)
                            cacheitem = item;
                    }
                    if (cacheitem.Time != DateTime.MinValue)
                    {
                        items.Add(cacheitem);
                        recents.Remove(cacheitem);
                    }
                }
                if (items.Count == 0)
                    return View("Index");
                RecentComparer RC = new RecentComparer();
                items.Sort(RC);
                return View("Index", items);
            }
            catch 
            {
                return View();

            }
            
           
        }


        public ActionResult About()
        {
            HttpCookie cookie = Request.Cookies["Account"];
            Account account = new Account();
            if (cookie != null)
            {


                if (repository.SignIn(cookie["Email"], cookie["Password"], out string message1) != null)
                {
                    account = repository.SignIn(cookie["Email"], cookie["Password"], out string message3);
                    ViewBag.Login = account.Login;

                }
                else
                {
                    HttpCookie cookie2 = new HttpCookie("Account");
                    cookie2.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie2);
                }
            }
            return View();
        }

      
        

        public ActionResult Download()
        {
            HttpCookie cookie = Request.Cookies["Account"];
            Account account = new Account();
            if (cookie != null)
            {


                if (repository.SignIn(cookie["Email"], cookie["Password"], out string message1) != null)
                {
                    account = repository.SignIn(cookie["Email"], cookie["Password"], out string message2);
                    ViewBag.Login = account.Login;

                }
                else
                {
                    HttpCookie cookie2 = new HttpCookie("Account");
                    cookie2.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie2);
                }
            }
            return View();
        }
        public new ActionResult Profile()
        {
            HttpCookie cookie = Request.Cookies["Account"];
            Account account = new Account();
            if (cookie != null)
            {


                if (repository.SignIn(cookie["Email"], cookie["Password"], out string message1) != null)
                {
                    account = repository.SignIn(cookie["Email"], cookie["Password"], out string message);
                    ViewBag.Login = cookie["Login"];
                    return View(account);
                }
                else
                {
                    HttpCookie cookie2 = new HttpCookie("Account");
                    cookie2.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie2);
                }
            }


            return View("Sign_in");
        }

        public ActionResult Admin()
        {
            HttpCookie cookie = Request.Cookies["Account"];
            Account account = new Account();
            if (cookie != null)
            {


                if (repository.SignIn(cookie["Email"], cookie["Password"], out string message1) != null)
                {
                    account = repository.SignIn(cookie["Email"], cookie["Password"], out string message12);
                    ViewBag.Login = account.Login;
                   
                    
                        return View();
                    
                }
                else
                {
                    HttpCookie cookie2 = new HttpCookie("Account");
                    cookie2.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie2);
                }
            }
            return View("Index");
        }
        [HttpPost]
        public ActionResult CodeResult(string code)
        {
            var images = dbContext.CodeImages.ToList();
            foreach (var image in images)
            {
                if (image.Code == code)
                {
                    return PartialView(image);
                }
            }
            return PartialView(null);
        }
        public ActionResult code()
        {
            return View();
        }
        public ActionResult News(int Id = 9999)
        {
            if (Id != 9999)
            {
                var news = dbContext.Recents.ToList();
                foreach (var recent in news)
                {
                    if (recent.Id == Id)
                    {
                        return View("ThisNews",recent);
                    }
                }
                
            }
            HttpCookie cookie = Request.Cookies["Account"];
            Account account = new Account();
            if (cookie != null)
            {


                if (repository.SignIn(cookie["Email"], cookie["Password"], out string message1) != null)
                {
                    account = repository.SignIn(cookie["Email"], cookie["Password"], out string message);
                    ViewBag.Login = account.Login;

                }
                else
                {
                    HttpCookie cookie2 = new HttpCookie("Account");
                    cookie2.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie2);
                }
            }
            var recents = dbContext.Recents.ToList();
            List<Recent> items = new List<Recent>();
            for (int i = 0; i < 8; i++)
            {
                Recent cacheitem = new Recent();
                cacheitem.Time = DateTime.MinValue;

                foreach (var item in recents)
                {

                    if (item.Time > cacheitem.Time)
                        cacheitem = item;
                }
                if (cacheitem.Time != DateTime.MinValue)
                {
                    items.Add(cacheitem);
                    recents.Remove(cacheitem);
                }
            }
            if (items.Count == 0)
                return View();
            RecentComparer RC = new RecentComparer();
            items.Sort(RC);
            return View(items);
        
        }
    }
}