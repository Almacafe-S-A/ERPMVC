using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPMVC.Models
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {

        // [UIHint("UserId")]
        [Required]
        public Guid UserId { get; set; }
        [Required]
        //[UIHint("Roledropdown")]
        public Guid RoleId { get; set; }
        public string UserName { get; set; }

        public string RoleName { get; set; }

      
        //public ApplicationRole Role { get; set; }

  
        //public ApplicationRole User { get; set; }

    }




}
