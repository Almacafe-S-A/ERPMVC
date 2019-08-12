using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ERPMVC.Models;
using ERPMVC.DTO;

namespace ERP.Contexts
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole, Guid, ApplicationUserClaim, ApplicationUserRole, AspNetUserLogins,
        AspNetRoleClaims, AspNetUserTokens>
    // public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var Customers = new List<Customer>()
            //{
            //     new Customer(){CustomerId=1,CustomerName="CAFE TOSTANO"},
            //     new Customer(){CustomerId=2,CustomerName="CAFE INDIO"},
            //};

            //modelBuilder.Entity<Customer>().HasData(Customers);

            base.OnModelCreating(modelBuilder);
        }



      


        
    }
}
