using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPAPI.Models
{
    public class ApplicationUserClaim : IdentityUserClaim<int>
    {
       // public int Id { get; set; }

        public string UserId { get; set; }

        public Guid PolicyId { get; set; }
    }
}
