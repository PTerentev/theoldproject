using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.UI;
using Domain;
using Domain.Entities;

namespace VoidInFateSite
{
    public class EfAccountsRepository
    {
        private EFDbContext context = new EFDbContext();
        
        

        public void Register(string login,string email,string password,out string ErrorMessage)
        {
            ErrorMessage = null;
            var newAcc = new Account
                {
                    Login = login,
                    Email = email,
                    Password = password,
                    FirstName = "Не указано",
                    SecondName = "Не указано",
                    IsAdmin = false,
                    IsModerator = false,
                    IsUser = true,
                    EmailConfirm = false
                }
            ;
            var list = context.Accounts.ToList();
            foreach (var acc in list)
            {
                if (acc.Email == newAcc.Email)
                {
                    ErrorMessage = "Такая почта уже зарегистрирована";
                    return;
                }
                if (acc.Login == newAcc.Login)
                {
                    ErrorMessage = "Такой логин уже зарегистрирован";
                    return;
                }
            }
            if(ErrorMessage == null)
            {
                context.Accounts.Add(newAcc);
                context.SaveChanges();
            }
        }

        public Account SignIn(string Email, string Password,out string message)
        {
            var list = context.Accounts.ToList();
            foreach (var acc in list)
            {
                if (acc.Email == Email)
                {
                    if (acc.Password == Password)
                    {
                        message = "ok";
                        return acc;
                    }
                }
            }
            message = "Ошибка ввода!";
            return null;
        }

        public void RemoveInfo(Account thisAccount)
        {
            context.Accounts.AddOrUpdate(thisAccount);
            context.SaveChanges();

        }
    }
}