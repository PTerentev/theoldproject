using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsModerator { get; set; }
        public bool IsUser { get; set; }
        public bool EmailConfirm { get; set; }
    }
}
