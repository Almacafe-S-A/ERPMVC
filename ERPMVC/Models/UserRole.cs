using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {

        // [UIHint("UserId")]
        public string UserId { get; set; }
        //[Required]
        //[UIHint("Roledropdown")]
        public string RoleId { get; set; }
        public string UserName { get; set; }

        public string RoleName { get; set; }

      
        //public ApplicationRole Role { get; set; }

  
        //public ApplicationRole User { get; set; }

    }




}
