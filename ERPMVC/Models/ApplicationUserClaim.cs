using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ApplicationUserClaim : IdentityUserClaim<int>
    {

        public string UserId { get; set; }

        public Guid PolicyId { get; set; }
    }
}
