using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;


namespace Domain
{
    public class EFDbContext : DbContext
    {
        public EFDbContext(): base("EFDbContext")
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Recent> Recents { get; set; }
        public DbSet<CodeImage> CodeImages { get; set; }
    }
}
